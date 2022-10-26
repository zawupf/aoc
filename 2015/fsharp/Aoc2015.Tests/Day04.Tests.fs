module Day04.Tests

open Xunit
open Utils
open Day04

[<Theory>]
[<InlineData(609043, "abcdef")>]
[<InlineData(1048970, "pqrstuv")>]
let ``Day04 Part 1`` expected secret =
    Assert.Equal(expected, secret |> findLowestNumberWithPrefix "00000")

[<Fact>]
let ``Day04 Stars`` () =
    try
        Assert.Equal("254575", job1 ())
        Assert.Equal("1038736", job2 ())
    with :? System.NotImplementedException ->
        ()
