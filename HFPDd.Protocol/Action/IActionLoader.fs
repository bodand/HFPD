namespace HFPDd.Protocol.Action

type IActionLoader =
    abstract AddFactory: action: string -> fact: IActionFactory -> unit
