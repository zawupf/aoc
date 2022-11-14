module Day06.Tests

open Xunit
open Utils
open Day06

[<Fact>]
let ``Day06 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
