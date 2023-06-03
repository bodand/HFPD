namespace libHFTests.Submissions

module SubmitterTests =

    open System
    open Xunit
    open libHF.Student
    open libHF.Submissions

    [<Fact>]
    let ``Submitter's Name property returns Submitter's name`` () =
        let sut =
            Submitter(
                { name = "Teszt Béla"
                  neptun = Neptun "AABB11" }
            )

        Assert.Equal(sut.Name, "Teszt Béla")

    [<Fact>]
    let ``Submitter's Neptun property returns Submitter's neptun`` () =
        let sut =
            Submitter(
                { name = "Teszt Béla"
                  neptun = Neptun "AABB11" }
            )

        Assert.Equal(sut.Neptun, Neptun "AABB11")

    [<Fact>]
    let ``Submitter's name can be changed`` () =
        let sut =
            Submitter(
                { name = "John"
                  neptun = Neptun "AABB11" }
            )

        sut.Name <- "Teszt Béla"
        Assert.Equal(sut.Name, "Teszt Béla")

    [<Fact>]
    let ``Submitter's Neptun can be changed`` () =
        let sut =
            Submitter(
                { name = "John"
                  neptun = Neptun "XXXXXX" }
            )

        sut.Neptun <- Neptun "ABCDEF"
        Assert.Equal(sut.Neptun, Neptun "ABCDEF")
