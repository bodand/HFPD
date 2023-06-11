namespace HFPDd.Core

[<AbstractClass>]
type ProtocolCommand() =
    abstract Execute: unit -> unit
