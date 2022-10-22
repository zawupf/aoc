module Tests12

open Xunit
open Day12

[<Fact>]
let ``Day12 Stars`` () =
    Assert.Equal("7179", job1 ())
    Assert.Equal("428576638953552", job2 ())
    ()
