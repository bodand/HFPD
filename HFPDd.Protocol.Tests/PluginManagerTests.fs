module HFPDd.Protocol.Tests.PluginManagerTests

open HFPDd.Core
open HFPDd.Protocol
open HFPDd.Protocol.Action
open HFPDd.Protocol.Protocol
open Xunit
open Moq

[<Fact>]
let ``PluginManager returns None for Actions that do not exist`` () =
    let pm: IActionRegistry = PluginManager()
    Assert.Equal(None, pm.GetAction("something"))

[<Fact>]
let ``PluginManager returns None for Protocols that do not exist`` () =
    let pm: IProtocolRegistry = PluginManager()
    Assert.Equal(None, pm.GetProtocol("something"))

let dummyAction =
    { new IAction with
        member this.Process(payload) =
            async { return RunStatus.Failure "dummy" } }

let dummyProtocol =
    { new IProtocol with
        member this.Process(payload) = ParsingStatus.Failed "dummy" }

[<Fact>]
let ``PluginManager returns a valid Action for registered ActionFactories`` () =
    let pm = PluginManager()
    let reg: IActionRegistry = pm
    let load: IActionLoader = pm
 
    let mock = Mock<IActionFactory>()
    mock.Setup(fun f -> f.BuildAction()).Returns(dummyAction) |> ignore
    load.AddFactory "dummy" mock.Object

    let dummy = reg.GetAction("dummy")

    match dummy with
    | None -> Assert.Fail("dummy action was None")
    | Some da -> Assert.Same(dummyAction, da)

[<Fact>]
let ``PluginManager returns a valid Protocol for registered ProtocolFactories`` () =
    let pm = PluginManager()
    let preg: IProtocolRegistry = pm
    let areg: IActionRegistry = pm
    let load: IProtocolLoader = pm
    
    let mock = Mock<IProtocolFactory>()
    mock.Setup(fun f -> f.BuildProtocol(areg)).Returns(dummyProtocol) |> ignore
    load.AddFactory "dummy" mock.Object

    let dummy = preg.GetProtocol("dummy")

    match dummy with
    | None -> Assert.Fail("dummy protocol was None")
    | Some dp -> Assert.Same(dummyProtocol, dp)
