module Day16.Tests

open Xunit
open Utils
open Day16

[<Fact>]
let ``Day16 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
