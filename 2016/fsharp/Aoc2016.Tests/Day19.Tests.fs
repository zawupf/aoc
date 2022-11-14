module Day19.Tests

open Xunit
open Utils
open Day19

[<Fact>]
let ``Day19 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
