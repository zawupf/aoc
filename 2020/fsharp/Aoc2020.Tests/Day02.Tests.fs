module Day02.Tests

open Xunit
open Day02

[<Fact>]
let ``Day02 Stars`` () =
    Assert.Equal("645", job1 ())
    Assert.Equal("737", job2 ())
    ()
