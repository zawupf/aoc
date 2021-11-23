module Day03.Tests

open Xunit
open Day03

[<Fact>]
let ``Day02 Stars`` () =
    Assert.Equal("167", job1 ())
    // Assert.Equal("736527114", job2());
    ()
