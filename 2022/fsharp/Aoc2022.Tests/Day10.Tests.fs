module Day10.Tests

open Xunit
open Utils
open Day10

let input =
    """
    addx 15
    addx -11
    addx 6
    addx -3
    addx 5
    addx -1
    addx -8
    addx 13
    addx 4
    noop
    addx -1
    addx 5
    addx -1
    addx 5
    addx -1
    addx 5
    addx -1
    addx 5
    addx -1
    addx -35
    addx 1
    addx 24
    addx -19
    addx 1
    addx 16
    addx -11
    noop
    noop
    addx 21
    addx -15
    noop
    noop
    addx -3
    addx 9
    addx 1
    addx -3
    addx 8
    addx 1
    addx 5
    noop
    noop
    noop
    noop
    noop
    addx -36
    noop
    addx 1
    addx 7
    noop
    noop
    noop
    addx 2
    addx 6
    noop
    noop
    noop
    noop
    noop
    addx 1
    noop
    noop
    addx 7
    addx 1
    noop
    addx -13
    addx 13
    addx 7
    noop
    addx 1
    addx -33
    noop
    noop
    noop
    addx 2
    noop
    noop
    noop
    addx 8
    noop
    addx -1
    addx 2
    addx 1
    noop
    addx 17
    addx -9
    addx 1
    addx 1
    addx -3
    addx 11
    noop
    noop
    addx 1
    noop
    addx 1
    noop
    noop
    addx -13
    addx -19
    addx 1
    addx 3
    addx 26
    addx -30
    addx 12
    addx -1
    addx 3
    addx 1
    noop
    noop
    noop
    addx -9
    addx 18
    addx 1
    addx 2
    noop
    noop
    addx 9
    noop
    noop
    noop
    addx -1
    addx 2
    addx -37
    addx 1
    addx 3
    noop
    addx 15
    addx -21
    addx 22
    addx -6
    addx 1
    noop
    addx 2
    addx 1
    noop
    addx -10
    noop
    noop
    addx 20
    addx 1
    addx 2
    addx 2
    addx -6
    addx -11
    noop
    noop
    noop
    """
    |> String.toLines

let output =
    """
    ##..##..##..##..##..##..##..##..##..##..
    ###...###...###...###...###...###...###.
    ####....####....####....####....####....
    #####.....#####.....#####.....#####.....
    ######......######......######......####
    #######.......#######.......#######.....
    """
    |> String.toLines

[<Fact>]
let ``Day10 cycles works`` () =
    Assert.Equal<int list>(
        [ 1; 1; 1; 4; 4 ],
        "noop\naddx 3\naddx -5" |> String.toLines |> cycles |> Seq.toList
    )

[<Fact>]
let ``Day10 signalStrengthSum works`` () =
    Assert.Equal(13140, input |> cycles |> signalStrengthSum)

[<Fact>]
let ``Day10 render works`` () =
    Assert.Equal<string[]>(output, input |> cycles |> render)

[<Fact>]
let ``Day10 Stars`` () =
    let result2 =
        """
        ###...##..#..#.####..##..#....#..#..##..
        #..#.#..#.#..#.#....#..#.#....#..#.#..#.
        #..#.#....####.###..#....#....#..#.#....
        ###..#.##.#..#.#....#.##.#....#..#.#.##.
        #....#..#.#..#.#....#..#.#....#..#.#..#.
        #.....###.#..#.#.....###.####..##...###.
        """
        |> String.toLines
        |> Array.append [| "" |]
        |> String.join "\n"

    try
        Assert.Equal("15260", job1 ())
        Assert.Equal(result2, job2 ())
    with :? System.NotImplementedException ->
        ()
