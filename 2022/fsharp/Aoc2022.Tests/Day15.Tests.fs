module Day15.Tests

open Xunit
open Utils
open Day15

[<Fact>]
let ``Day15 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
