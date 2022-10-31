module Day12.Tests

open Xunit
open Utils
open Day12

[<Theory>]
[<InlineData(6, "[1,2,3]")>]
[<InlineData(6, "{\"a\":2,\"b\":4}")>]
[<InlineData(3, "[[[3]]]")>]
[<InlineData(3, "{\"a\":{\"b\":4},\"c\":-1}")>]
[<InlineData(0, "{\"a\":[-1,1]}")>]
[<InlineData(0, "[-1,{\"a\":1}]")>]
[<InlineData(0, "[]")>]
[<InlineData(0, "{}")>]
let ``Day12 sum of numbers works`` sum input =
    Assert.Equal(sum, input |> findNumbers |> Seq.sum)

[<Theory>]
[<InlineData(6, "[1,2,3]")>]
[<InlineData(4, "[1,{\"c\":\"red\",\"b\":2},3]")>]
[<InlineData(0, "{\"d\":\"red\",\"e\":[1,2,3,4],\"f\":5}")>]
[<InlineData(6, "[1,\"red\",5]")>]
let ``Day12 sum of numbers w/o red works`` sum input =
    Assert.Equal(sum, input |> removeRed |> findNumbers |> Seq.sum)

[<Fact>]
let ``Day12 Stars`` () =
    try
        Assert.Equal("156366", job1 ())
        Assert.Equal("96852", job2 ())
    with :? System.NotImplementedException ->
        ()
