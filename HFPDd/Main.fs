namespace HFPDd

open System.Net
open System.Net.Sockets
open HFPDd.Core
open HFPDd.Network

module Main =
    let runtime = RuntimeManager()

    let handler baggage read sender = async { return TerminateServer () }


    [<EntryPoint>]
    let main args =
        try
            let ep = IPEndPoint(IPAddress.Any, 3333)
            let socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            socket.Bind(ep)
            socket.Listen()
            
            use asock = new SocketAdapter(socket)
            use server = TcpServer.Start(AsyncSocket(asock), TcpServer.WrapClientHandler(handler))
            
            server.Run()
            0
        with e ->
            printfn $"fatal: %s{e.Source}: %s{e.Message}"
            printfn $"%s{e.StackTrace}"
            1
