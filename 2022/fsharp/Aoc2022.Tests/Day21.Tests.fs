module Day21.Tests

open Xunit
open Utils
open Day21

[<Fact>]
let ``Day21 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
