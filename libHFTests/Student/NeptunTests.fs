namespace libHFTests.Student

open System

module NeptunTests =

    open Xunit
    open libHF.Student

    [<Theory>]
    [<InlineData("AB34EF")>]
    [<InlineData("ABCDEF")>]
    [<InlineData("123456")>]
    let ``Neptun can be constructed from a valid NEPTUN code`` neptun =
        let exc = Record.Exception(fun () -> Neptun neptun |> ignore)
        Assert.Null exc

    [<Theory>]
    [<InlineData("")>]
    [<InlineData("X")>]
    [<InlineData("XXXXX")>]
    [<InlineData("XXXXXXX")>]
    let ``The strings with invalid length to be a NEPTUN code throw ArgumentException`` inv_code =
        let exc = Record.Exception(fun () -> Neptun inv_code |> ignore)
        Assert.IsType<ArgumentException> exc

    [<Theory>]
    [<InlineData("AB DEF")>]
    [<InlineData("XXXXXÁ")>]
    [<InlineData("XXXXX?")>]
    let ``The strings with invalid data to be a NEPTUN code throw ArgumentException`` inv_code =
        let exc = Record.Exception(fun () -> Neptun inv_code |> ignore)
        Assert.IsType<ArgumentException> exc

    [<Fact>]
    let ``Neptun codes are uppercase if input is uppercase`` () =
        let sut = Neptun "AB34EF"
        Assert.Equal(sut.value, "AB34EF")

    [<Fact>]
    let ``Neptun codes are uppercase if input is lowercase`` () =
        let sut = Neptun "ab34ef"
        Assert.Equal(sut.value, "AB34EF")

    [<Fact>]
    let ``Neptun codes are uppercase if input is mixed-case`` () =
        let sut = Neptun "Ab34Ef"
        Assert.Equal(sut.value, "AB34EF")
