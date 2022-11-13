module Day23.Tests

open Xunit
open Utils
open Day23

let input =
    """
    inc a
    jio a, +2
    tpl a
    inc a
    """
    |> String.trim
    |> String.split '\n'
    |> Array.map String.trim

[<Fact>]
let ``Day23 sample program works`` () =
    let computer = Computer.empty |> Computer.load input |> Computer.run
    Assert.Equal(2u, computer |> Computer.get A)

[<Fact>]
let ``Day23 Stars`` () =
    try
        Assert.Equal("184", job1 ())
        Assert.Equal("231", job2 ())
    with :? System.NotImplementedException ->
        ()
