module Day16.Tests

open Xunit
open Utils
open Day16

[<Theory>]
[<InlineData("0", "")>]
[<InlineData("100", "1")>]
[<InlineData("001", "0")>]
[<InlineData("11111000000", "11111")>]
[<InlineData("1111000010100101011110000", "111100001010")>]
let ``Day16 randomBits works`` (expected: string) salt =
    Assert.Equal(
        expected,
        salt
        |> randomBits expected.Length (salt |> Seq.length)
        |> String.ofChars
    )

[<Theory>]
[<InlineData("100", "110010110100")>]
let ``Day16 checksum works`` expected bits =
    Assert.Equal(
        expected,
        bits |> checksum (bits |> Seq.length) |> String.ofChars
    )

[<Fact>]
let ``Day16 Stars`` () =
    try
        Assert.Equal("10011010010010010", job1 ())
        Assert.Equal("10101011110100011", job2 ())
    with :? System.NotImplementedException ->
        ()
