module Day18.Tests

open Xunit
open Utils
open Day18

let input =
    ".#.#.#
...##.
#....#
..#...
#.#..#
####.."

[<Fact>]
let ``Day18 parse works`` () =
    Assert.Equal(input, input |> Grid.parse id |> Grid.toString)

[<Theory>]
[<InlineData(0, -1)>]
[<InlineData(0, 0)>]
[<InlineData(1, 5)>]
[<InlineData(1, 30)>]
[<InlineData(0, 35)>]
[<InlineData(0, 36)>]
let ``Day18 item works`` count index =
    let grid = input |> Grid.parse id
    Assert.Equal(count, grid |> Grid.item index)

[<Theory>]
[<InlineData(0, -1)>]
[<InlineData(1, 0)>]
[<InlineData(1, 5)>]
[<InlineData(2, 30)>]
[<InlineData(1, 35)>]
// [<InlineData(0, 36)>]
let ``Day18 countAdjacentLights works`` count index =
    let grid = input |> Grid.parse id
    Assert.Equal(count, grid |> Grid.countAdjacentLights index)

[<Fact>]
let ``Day18 next grid works`` () =
    Assert.Equal(
        4,
        input |> Grid.parse id |> grids id |> Seq.item 4 |> Grid.countLights
    )

[<Fact>]
let ``Day18 next grid with overrides works`` () =
    Assert.Equal(
        17,
        input
        |> Grid.parse cornerLightsStuckOn
        |> grids cornerLightsStuckOn
        |> Seq.item 5
        |> Grid.countLights
    )

[<Fact>]
let ``Day18 Stars`` () =
    try
        Assert.Equal("821", job1 ())
        Assert.Equal("886", job2 ())
    with :? System.NotImplementedException ->
        ()
