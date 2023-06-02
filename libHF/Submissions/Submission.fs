namespace libHF.Submissions

open System
open System.Collections.Generic
open System.IO
open libHF.Common

module Submission =
    type SubmissionStatus =
        | Accepted of message : string * points : int
        | Rejected of message : string
        | AutomaticFailure of message : string
                   
    type Submission( submitter: Submitter
                   , status : SubmissionStatus
                   , date : DateTime
                   , files : List<File>
                   ) =
        interface IPrettyPrintable with
            member this.RenderText() = $"Submission {submitter.Name}"