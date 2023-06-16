namespace HFPDd

open System
open System.ComponentModel
open System.Net.Sockets
open HFPDd.Network

open System.Text
open System.Threading

type ProcessContinuation<'baggage> =
    | TerminateServer of 'baggage
    | FinishRequest of 'baggage
    | ContinueRequest of 'baggage

type ServerHandler<'baggage> = IAsyncSocket -> Async<ProcessContinuation<'baggage>>

type SimpleHandler<'baggage> =
    'baggage option -> string -> (string -> Async<int>) -> Async<ProcessContinuation<'baggage>>

type TcpServer<'baggage> =
    static member WrapClientHandler
        (
            client: SimpleHandler<'baggage>,
            ?enc: Encoding,
            ?bufSize: int
        ) : ServerHandler<'baggage> =
        let enc = defaultArg enc Encoding.UTF8
        let bufSize = defaultArg bufSize (4 * 1024)

        let stringSender (socket: IAsyncSocket) (enc: Encoding) =
            fun (str: string) ->
                let bytes = enc.GetBytes(str)
                socket.AsyncSend(bytes, None, None)

        let rec serverHandlerImpl (bg: 'baggage option) (socket: IAsyncSocket) =
            async {
                let bytes: byte[] = Array.zeroCreate bufSize
                let! (read: int) = socket.AsyncReceive(bytes, None, None)
                let str = enc.GetString(bytes, 0, read)
                let! continuation = client bg str <| stringSender socket enc

                match continuation with
                | TerminateServer baggage -> return TerminateServer baggage
                | FinishRequest baggage -> return FinishRequest baggage
                | ContinueRequest baggage -> return! serverHandlerImpl (Some baggage) socket
            }

        serverHandlerImpl None

    static member Start(serverSocket: IAsyncSocket, handle: ServerHandler<'baggage>, ?terminal: 'baggage -> unit) =
        let onFinish = defaultArg terminal (fun _ -> ())
        let cts = new CancellationTokenSource()

        let impl =
            { new IDisposable with
                member this.Dispose() =
                    cts.Cancel()
                    serverSocket.Close() }

        let rec dataLoop (handle: IAsyncSocket -> Async<ProcessContinuation<'baggage>>) =
            async {
                let mutable continueRunning = true
                let! connection = serverSocket.AsyncAccept()

                try
                    try
                        match! handle connection with
                        | TerminateServer baggage ->
                            onFinish baggage
                            continueRunning <- false
                        | FinishRequest baggage -> onFinish baggage
                        | ContinueRequest _ ->
                            raise (
                                InvalidAsynchronousStateException(
                                    "Server entered invalid state: main loop should continue request"
                                )
                            )
                    with e ->
                        printfn $"Error: %s{e.Message}"
                finally
                    connection.Shutdown(SocketShutdown.Both)
                    connection.Close()

                if not continueRunning then
                    return! async { return () }
                else
                    return! dataLoop handle
            }

        new ServerImplementation(Async.StartAsTask(dataLoop handle, cancellationToken = cts.Token), impl)
