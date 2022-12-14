module Day14.Tests

open Xunit
open Utils
open Day14

let input =
    """
    498,4 -> 498,6 -> 496,6
    503,4 -> 502,4 -> 502,9 -> 494,9
    """
    |> String.toLines

[<Fact>]
let ``Day14 simulate works`` () =
    Assert.Equal(24, input |> parse Part1 |> simulate)
    Assert.Equal(93, input |> parse Part2 |> simulate)

[<Fact>]
let ``Day14 Stars`` () =
    try
        Assert.Equal("757", job1 ())
        Assert.Equal("24943", job2 ())
    with :? System.NotImplementedException ->
        ()
