namespace libHFTests.Submissions

open System
open System.IO
open Xunit
open libHF.Student
open libHF.Submissions
open libHF.Submissions.Submission

module SubmissionTests =
    let subm =
        Submitter(
            Student("Teszt Béla", Neptun("AABBCC"))
        )

    [<Fact>]
    let ``Submission can be constructed with parameters`` () =
        let now = DateTime.Now
        let sut = Submission(subm, SubmissionStatus.Unknown, now, List.Empty)
        Assert.Empty(sut.Files)
        Assert.Equivalent(subm, sut.Submitter)
        Assert.Equal(sut.Date, now)
        Assert.Equal(sut.Status, SubmissionStatus.Unknown)

    [<Fact>]
    let ``Submission can be constructed with Make helper`` () =
        let sut = Submission.Make subm List.Empty
        Assert.Empty(sut.Files)
        Assert.Equivalent(subm, sut.Submitter)
        Assert.NotEqual(sut.Date, DateTime.Now)
        Assert.Equal(sut.Status, SubmissionStatus.Unknown)

    [<Fact>]
    let ``Submission can start running automatic tests`` () =
        let sut = Submission.Make subm List.Empty
        sut.StartRunning
        Assert.Equal(Running, sut.Status)

    [<Fact>]
    let ``Submission can be Rejected with message`` () =
        let sut = Submission.Make subm List.Empty
        sut.Reject "Reasoning"

        Assert.Equal(Rejected "Reasoning", sut.Status)

    [<Fact>]
    let ``Submission can be Rejected by automatic checks`` () =
        let sut = Submission.Make subm List.Empty
        sut.AutoFail "Reasoning"

        Assert.Equal(AutomaticFailure "Reasoning", sut.Status)

    [<Fact>]
    let ``Submission can be Accepted with message and points`` () =
        let sut = Submission.Make subm List.Empty
        sut.Accept "Reasoning" 42

        Assert.Equal(Accepted("Reasoning", 42), sut.Status)
