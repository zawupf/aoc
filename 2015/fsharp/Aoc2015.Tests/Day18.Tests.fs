module Day18.Tests

open Xunit
open Utils
open Day18

[<Fact>]
let ``Day18 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
