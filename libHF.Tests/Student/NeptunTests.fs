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

    [<Fact>]
    let ``Neptun object has the same hash as its string`` () =
        let sut = Neptun "Ab34Ef"
        Assert.Equal("AB34EF".GetHashCode(), sut.GetHashCode())

    [<Fact>]
    let ``Neptun object compares before unknown types`` () =
        let sut = Neptun "Ab34Ef" :> IComparable
        Assert.Equal(1, sut.CompareTo(2))

    [<Theory>]
    [<InlineData("ABCDEF", "ABCDEF", 0)>]
    [<InlineData("ABCDEF", "FEDCBA", 1)>]
    [<InlineData("FEDCBA", "ABCDEF", -1)>]
    let ``Neptun object compares with strings`` str neptun res =
        let sut = Neptun neptun :> IComparable
        Assert.Equal(res, sut.CompareTo(str))

    [<Theory>]
    [<InlineData("ABCDEF", "ABCDEF", 0)>]
    [<InlineData("ABCDEF", "FEDCBA", 1)>]
    [<InlineData("FEDCBA", "ABCDEF", -1)>]
    let ``Neptun object compares with other Neptun objects`` str neptun res =
        let sut = Neptun neptun :> IComparable
        Assert.Equal(res, sut.CompareTo(Neptun str))
