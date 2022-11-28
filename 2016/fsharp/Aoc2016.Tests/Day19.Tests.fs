module Day19.Tests

open Xunit
open Utils
open Day19

[<Fact>]
let ``Day19 play1 works`` () = Assert.Equal(3, play1 5)

[<Fact>]
let ``Day19 play2 works`` () = Assert.Equal(2, play2 5)

[<Fact>]
let ``Day19 Stars`` () =
    try
        Assert.Equal("1834471", job1 ())
        Assert.Equal("1420064", job2 ())
    with :? System.NotImplementedException ->
        ()
