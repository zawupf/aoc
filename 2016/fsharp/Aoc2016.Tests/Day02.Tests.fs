module Day02.Tests

open Xunit
open Utils
open Day02

[<Fact>]
let ``Day02 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
