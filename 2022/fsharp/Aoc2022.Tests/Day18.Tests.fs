module Day18.Tests

open Xunit
open Utils
open Day18

let input1 =
    """
    1,1,1
    2,1,1
    """
    |> String.toLines
    |> Array.toList

let input2 =
    """
    2,2,2
    1,2,2
    3,2,2
    2,1,2
    2,3,2
    2,2,1
    2,2,3
    2,2,4
    2,2,6
    1,2,5
    3,2,5
    2,1,5
    2,3,5
    """
    |> String.toLines
    |> Array.toList

[<Fact>]
let ``Day18 surface works`` () =
    Assert.Equal(10, input1 |> totalSurface)
    Assert.Equal(64, input2 |> totalSurface)

[<Fact>]
let ``Day18 outerSurface works`` () =
    Assert.Equal(58, input2 |> outerSurface)

[<Fact>]
let ``Day18 Stars`` () =
    try
        Assert.Equal("4314", job1 ())
        Assert.Equal("2444", job2 ())
    with :? System.NotImplementedException ->
        ()
