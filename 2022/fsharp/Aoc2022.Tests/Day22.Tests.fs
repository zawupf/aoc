module Day22.Tests

open Xunit
open Utils
open Day22

let input =
    """
        ...#
        .#..
        #...
        ....
...#.......#
........#...
..#....#....
..........#.
        ...#....
        .....#..
        .#......
        ......#.

10R5L5R10L4R5L5
"""

[<Fact>]
let ``Day22 parse works`` () =
    let scene = input |> Scene.parse
    Assert.Equal(18, scene.map |> Array2D.length1)
    Assert.Equal(14, scene.map |> Array2D.length2)
    Assert.Equal("10R5L5R10L4R5L5", scene.path)

    Assert.Equal(
        {
            pos = { x = 9; y = 1 }
            facing = Right
        },
        scene.cursor
    )

[<Fact>]
let ``Day22 password works`` () =
    Assert.Equal(6032, input |> Scene.parse |> Scene.walk |> Scene.password)

[<Fact>]
let ``Day22 Stars`` () =
    try
        Assert.Equal("164014", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
