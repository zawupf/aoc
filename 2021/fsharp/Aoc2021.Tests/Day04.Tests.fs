module Day04.Tests

open Xunit
open Utils

let rawInput =
    "
        7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

        22 13 17 11  0
         8  2 23  4 24
        21  9 14 16  7
         6 10  3 18  5
         1 12 20 15 19

         3 15  0  2 22
         9 18 13 17  5
        19  8  7 25 23
        20 11 10 24  4
        14 21 16 12  6

        14 21 17 24  4
        10 16 15  9 19
        18  8 23 26 20
        22 11 13  6  5
         2  0 12  3  7
    "

let input =
    rawInput
    |> String.trim
    |> String.split '\n'
    |> Array.map String.trim
    |> Array.toList

[<Fact>]
let ``Day04 input data is correct`` () =
    Assert.Equal(19, input |> List.length)
    Assert.Equal("7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1", input |> List.head)
    Assert.Equal("2  0 12  3  7", input |> List.last)

[<Fact>]
let ``Day04 win game works`` () =
    Assert.Equal(4512, input |> Game.ofLines |> Game.play |> Seq.head |> Board.score)

[<Fact>]
let ``Day04 loose game works`` () =
    Assert.Equal(1924, input |> Game.ofLines |> Game.play |> Seq.last |> Board.score)

[<Fact>]
let ``Day04 Stars`` () =
    Assert.Equal("4662", job1 ())
    Assert.Equal("12080", job2 ())
