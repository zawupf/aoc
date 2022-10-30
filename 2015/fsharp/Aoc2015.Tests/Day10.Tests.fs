module Day10.Tests

open Xunit
open Utils
open Day10

[<Theory>]
[<InlineData("1", "11")>]
[<InlineData("11", "21")>]
[<InlineData("21", "1211")>]
[<InlineData("1211", "111221")>]
[<InlineData("111221", "312211")>]
let ``Day10 look and say`` look say = Assert.Equal(say, look |> lookAndSay)

[<Fact>]
let ``Day10 play works`` () =
    Assert.Equal("312211", "1" |> play |> Seq.item 5)

[<Fact>]
let ``Day10 Stars`` () =
    try
        Assert.Equal("360154", job1 ())
        Assert.Equal("5103798", job2 ())
    with :? System.NotImplementedException ->
        ()
