module Day12.Tests

open Xunit
open Utils
open Day12

[<Fact>]
let ``Day12 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
