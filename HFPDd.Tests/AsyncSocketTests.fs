module HFPDd.Tests.AsyncSocketTests

open System
open System.Net.Sockets
open HFPDd.Network
open Moq
open Xunit

[<Fact>]
let ``AsyncSocket AsyncAccept returns async IAsyncSocket type`` () =
    let mock = Mock<ISocket>()

    mock
        .Setup(fun s -> s.BeginAccept(It.IsAny<AsyncCallback>(), It.IsAny<Object>()))
        .Returns<IAsyncResult>(null)
    |> ignore

    mock.Setup(fun s -> s.EndAccept(null)).Returns(Mock<ISocket>().Object) |> ignore

    let asock: IAsyncSocket = AsyncSocket(mock.Object)
    Assert.Equal(typeof<Async<IAsyncSocket>>, asock.AsyncAccept().GetType())

[<Fact>]
let ``AsyncSocket's backing type returns the socket it was constructed with`` () =
    let mock = Mock<ISocket>()
    let msock = mock.Object
    let asock = AsyncSocket(msock)
    Assert.Same(msock, asock.Backing)

[<Fact>]
let ``AsyncSocket's Close closes the Socket`` () =
    let mock = Mock<ISocket>()
    let asock: IAsyncSocket = AsyncSocket(mock.Object)
    asock.Close()
    mock.Verify(fun s -> s.Close())

[<Fact>]
let ``AsyncSocket's Shutdown shuts down the Socket`` () =
    let sd = SocketShutdown.Both
    let mock = Mock<ISocket>()
    let asock: IAsyncSocket = AsyncSocket(mock.Object)
    asock.Shutdown(sd)
    mock.Verify(fun s -> s.Shutdown(sd))

[<Fact>]
let ``AsyncSocket AsyncReceive wraps BeginReceive/EndReceive`` () =
    async {
        let bytes: byte array = Array.zeroCreate 1
        let mock = Mock<ISocket>(MockBehavior.Strict)
        let ares = Mock<IAsyncResult>()
        ares.Setup(fun x -> x.CompletedSynchronously).Returns(true) |> ignore

        mock
            .Setup(fun s ->
                s.BeginReceive(It.IsNotNull(), 0, 1, SocketFlags.None, It.IsAny<AsyncCallback>(), It.IsAny<Object>()))
            .Returns(ares.Object)
        |> ignore

        mock.Setup(fun s -> s.EndReceive(It.IsAny<IAsyncResult>())).Returns(42) |> ignore

        let asock: IAsyncSocket = AsyncSocket(mock.Object)
        let! x = asock.AsyncReceive(bytes, Some 0, Some 1)
        Assert.Equal(42, x)
    }

[<Fact>]
let ``AsyncSocket AsyncReceive wraps BeginSend/EndSend`` () =
    async {
        let bytes: byte array = Array.zeroCreate 1
        let mock = Mock<ISocket>(MockBehavior.Strict)
        let ares = Mock<IAsyncResult>()
        ares.Setup(fun x -> x.CompletedSynchronously).Returns(true) |> ignore

        mock
            .Setup(fun s ->
                s.BeginSend(It.IsNotNull(), 0, 1, SocketFlags.None, It.IsAny<AsyncCallback>(), It.IsAny<Object>()))
            .Returns(ares.Object)
        |> ignore

        mock.Setup(fun s -> s.EndSend(It.IsAny<IAsyncResult>())).Returns(42) |> ignore

        let asock: IAsyncSocket = AsyncSocket(mock.Object)
        let! x = asock.AsyncSend(bytes, Some 0, Some 1)
        Assert.Equal(42, x)
    }
