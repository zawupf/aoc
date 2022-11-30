module Day05.Tests

open Xunit
open Utils
open Day05

[<Fact>]
let ``Day05 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
