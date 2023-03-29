module Day08.Tests

open Xunit
open Utils
open Day08

[<Fact>]
let ``Day08 screen works`` () =
    let n = 4

    let screens =
        [
            "rect 3x2"
            "rotate column x=1 by 1"
            "rotate row y=0 by 4"
            "rotate column x=1 by 1"
        ]
        |> List.take n
        |> List.scan
            (fun screen instruction -> screen |> Screen.next instruction)
            (Screen.empty 7 3)
        |> List.tail

    let result = screens |> List.map Screen.toString
    let litCountsResult = screens |> List.map Screen.litCount

    let expected =
        [
            "###....\n###....\n......."
            "#.#....\n###....\n.#....."
            "....#.#\n###....\n.#....."
            ".#..#.#\n#.#....\n.#....."
        ]
        |> List.take n

    let litCountsExpected = [ 6; 6; 6; 6 ] |> List.take n

    Assert.Equal<string list>(expected, result)
    Assert.Equal<int list>(litCountsExpected, litCountsResult)

[<Fact>]
let ``Day08 Stars`` () =
    try
        Assert.Equal("115", job1 ())

        Assert.Equal(
            """
####.####.####.#...##..#.####.###..####..###...##.
#....#....#....#...##.#..#....#..#.#......#.....#.
###..###..###...#.#.##...###..#..#.###....#.....#.
#....#....#......#..#.#..#....###..#......#.....#.
#....#....#......#..#.#..#....#.#..#......#..#..#.
####.#....####...#..#..#.#....#..#.#.....###..##..""",
            job2 ()
        )
    with :? System.NotImplementedException ->
        ()
