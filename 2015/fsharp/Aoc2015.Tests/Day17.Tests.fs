module Day17.Tests

open Xunit
open Utils
open Day17

let sizes = [ 20; 15; 10; 5; 5 ]

[<Fact>]
let ``Day17 count combinations works`` () =
    Assert.Equal(4, sizes |> countCombinations 25)

[<Fact>]
let ``Day17 Stars`` () =
    try
        Assert.Equal("654", job1 ())
        Assert.Equal("57", job2 ())
    with :? System.NotImplementedException ->
        ()
