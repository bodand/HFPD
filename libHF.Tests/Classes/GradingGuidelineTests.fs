namespace libHFTests.Classes

open Xunit
open libHF.Classes

module GradingGuidelineTests =
    [<Fact>]
    let ``A positive grading guideline can be constructed from a string and int`` () =
        let gr = PointGuideline("OO design", 1)
        Assert.NotNull gr

    [<Fact>]
    let ``A negative grading guideline can be constructed from a string and int`` () =
        let gr = ErrataGuideline("typecheck", 4)
        Assert.NotNull gr

    [<Fact>]
    let ``A positive grading guideline reports zero value as accepted`` () =
        let gr = PointGuideline("OO design", 1)
        Assert.True <| gr.AllowedPoints 0

    [<Fact>]
    let ``A positive grading guideline reports a non-edge-case value as accepted`` () =
        let gr = PointGuideline("OO design", 2)
        Assert.True <| gr.AllowedPoints 1
        
    [<Fact>]
    let ``A positive grading guideline reports a upper-edge case value as accepted`` () =
        let gr = PointGuideline("OO design", 2)
        Assert.True <| gr.AllowedPoints 2

    [<Fact>]
    let ``A positive grading guideline reports a negative value as not accepted`` () =
        let gr = PointGuideline("OO design", 2)
        Assert.False <| gr.AllowedPoints -2
        
    [<Fact>]
    let ``A positive grading guideline reports a too great value as not accepted`` () =
        let gr = PointGuideline("OO design", 2)
        Assert.False <| gr.AllowedPoints 42

    [<Fact>]
    let ``A negative grading guideline reports zero value as accepted`` () =
        let gr = ErrataGuideline("typecheck", 1)
        Assert.True <| gr.AllowedPoints 0

    [<Fact>]
    let ``A negative grading guideline reports a non-edge-case value as accepted`` () =
        let gr = ErrataGuideline("typecheck", 2)
        Assert.True <| gr.AllowedPoints -1
        
    [<Fact>]
    let ``A negative grading guideline reports a upper-edge case value as accepted`` () =
        let gr = ErrataGuideline("typecheck", 2)
        Assert.True <| gr.AllowedPoints -2

    [<Fact>]
    let ``A negative grading guideline reports a positive value as not accepted`` () =
        let gr = ErrataGuideline("typecheck", 2)
        Assert.False <| gr.AllowedPoints 3
        
    [<Fact>]
    let ``A negative grading guideline reports a too great value as not accepted`` () =
        let gr = ErrataGuideline("typecheck", 2)
        Assert.False <| gr.AllowedPoints -42

