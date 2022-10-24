module Day07.Tests

open Xunit
open Utils
open Day07

[<Fact>]
let ``Day07 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
