module HFPDd.Core.Tests.RunStatusTests

open HFPDd.Core
open Xunit

[<Fact>]
let ``A RunStatus object can be a Failure object`` () =
    let x: RunStatus = Failure "lmao"
    Assert.Equal(Failure "lmao", x)

[<Fact>]
let ``A RunStatus object can be a Success object with all optional data omitted`` () =
    let x: RunStatus = Success("lmao", None, None)
    Assert.Equal( Success("lmao", None, None), x)

[<Fact>]
let ``A RunStatus object can be a Success object with error data omitted`` () =
    let x: RunStatus = Success("lmao", Some "output", None)
    Assert.Equal(Success("lmao", Some "output", None), x)

[<Fact>]
let ``A RunStatus object can be a Success object with output data omitted`` () =
    let x: RunStatus = Success("lmao", None, Some "error")
    Assert.Equal(Success("lmao", None, Some "error"), x)

[<Fact>]
let ``A RunStatus object can be a Success object with all data`` () =
    let x: RunStatus = Success("lmao", Some "output", Some "error")
    Assert.Equal(Success("lmao", Some "output", Some "error"), x)
