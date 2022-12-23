module Day23.Tests

open Xunit
open Utils
open Day23

let input =
    """
    ....#..
    ..###.#
    #...#.#
    .#...##
    #.###..
    ##.#.##
    .#..#..
    """
    |> String.toLines

[<Fact>]
let ``Day23 countEmptyTiles works`` () =
    Assert.Equal(
        110,
        input |> parse |> rounds |> Seq.item 10 |> countEmptyTiles
    )

    Assert.Equal(20, input |> parse |> rounds |> Seq.length)

[<Fact>]
let ``Day23 Stars`` () =
    try
        Assert.Equal("3812", job1 ())
        Assert.Equal("1003", job2 ())
    with :? System.NotImplementedException ->
        ()
