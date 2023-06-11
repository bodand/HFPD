namespace libHFTests.Submissions

open System
open Xunit
open libHF.Submissions
open libHF.Classes

module GradingTests =
    let point = PointGuideline("OO design", 2)
    let errata = ErrataGuideline("typecheck", -2)

    [<Fact>]
    let ``Grading can be constructed with valid positive points`` () =
        let exc = Record.Exception(fun () -> Grading(point, 2) |> ignore)
        Assert.Null exc

    [<Fact>]
    let ``Grading can be constructed with valid negitve points`` () =
        let exc = Record.Exception(fun () -> Grading(errata, -2) |> ignore)
        Assert.Null exc

    [<Fact>]
    let ``Grading throws if to be constructed with invalid positive points`` () =
        let exc = Record.Exception(fun () -> Grading(errata, 2) |> ignore)
        Assert.NotNull exc
        Assert.IsType<ArgumentException> exc |> ignore

    [<Fact>]
    let ``Grading throws if to be constructed with invalid negitve points`` () =
        let exc = Record.Exception(fun () -> Grading(point, -2) |> ignore)
        Assert.NotNull exc
        Assert.IsType<ArgumentException> exc |> ignore

    [<Fact>]
    let ``Grading constructor sets received points`` () =
        let gr = Grading(point, 2)
        Assert.Equal(2, gr.ReceivedPoints)

    [<Fact>]
    let ``Grading constructor sets the graded Guideline`` () =
        let gr = Grading(point, 2)
        Assert.Same(point, gr.Guideline)
