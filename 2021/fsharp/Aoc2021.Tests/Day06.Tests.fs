module Day06.Tests

open Xunit

let input = [ 3; 4; 3; 1; 2 ]

[<Fact>]
let ``Day06 simulate works`` () =
    Assert.Equal(26L, input |> simulateFishPopulation |> Seq.item 18)
    Assert.Equal(5934L, input |> simulateFishPopulation |> Seq.item 80)
    Assert.Equal(26984457539L, input |> simulateFishPopulation |> Seq.item 256)

[<Fact>]
let ``Day06 Stars`` () =
    Assert.Equal("350605", job1 ())
    Assert.Equal("1592778185024", job2 ())
