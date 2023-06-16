namespace HFPDd.Protocol.Action

type IActionFactory =
    abstract BuildAction: unit -> IAction
