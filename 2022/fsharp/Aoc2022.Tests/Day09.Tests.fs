module Day09.Tests

open Xunit
open Utils
open Day09

[<Fact>]
let ``Day09 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
