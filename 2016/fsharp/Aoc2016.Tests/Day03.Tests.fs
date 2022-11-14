module Day03.Tests

open Xunit
open Utils
open Day03

[<Fact>]
let ``Day03 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
