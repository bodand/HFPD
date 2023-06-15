module HFPDd.Core.Tests.RuntimeManagerTests

open HFPDd.Core
open Xunit

[<Fact>]
let ``RuntimeManager does not have an ID when constructed`` () =
    let rtm = RuntimeManager()
    Assert.Equal(rtm.Id, None)

[<Fact>]
let ``RuntimeManager sets its ID on first Initialize`` () =
    let rtm = RuntimeManager()
    let exc = Record.Exception(fun () -> rtm.Initialize("X"))
    Assert.Null(exc)
    Assert.Equal(Some "X", rtm.Id)

[<Fact>]
let ``RuntimeManager throws on second Initialize`` () =
    let rtm = RuntimeManager()
    let exc = Record.Exception(fun () -> rtm.Initialize("X"))
    Assert.Null(exc)
    let exc = Record.Exception(fun () -> rtm.Initialize("Y"))
    Assert.IsType<RuntimeAlreadyHasIdException>(exc) |> ignore
    Assert.Equal(Some "X", rtm.Id)

[<Fact>]
let ``RuntimeManager is not terminated on initialization `` () =
    let rtm = RuntimeManager()
    Assert.False(rtm.Terminated)

[<Fact>]
let ``RuntimeManager Terminate method terminates the runtime object`` () =
    let rtm = RuntimeManager()
    rtm.Initialize("X")
    let exc = Record.Exception(fun () -> rtm.Terminate("X"))
    Assert.Null(exc)
    Assert.True(rtm.Terminated)

[<Fact>]
let ``RuntimeManager throws if a Terminate is called twice`` () =
    let rtm = RuntimeManager()
    rtm.Initialize("X")
    let exc = Record.Exception(fun () -> rtm.Terminate("X"))
    Assert.Null(exc)
    let exc = Record.Exception(fun () -> rtm.Terminate("X"))
    let rte = Assert.IsType<RuntimeTerminatedException>(exc)
    Assert.True(rtm.Terminated)
    Assert.Equal("X", rte.id)

[<Fact>]
let ``RuntimeManager throws if Initialize is called after Terminate`` () =
    let rtm = RuntimeManager()
    rtm.Initialize("X")
    rtm.Terminate("X")
    let exc = Record.Exception(fun () -> rtm.Initialize("X"))
    Assert.IsType<RuntimeTerminatedException>(exc) |> ignore
    Assert.True(rtm.Terminated)
