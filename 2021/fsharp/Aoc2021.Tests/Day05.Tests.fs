module Day05.Tests

open Xunit
open Utils

let rawInput =
    "
        0,9 -> 5,9
        8,0 -> 0,8
        9,4 -> 3,4
        2,2 -> 2,1
        7,0 -> 7,4
        6,4 -> 2,0
        0,9 -> 2,9
        3,4 -> 1,4
        0,0 -> 8,8
        5,5 -> 8,2
    "

let input =
    rawInput
    |> String.trim
    |> String.split '\n'
    |> Array.map String.trim
    |> Array.toList

[<Fact>]
let ``Day05 straight lines only works`` () =
    Assert.Equal(5, input |> countMostDangerousPoints WithoutDiagonals)

[<Fact>]
let ``Day05 lines with diagonals works`` () =
    Assert.Equal(12, input |> countMostDangerousPoints WithDiagonals)

[<Fact>]
let ``Day05 Stars`` () =
    Assert.Equal("8622", job1 ())
    Assert.Equal("22037", job2 ())
