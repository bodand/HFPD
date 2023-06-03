namespace libHF.Classes

open System
open libHF.Student
open libHF.Submissions.Submission

/// <summary>
/// A given Homework in a Class.
/// </summary>
/// <remarks>
/// <para>
/// A Class can have multiple Homeworks assigned; this type defines the Homework for this relation.
/// </para>
/// <para>
/// A Homework is assigned to a set of students, and is defined with a list of grading guidelines for which the
/// points are distributed.
/// Each student can provide their own Submission for a Homework, if they are in the group of students the Homework
/// is assigned to.
/// </para>
/// </remarks>
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

    member this.AddSubmission(s: Submission) =
        let neptuns = List.map (fun (s: Student) -> s.Neptun) students

        if not <| List.contains s.Submitter.Neptun neptuns then
            raise <| ArgumentException "Student is not assigned this Homework"

        submissions <- s :: submissions
