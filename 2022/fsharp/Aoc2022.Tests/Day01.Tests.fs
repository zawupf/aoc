module Day01.Tests

open Xunit
open Utils
open Day01

let input =
    """
        1000
        2000
        3000

        4000

        5000
        6000

        7000
        8000
        9000

        10000
    """
    |> String.trim
    |> String.split '\n'
    |> Array.map String.trim
    |> Array.toList

[<Fact>]
let ``Day01 findMostCalories works`` () =
    Assert.Equal(24000, input |> parse |> findMostCalories)

[<Fact>]
let ``Day01 findTop3Calories works`` () =
    Assert.Equal(45000, input |> parse |> findTop3Calories)

[<Fact>]
let ``Day01 Stars`` () =
    try
        Assert.Equal("67658", job1 ())
        Assert.Equal("200158", job2 ())
    with :? System.NotImplementedException ->
        ()
