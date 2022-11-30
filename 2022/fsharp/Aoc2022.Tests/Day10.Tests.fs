module Day10.Tests

open Xunit
open Utils
open Day10

[<Fact>]
let ``Day10 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
