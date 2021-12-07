module Day07.Tests

open Xunit

let input = [ 16; 1; 2; 0; 4; 2; 7; 1; 2; 14 ]

[<Fact>]
let ``Day07 simulate works`` () =
    Assert.Equal((2, 37), input |> findBestPosition)
    Assert.Equal((5, 168), input |> findBestCrabPosition)

[<Fact>]
let ``Day07 Stars`` () =
    Assert.Equal("348996", job1 ())
    Assert.Equal("98231647", job2 ())
