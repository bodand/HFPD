module HFPDd.Protocol.Tests.ParingStatusTests

open Xunit
open HFPDd.Protocol.Protocol

[<Fact>]
let ``ParsingStatus can be a Failed object with a message`` () =
    let sut: ParsingStatus<string> = Failed "ill-formed"
    Assert.Equal(Failed "ill-formed", sut)

[<Fact>]
let ``ParsingStatus can be a Success object with a message`` () =
    let sut: ParsingStatus<string> = Success("(+ (- 4 2) 2)", None)
    Assert.Equal(Success("(+ (- 4 2) 2)", None), sut)

[<Fact>]
let ``ParsingStatus can be a Parsing object with undefined data`` () =
    let sut: ParsingStatus<string> = Parsing "4 - 2 +"
    Assert.Equal(Parsing "4 - 2 +", sut)
