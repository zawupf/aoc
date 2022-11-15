module Day05.Tests

open Xunit
open Utils
open Day05

[<Fact>]
let ``Day05 computePassword works`` () =
    Assert.Equal("18f47a30", "abc" |> computePassword)

[<Fact>]
let ``Day05 computePassword2 works`` () =
    Assert.Equal("05ace8e3", "abc" |> computePassword2)

[<Fact>]
let ``Day05 Stars`` () =
    try
        Assert.Equal("d4cd2ee1", job1 ())
        Assert.Equal("f2c730e5", job2 ())
    with :? System.NotImplementedException ->
        ()
