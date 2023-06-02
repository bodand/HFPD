namespace libHF.Submissions

open libHF.Common

type Submitter(name: string, neptun: string) =
    interface IPrettyPrintable with
        member this.RenderText() = $"{name} ({neptun})"
    
    member val Name = name with get, set
    member val Neptun = neptun with get   
    