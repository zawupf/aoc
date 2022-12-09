module Day09.Tests

open Xunit
open Utils
open Day09

let input =
    """
    R 4
    U 4
    L 3
    D 1
    R 4
    D 1
    L 5
    R 2
    """
    |> String.toLines

let input2 =
    """
    R 5
    U 8
    L 8
    D 3
    R 17
    D 10
    L 25
    U 20
    """
    |> String.toLines

[<Fact>]
let ``Day09 countTailPositions works`` () =
    Assert.Equal(13, input |> countTailPositions 2)
    Assert.Equal(1, input |> countTailPositions 10)
    Assert.Equal(36, input2 |> countTailPositions 10)

[<Fact>]
let ``Day09 Stars`` () =
    try
        Assert.Equal("6098", job1 ())
        Assert.Equal("2597", job2 ())
    with :? System.NotImplementedException ->
        ()
