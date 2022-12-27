module Day25.Tests

open Xunit
open Utils
open Day25

[<Theory>]
[<InlineData(1, "1")>]
[<InlineData(2, "2")>]
[<InlineData(3, "1=")>]
[<InlineData(4, "1-")>]
[<InlineData(5, "10")>]
[<InlineData(6, "11")>]
[<InlineData(7, "12")>]
[<InlineData(8, "2=")>]
[<InlineData(9, "2-")>]
[<InlineData(10, "20")>]
[<InlineData(15, "1=0")>]
[<InlineData(20, "1-0")>]
[<InlineData(2022, "1=11-2")>]
[<InlineData(12345, "1-0---0")>]
[<InlineData(314159265, "1121-1110-1=0")>]
let ``Day25 Snafu.toInt works`` expected snafu =
    Assert.Equal(expected, snafu |> Snafu.toInt)

[<Theory>]
[<InlineData(1, "1")>]
[<InlineData(2, "2")>]
[<InlineData(3, "1=")>]
[<InlineData(4, "1-")>]
[<InlineData(5, "10")>]
[<InlineData(6, "11")>]
[<InlineData(7, "12")>]
[<InlineData(8, "2=")>]
[<InlineData(9, "2-")>]
[<InlineData(10, "20")>]
[<InlineData(15, "1=0")>]
[<InlineData(20, "1-0")>]
[<InlineData(2022, "1=11-2")>]
[<InlineData(12345, "1-0---0")>]
[<InlineData(314159265, "1121-1110-1=0")>]
let ``Day25 Snafu.ofInt works`` number expected =
    Assert.Equal(expected, number |> Snafu.ofInt)

[<Fact>]
let ``Day25 Stars`` () =
    try
        Assert.Equal("2=1-=02-21===-21=200", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
