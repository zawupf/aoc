module Tests10

open Xunit
open Day10
open Utils

module Data =
    let sample1 = [ ".#..#"; "....."; "#####"; "....#"; "...##" ]
    let sample2 = [ ".#..+"; "....."; "#####"; "....#"; "...##" ]

[<Fact>]
let ``Day10 Parse asteroids works`` () =
    let asteroids = Data.sample1 |> parse
    Assert.True(asteroids |> List.contains (1, 0))
    Assert.True(asteroids |> List.contains (4, 0))
    Assert.True(asteroids |> List.contains (0, 2))
    Assert.True(asteroids |> List.contains (1, 2))
    Assert.True(asteroids |> List.contains (2, 2))
    Assert.True(asteroids |> List.contains (3, 2))
    Assert.True(asteroids |> List.contains (4, 2))
    Assert.True(asteroids |> List.contains (4, 3))
    Assert.True(asteroids |> List.contains (4, 4))
    Assert.True(asteroids |> List.contains (3, 4))
    Assert.Equal(10, asteroids |> List.length)

    Assert.Throws<System.Exception>(fun () -> Data.sample2 |> parse |> ignore)

[<Theory>]
[<InlineData("10-sample1", 3, 4, 8)>]
[<InlineData("10-sample2", 5, 8, 33)>]
[<InlineData("10-sample3", 1, 2, 35)>]
[<InlineData("10-sample4", 6, 3, 41)>]
[<InlineData("10-sample5", 11, 13, 210)>]
let ``Day10 max visible works`` sample x y n =
    Assert.Equal((n, (x, y)), sample |> readInputLines |> parse |> maxVisible)

[<Theory>]
[<InlineData("10-sample5", 1, 11, 12)>]
[<InlineData("10-sample5", 2, 12, 1)>]
[<InlineData("10-sample5", 3, 12, 2)>]
[<InlineData("10-sample5", 10, 12, 8)>]
[<InlineData("10-sample5", 20, 16, 0)>]
[<InlineData("10-sample5", 50, 16, 9)>]
[<InlineData("10-sample5", 100, 10, 16)>]
[<InlineData("10-sample5", 199, 9, 6)>]
[<InlineData("10-sample5", 200, 8, 2)>]
[<InlineData("10-sample5", 201, 10, 9)>]
[<InlineData("10-sample5", 299, 11, 1)>]
let ``Day10 vaporize works`` sample nth x y =
    Assert.Equal(
        (x, y),
        sample
        |> readInputLines
        |> parse
        |> vaporizeOrder
        |> List.item (nth - 1)
    )

[<Fact>]
let ``Day10 Stars`` () =
    Assert.Equal("276", job1 ())
    Assert.Equal("1321", job2 ())
    ()
