module Day08.Tests

open Xunit
open Utils
open Day08

[<Theory>]
[<InlineData(0, "\"\"")>]
[<InlineData(3, "\"abc\"")>]
[<InlineData(7, "\"aaa\\\"aaa\"")>]
[<InlineData(1, "\"\\x27\"")>]
let ``Day08 decode literal works`` expected literal =
    Assert.Equal(expected, literal |> decodeCharCount)

[<Theory>]
[<InlineData(6, "\"\"")>]
[<InlineData(9, "\"abc\"")>]
[<InlineData(16, "\"aaa\\\"aaa\"")>]
[<InlineData(11, "\"\\x27\"")>]
let ``Day08 encode string works`` expected literal =
    Assert.Equal(expected, literal |> encodeCharCount)

[<Fact>]
let ``Day08 Stars`` () =
    try
        Assert.Equal("1350", job1 ())
        Assert.Equal("2085", job2 ())
    with :? System.NotImplementedException ->
        ()
