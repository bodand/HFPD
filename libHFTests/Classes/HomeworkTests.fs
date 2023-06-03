namespace libHFTests.Classes

open System
open Microsoft.FSharp.Collections
open Xunit
open libHF.Classes
open libHF.Student
open libHF.Submissions
open libHF.Submissions.Submission
open libHFTests

module HomeworkTests =
    let students =
        [ { name = "John"
            neptun = Neptun "UVWXYZ" }
          { name = "Foo Bar"
            neptun = Neptun "123456" } ]

    let grading = [ PointGuideline("OO design", 2); ErrataGuideline("typecheck", 4) ]
    let bad_grading = [ PointGuideline("OO design", -2) ]

    [<Fact>]
    let ``Homework can be constructed with required parameters`` () =
        let exc = Record.Exception(fun () -> Homework(students, grading) |> ignore)
        Assert.Null exc

    [<Fact>]
    let ``Homework cannot be constructed with bad grading`` () =
        let exc = Record.Exception(fun () -> Homework(students, bad_grading) |> ignore)
        Assert.NotNull exc
        Assert.IsType<ArgumentOutOfRangeException> exc |> ignore

    [<Fact>]
    let ``Homework adds together gradings to calculate the maximum points`` () =
        let hf = Homework(students, grading)
        Assert.Equal(2, hf.MaxPoints)

    [<Fact>]
    let ``Homework can be supplied a Submission`` () =
        let hf = Homework(students, grading)
        Assert.Empty hf.Submissions

        let sub =
            Submission.Make
                (Submitter
                    { name = "John"
                      neptun = Neptun "UVWXYZ" })
                List.Empty

        hf.AddSubmission sub

        Assert.Contains(sub, hf.Submissions)

    [<Fact>]
    let ``Homework constructor sets grades`` () =
        let hf = Homework(students, grading)
        Assert.Same(grading, hf.Grading)

    [<Fact>]
    let ``Homework constructor sets students`` () =
        let hf = Homework(students, grading)
        Assert.Same(students, hf.Students)
