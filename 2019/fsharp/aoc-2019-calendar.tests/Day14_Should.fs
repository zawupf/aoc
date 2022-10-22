module Tests14

open Xunit
open Day14

[<Fact>]
let ``Day14 Stars`` () =
    Assert.Equal("97422", job1 ())
    Assert.Equal("13108426", job2 ())
    ()
