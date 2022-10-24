module Day20.Tests

open Xunit
open Utils
open Day20

[<Fact>]
let ``Day20 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
