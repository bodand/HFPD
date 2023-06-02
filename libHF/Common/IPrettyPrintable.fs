namespace libHF.Common

[<Interface>]
type IPrettyPrintable =
    abstract member RenderText: unit -> string
