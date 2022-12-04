module Day04.Tests

open Xunit
open Utils
open Day04

let input =
    """
    2-4,6-8
    2-3,4-5
    5-7,7-9
    2-8,3-7
    6-6,4-6
    2-6,4-8
    """
    |> String.trim
    |> String.split '\n'
    |> Array.map (String.trim >> Range.parsePair)
    |> Array.toList

[<Fact>]
let ``Day04 countFullyContainedPairs works`` () =
    Assert.Equal(2, input |> countFullyContainedPairs)

[<Fact>]
let ``Day04 countOverlappingPairs works`` () =
    Assert.Equal(4, input |> countOverlappingPairs)

[<Fact>]
let ``Day04 Stars`` () =
    try
        Assert.Equal("305", job1 ())
        Assert.Equal("811", job2 ())
    with :? System.NotImplementedException ->
        ()
