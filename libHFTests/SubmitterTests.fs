module SubmitterTests

open System
open Xunit
open libHF.Submissions

[<Fact>]
let ``Submitter's Name property returns Submitter's name`` () =
    let sut = Submitter("Teszt Béla", "AABB11")
    Assert.Equal(sut.Name, "Teszt Béla")

[<Fact>]
let ``Submitter's Neptun property returns Submitter's neptun`` () =
    let sut = Submitter("Teszt Béla", "AABB11")
    Assert.Equal(sut.Neptun, "AABB11")
