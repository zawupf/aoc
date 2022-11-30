module Day24.Tests

open Xunit
open Utils
open Day24

[<Fact>]
let ``Day24 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
