module Day04.Tests

open Xunit
open Utils
open Day04

[<Fact>]
let ``Day04 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
