namespace libHF.Classes

open System
open libHF.Student
open libHF.Submissions.Submission

type Homework(students: List<Student>, grading: List<GradingGuideline>) =
    let pointsComponent private guideline =
        match guideline with
        | PointGuideline(_, max) -> max
        | ErrataGuideline _ -> 0

    let max_pts = List.reduce (+) <| List.map pointsComponent grading

    do
        if max_pts < 0 then
            raise
            <| ArgumentOutOfRangeException $"Invalid value provided for max_pts: {max_pts} is negative"

    let mutable submissions = List<Submission>.Empty

    member val MaxPoints = max_pts with get
    member val Students = students with get
    member val Grading = grading with get

    member this.Submissions = submissions

    member this.AddSubmission(s: Submission) = submissions <- s :: submissions
