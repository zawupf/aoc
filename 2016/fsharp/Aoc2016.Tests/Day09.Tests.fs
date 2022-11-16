module Day09.Tests

open Xunit
open Utils
open Day09

[<Theory>]
[<InlineData("ADVENT", "ADVENT")>]
[<InlineData("ABBBBBC", "A(1x5)BC")>]
[<InlineData("XYZXYZXYZ", "(3x3)XYZ")>]
[<InlineData("ABCBCDEFEFG", "A(2x2)BCD(2x2)EFG")>]
[<InlineData("(1x3)A", "(6x1)(1x3)A")>]
[<InlineData("X(3x3)ABC(3x3)ABCY", "X(8x2)(3x3)ABCY")>]
let ``Day09 decompressed length 1 works`` expected data =
    let result = data |> decompressedLength1
    let expected = expected |> String.length |> int64
    Assert.Equal(expected, result)

[<Theory>]
[<InlineData(9, "(3x3)XYZ")>]
[<InlineData(20, "X(8x2)(3x3)ABCY")>]
[<InlineData(241920, "(27x12)(20x12)(13x14)(7x10)(1x12)A")>]
[<InlineData(445, "(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN")>]
let ``Day09 decompressed length 2 works`` expected data =
    Assert.Equal(expected, data |> decompressedLength2)

[<Fact>]
let ``Day09 Stars`` () =
    try
        Assert.Equal("152851", job1 ())
        Assert.Equal("11797310782", job2 ())
    with :? System.NotImplementedException ->
        ()
