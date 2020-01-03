module Tests13

open Xunit
open Day13

[<Fact>]
let ``Day13 Stars``() =
    Assert.Equal("173", job1())
    Assert.Equal("8942", job2())
    ()
