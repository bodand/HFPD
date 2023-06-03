namespace libHFTests.Submissions

module SubmitterTests =

    open System
    open Xunit
    open libHF.Student
    open libHF.Submissions

    [<Fact>]
    let ``Submitter's Name property returns Submitter's name`` () =
        let sut = Submitter(Student("Teszt Béla", Neptun "AABB11"))

        Assert.Equal(sut.Name, "Teszt Béla")

    [<Fact>]
    let ``Submitter's Neptun property returns Submitter's neptun`` () =
        let sut = Submitter(Student("Teszt Béla", Neptun "AABB11"))

        Assert.Equal(sut.Neptun, Neptun "AABB11")

    [<Fact>]
    let ``Submitter's hash is the same as its Student's`` () =
        let student = Student("John", Neptun "XXXXXX")
        let sut = Submitter(student)
        Assert.Equal(student.GetHashCode(), sut.GetHashCode())

    [<Fact>]
    let ``Submitter's equal iff their wrapped students`` () =
        let student = Student("John", Neptun "XXXXXX")
        let sub1 = Submitter(student)
        let sub2 = Submitter(student)
        Assert.Equal(sub1, sub2)
        
    [<Fact>]
    let ``Submitter's equal to a student iff that is to their wrapped student`` () =
        let student = Student("John", Neptun "XXXXXX")
        let sub1 = Submitter(student)
        Assert.True(sub1.Equals(student))
        
    [<Fact>]
    let ``Submitter's inequal to unkown type's values`` () =
        let sut = Submitter(Student("John", Neptun "XXXXXX"))
        Assert.False(sut.Equals(42))
