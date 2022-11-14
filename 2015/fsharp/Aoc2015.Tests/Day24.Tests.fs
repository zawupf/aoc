module Day24.Tests

open Xunit
open Utils
open Day24

let input = [ 1..5 ] @ [ 7..11 ]

[<Fact>]
let ``Day24 part 1 works`` () =
    Assert.Equal(99L, input |> findMinimalEntanglement 3)

[<Fact>]
let ``Day24 part 2 works`` () =
    Assert.Equal(44L, input |> findMinimalEntanglement 4)

[<Fact>]
let ``Day24 Stars`` () =
    try
        Assert.Equal("11846773891", job1 ())
        Assert.Equal("80393059", job2 ())
    with :? System.NotImplementedException ->
        ()
