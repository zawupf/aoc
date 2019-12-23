module Tests09

open Xunit
open Day09

[<Fact>]
let ``Day09 Stars``() =
    Assert.Equal("2941952859", job1())
    Assert.Equal("66113", job2())
    ()
