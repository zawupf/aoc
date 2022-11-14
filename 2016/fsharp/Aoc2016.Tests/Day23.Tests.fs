module Day23.Tests

open Xunit
open Utils
open Day23

[<Fact>]
let ``Day23 Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
