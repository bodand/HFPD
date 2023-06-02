namespace libHFTests

module SubmitterTests =

    open System
    open Xunit
    open libHF.Submissions

    [<Fact>]
    let ``Submitter's Name property returns Submitter's name`` () =
        let sut = Submitter("Teszt Béla", Neptun "AABB11")
        Assert.Equal(sut.Name, "Teszt Béla")

    [<Fact>]
    let ``Submitter's Neptun property returns Submitter's neptun`` () =
        let sut = Submitter("Teszt Béla", Neptun "AABB11")
        Assert.Equal(sut.Neptun, Neptun "AABB11")

    [<Fact>]
    let ``Submitter's name can be changed`` () =
        let sut = Submitter("John", Neptun "AABB11")
        sut.Name <- "Teszt Béla"
        Assert.Equal(sut.Name, "Teszt Béla")

    [<Fact>]
    let ``Submitter's Neptun can be changed`` () =
        let sut = Submitter("John", Neptun "XXXXXX")
        sut.Neptun <- Neptun "ABCDEF"
        Assert.Equal(sut.Neptun, Neptun "ABCDEF")
