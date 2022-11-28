module Day18.Tests

open Xunit
open Utils
open Day18

[<Theory>]
[<InlineData(6, "..^^.", 3)>]
[<InlineData(38, ".^^.^.^^^^", 10)>]
let ``Day18 countSafeTiles works`` expected row rowCount =
    Assert.Equal(expected, row |> countSafeTiles rowCount)

[<Fact>]
let ``Day18 Stars`` () =
    try
        Assert.Equal("2013", job1 ())
        Assert.Equal("20006289", job2 ())
    with :? System.NotImplementedException ->
        ()
