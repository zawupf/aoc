module Day13.Tests

open Xunit
open Utils
open Day13

[<Fact>]
let ``Day13 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
