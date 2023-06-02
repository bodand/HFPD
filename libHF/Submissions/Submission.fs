namespace libHF.Submissions

open System
open System.IO
open libHF.Common

module Submission =
    type SubmissionStatus =
        | Accepted of message: string * points: int
        | Rejected of message: string
        | AutomaticFailure of message: string
        | Running
        | Unknown

    type Submission(submitter: Submitter, status: SubmissionStatus, date: DateTime, files: List<File>) =
        let mutable current_status = status

        interface IPrettyPrintable with
            member this.RenderText() =
                $"Submission '{(submitter :> IPrettyPrintable).RenderText}'"

        static member public Make submitter files =
            Submission(submitter, Unknown, DateTime.Now, files)

        member val Submitter = submitter with get
        member val Date = date with get
        member val Files = files with get

        member this.Status
            with get () = current_status
            and private set (value) = current_status <- value
        
        member this.StartRunning = this.Status <- Running
        member this.AutoFail msg = this.Status <- AutomaticFailure msg
        member this.Accept msg pts = this.Status <- Accepted (msg, pts)
        member this.Reject msg = this.Status <- Rejected msg