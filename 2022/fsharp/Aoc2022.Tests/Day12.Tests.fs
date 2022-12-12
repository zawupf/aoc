module Day12.Tests

open Xunit
open Utils
open Day12

let input =
    """
    Sabqponm
    abcryxxl
    accszExk
    acctuvwj
    abdefghi
    """
    |> String.toLines

[<Fact>]
let ``Day12 findShortestPath works`` () =
    Assert.Equal(31, input |> Area.parse |> Area.findShortestPath)

[<Fact>]
let ``Day12 findShortestGlobalPath works`` () =
    Assert.Equal(29, input |> Area.parse |> Area.findShortestGlobalPath)

[<Fact>]
let ``Day12 Stars`` () =
    try
        Assert.Equal("497", job1 ())
        Assert.Equal("492", job2 ())
    with :? System.NotImplementedException ->
        ()
