module Day17.Tests

open Xunit
open Utils
open Day17

[<Fact>]
let ``Day17 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
