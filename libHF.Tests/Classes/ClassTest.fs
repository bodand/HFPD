namespace libHFTests.Classes

open Xunit
open libHF.Classes
open libHF.Student

module ClassTests =
    let students =
        [ Student("John", Neptun "UVWXYZ")
          Student("Foo Bar", Neptun "123456") ]

    let grading = [ PointGuideline("OO design", 2); ErrataGuideline("typecheck", 4) ]
    
    [<Fact>]
    let ``Class can be constructed by its name`` () =
        let exc = Record.Exception(fun() -> Class("Test") |> ignore)
        Assert.Null exc
        
    [<Fact>]
    let ``Class constructor sets name`` () =
        let klass = Class("Test")
        Assert.Equal("Test", klass.Name)
        
    [<Fact>]
    let ``Fresh Class object does not have homework`` () =
        let klass = Class("Test")
        Assert.Empty klass.Homeworks
        
    [<Fact>]
    let ``Homework can be added to Class object`` () =
        let klass = Class("Test")
        Assert.Empty klass.Homeworks
        
        let homework = Homework(students, grading)
        klass.AddHomework(homework)
        Assert.Contains (homework, klass.Homeworks)