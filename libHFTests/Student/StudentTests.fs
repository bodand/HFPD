namespace libHFTests.Student

open System
open Xunit
open libHF.Student

module StudentTests =
    [<Fact>]
    let ``Students compares before unkowns`` () =
        let std1 = Student("John", Neptun "999999") :> IComparable
        Assert.Equal(1, std1.CompareTo(1))
        
    [<Theory>]
    [<InlineData("ABCDEF", "ABCDEF", 0)>]
    [<InlineData("ABCDEF", "FEDCBA", -1)>]
    [<InlineData("FEDCBA", "ABCDEF", 1)>]
    let ``Students compare equally to their Neptun values`` n1 n2 res =
        let std1 = Student("John", Neptun n1) :> IComparable
        let std2 = Student("John", Neptun n2)
        Assert.Equal(res, std1.CompareTo(std2))

    [<Theory>]
    [<InlineData("ABCDEF", "ABCDEF", true)>]
    [<InlineData("ABCDEF", "FEDCBA", false)>]
    [<InlineData("FEDCBA", "ABCDEF", false)>]
    let ``Students are equal iff their Neptun codes`` n1 n2 res =
        let std1 = Student("John", Neptun n1)
        let std2 = Student("John", Neptun n2)
        Assert.Equal(res, std1.Equals(std2))

    [<Fact>]
    let ``Student has the same hash as their Neptun`` () =
        let std = Student("John", Neptun "ABCDEF") 
        Assert.Equal("ABCDEF".GetHashCode(), std.GetHashCode())
