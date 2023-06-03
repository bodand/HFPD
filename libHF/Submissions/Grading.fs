namespace libHF.Submissions

open System
open libHF.Classes

type Grading(guideline: GradingGuideline, received: int) =
    do
        if not <| guideline.AllowedPoints received then
            raise
            <| ArgumentException $"Illegal amount of points received on grade: {received}"

    member val Guideline = guideline with get
    member val ReceivedPoints = received with get
