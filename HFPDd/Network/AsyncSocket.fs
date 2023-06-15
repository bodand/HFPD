namespace HFPDd.Network

open System.Net.Sockets

type AsyncSocket(socket: ISocket) =
    interface IAsyncSocket with
        member this.AsyncAccept() =
            Async.FromBeginEnd(socket.BeginAccept, (fun x -> AsyncSocket(socket.EndAccept(x))))

        member this.AsyncReceive(buffer, ?offset, ?count) =
            let offset = defaultArg offset 0
            let count = defaultArg count buffer.Length

            let beginReceive (b, o, c, cb, s) =
                socket.BeginReceive(b, o, c, SocketFlags.None, cb, s)

            Async.FromBeginEnd(buffer, offset, count, beginReceive, socket.EndReceive)


        member this.AsyncSend(buffer, ?offset, ?count) =
            let offset = defaultArg offset 0
            let count = defaultArg count buffer.Length

            let beginSend (b, o, c, cb, s) =
                socket.BeginSend(b, o, c, SocketFlags.None, cb, s)

            Async.FromBeginEnd(buffer, offset, count, beginSend, socket.EndSend)

        member this.Close() = socket.Close()
        member this.Shutdown(sd) = socket.Shutdown(sd)

    member val Backing = socket
