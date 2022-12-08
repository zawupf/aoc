module Day08.Tests

open Xunit
open Utils
open Day08

let input =
    """
    30373
    25512
    65332
    33549
    35390
    """
    |> String.trim
    |> String.split '\n'
    |> Array.map String.trim
    |> Grid.parse

[<Fact>]
let ``Day08 countVisibleTrees works`` () =
    Assert.Equal(21, input |> countVisibleTrees)

[<Fact>]
let ``Day08 maxScenicScore works`` () =
    Assert.Equal(8, input |> maxScenicScore)

[<Fact>]
let ``Day08 Stars`` () =
    try
        Assert.Equal("1789", job1 ())
        Assert.Equal("314820", job2 ())
    with :? System.NotImplementedException ->
        ()
