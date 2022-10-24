module Day11.Tests

open Xunit
open Utils
open Day11

[<Fact>]
let ``Day11 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
