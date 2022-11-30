module Day01.Tests

open Xunit
open Utils
open Day01

let seaDepths =
    "199 200 208 210 200 207 240 269 260 263" |> String.parseInts ' '

[<Fact>]
let ``Day01 count increased sea depths works`` () =
    Assert.Equal(7, countIncreasedSeaDepths seaDepths)

[<Fact>]
let ``Day01 count increased sea depth windows works`` () =
    Assert.Equal(5, countIncreasedSeaDepthWindows seaDepths)

[<Fact>]
let ``Day01 Stars`` () =
    Assert.Equal("1583", job1 ())
    Assert.Equal("1627", job2 ())
