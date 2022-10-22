module Tests05

open Xunit
open Day05

[<Fact>]
let ``Day05 Stars`` () =
    Assert.Equal("9025675", job1 ())
    Assert.Equal("11981754", job2 ())
