module Day14.Tests

open Xunit
open Utils
open Day14

[<Fact>]
let ``Day14 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
