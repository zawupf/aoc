module Day07.Tests

open Xunit
open Utils
open Day07

[<Fact>]
let ``Day07 part 1 works`` () =
    let circuit =
        Circuit.empty
        |> Circuit.add "123 -> x"
        |> Circuit.add "456 -> y"
        |> Circuit.add "x AND y -> d"
        |> Circuit.add "x OR y -> e"
        |> Circuit.add "x LSHIFT 2 -> f"
        |> Circuit.add "y RSHIFT 2 -> g"
        |> Circuit.add "NOT x -> h"
        |> Circuit.add "NOT y -> i"
        |> Circuit.run

    Assert.Equal(72us, circuit |> Circuit.get "d")
    Assert.Equal(507us, circuit |> Circuit.get "e")
    Assert.Equal(492us, circuit |> Circuit.get "f")
    Assert.Equal(114us, circuit |> Circuit.get "g")
    Assert.Equal(65412us, circuit |> Circuit.get "h")
    Assert.Equal(65079us, circuit |> Circuit.get "i")
    Assert.Equal(123us, circuit |> Circuit.get "x")
    Assert.Equal(456us, circuit |> Circuit.get "y")

[<Fact>]
let ``Day07 Stars`` () =
    try
        Assert.Equal("3176", job1 ())
        Assert.Equal("14710", job2 ())
    with :? System.NotImplementedException ->
        ()
