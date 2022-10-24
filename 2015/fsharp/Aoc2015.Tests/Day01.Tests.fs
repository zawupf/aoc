module Day01.Tests

open Xunit
open Utils
open Day01

[<Fact>]
let ``Day01 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
