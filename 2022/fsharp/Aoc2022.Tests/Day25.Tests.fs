module Day25.Tests

open Xunit
open Utils
open Day25

[<Fact>]
let ``Day25 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
