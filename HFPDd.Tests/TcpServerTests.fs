module HFPDd.Tests.TcpServerTests

open System
open System.Net.Sockets
open HFPDd.Network
open Xunit
open Moq
open HFPDd

[<Fact>]
let ``Server can wrap a SimpleHandle to a ClientWrapper`` () =
    async {
        let mock = Mock<IAsyncSocket>(MockBehavior.Strict)

        mock
            .Setup(fun s -> s.AsyncReceive(It.IsNotNull(), It.IsAny(), It.IsAny()))
            .Returns(async { return 42 })
        |> ignore

        let handler bg read send = async { return TerminateServer 42 }
        let wrapped = TcpServer<int>.WrapClientHandler(handler)
        let! x = wrapped mock.Object
        Assert.Equal(x, TerminateServer 42)
    }

[<Fact>]
let ``TcpServer wrapped handler receives read string`` () =
    async {
        let mock = Mock<IAsyncSocket>(MockBehavior.Strict)

        mock
            .Setup(fun s -> s.AsyncReceive(It.IsNotNull(), It.IsAny(), It.IsAny()))
            .Returns(async { return 42 })
        |> ignore

        let handler bg (read: string) send =
            async { return TerminateServer read.Length }

        let wrapped = TcpServer<int>.WrapClientHandler(handler)
        let! x = wrapped mock.Object
        Assert.Equal(x, TerminateServer 42)
    }

[<Fact>]
let ``TcpServer wrapped handler can send data`` () =
    async {
        let mock = Mock<IAsyncSocket>(MockBehavior.Strict)

        mock
            .Setup(fun s -> s.AsyncReceive(It.IsNotNull(), It.IsAny(), It.IsAny()))
            .Returns(async { return 42 })
        |> ignore

        mock
            .Setup(fun s -> s.AsyncSend(It.IsNotNull(), It.IsAny(), It.IsAny()))
            .Returns(async { return 69 })
        |> ignore

        let data = "data"

        let handler bg read send =
            async {
                let! sent = send data
                return TerminateServer sent
            }

        let wrapped = TcpServer<int>.WrapClientHandler(handler)
        let! x = wrapped mock.Object
        Assert.Equal(x, TerminateServer 69)
        mock.Verify(fun s -> s.AsyncSend(It.IsNotNull(), None, None))
    }

[<Fact>]
let ``TcpServer wrapped handler is called again with baggage`` () =
    async {
        let mock = Mock<IAsyncSocket>(MockBehavior.Strict)

        mock
            .Setup(fun s -> s.AsyncReceive(It.IsNotNull(), It.IsAny(), It.IsAny()))
            .Returns(async { return 42 })
        |> ignore

        mock
            .Setup(fun s -> s.AsyncSend(It.IsNotNull(), It.IsAny(), It.IsAny()))
            .Returns(async { return 69 })
        |> ignore

        let handler (bg: string option) read send =
            async {
                return
                    match bg with
                    | None -> ContinueRequest "lm"
                    | Some value -> TerminateServer(value + "ao")
            }

        let wrapped = TcpServer.WrapClientHandler(handler)
        let! x = wrapped mock.Object
        Assert.Equal(x, TerminateServer "lmao")
    }

[<Fact>]
let ``TcpServer listens on an IAsyncSocket`` () =
    async {
        let mock = Mock<IAsyncSocket>()

        mock
            .Setup(fun s -> s.AsyncReceive(It.IsNotNull(), It.IsAny(), It.IsAny()))
            .Returns(async { return 42 })
        |> ignore

        mock
            .Setup(fun s -> s.AsyncSend(It.IsNotNull(), It.IsAny(), It.IsAny()))
            .Returns(async { return 69 })
        |> ignore

        let serverMock = Mock<IAsyncSocket>()

        serverMock.Setup(fun s -> s.AsyncAccept()).Returns(async { return mock.Object })
        |> ignore

        let handler bg read send = async { return TerminateServer "lmao" }

        let run () =
            let wrapped = TcpServer.WrapClientHandler(handler)
    
            use server =
                TcpServer.Start(serverMock.Object, wrapped, (fun s -> Assert.Equal("lmao", s)))
            0

        run () |> ignore
        
        serverMock.Verify(fun s -> s.Close())
        serverMock.Verify(fun s -> s.Shutdown(SocketShutdown.Both))
    }
