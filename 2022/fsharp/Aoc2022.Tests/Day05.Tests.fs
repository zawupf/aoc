module Day05.Tests

open Xunit
open Utils
open Day05

let moves = [
    "move 1 from 2 to 1"
    "move 3 from 1 to 3"
    "move 2 from 2 to 1"
    "move 1 from 1 to 2"
]

let stacks = [|
    "NZ" |> String.toCharArray |> Array.toList
    "DCM" |> String.toCharArray |> Array.toList
    "P" |> String.toCharArray |> Array.toList
|]

[<Fact>]
let ``Day05 topCrates 1 works`` () =
    Assert.Equal("CMZ", stacks |> move craneOneByOne moves |> topCrates)

[<Fact>]
let ``Day05 topCrates 2 works`` () =
    Assert.Equal("MCD", stacks |> move craneAllAtOnce moves |> topCrates)

[<Fact>]
let ``Day05 Stars`` () =
    try
        Assert.Equal("GFTNRBZPF", job1 ())
        Assert.Equal("VRQWPDSGP", job2 ())
    with :? System.NotImplementedException ->
        ()
