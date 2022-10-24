module Day01.Tests

open Xunit
open Utils
open Day01

[<Theory>]
[<InlineData(0, "(())")>]
[<InlineData(0, "()()")>]
[<InlineData(3, "(((")>]
[<InlineData(3, "(()(()(")>]
[<InlineData(3, "))(((((")>]
[<InlineData(-1, "())")>]
[<InlineData(-1, "))(")>]
[<InlineData(-3, ")))")>]
[<InlineData(-3, ")())())")>]
let ``Day01 Part 1`` (expected, input) = Assert.Equal(expected, floor input)


[<Theory>]
[<InlineData(1, ")")>]
[<InlineData(5, "()())")>]
let ``Day01 Part 2`` (expected, input) =
    Assert.Equal(expected, enterBasementPosition input)

[<Fact>]
let ``Day01 Stars`` () =
    try
        Assert.Equal("74", job1 ())
        Assert.Equal("1795", job2 ())
    with :? System.NotImplementedException ->
        ()
