module Day22.Tests

open Xunit
open Utils
open Day22

[<Fact>]
let ``Day22 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
