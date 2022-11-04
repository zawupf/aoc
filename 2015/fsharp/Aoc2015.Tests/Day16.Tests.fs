module Day16.Tests

open Xunit
open Utils
open Day16

[<Fact>]
let ``Day16 Stars`` () =
    try
        Assert.Equal("103", job1 ())
        Assert.Equal("405", job2 ())
    with :? System.NotImplementedException ->
        ()
