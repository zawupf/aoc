module Tests02

open Xunit
open Day02

[<Fact>]
let ``Day02 Stars`` () =
    Assert.Equal("3765464", job1 ())
    Assert.Equal("7610", job2 ())
