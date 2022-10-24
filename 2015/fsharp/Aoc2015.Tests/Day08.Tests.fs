module Day08.Tests

open Xunit
open Utils
open Day08

[<Fact>]
let ``Day08 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
