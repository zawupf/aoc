module Day02.Tests

open Xunit
open Utils
open Day02

[<Theory>]
[<InlineData(58, "2x3x4")>]
[<InlineData(43, "1x1x10")>]
let ``Day02 required paper works`` expected input =
    Assert.Equal(expected, input |> Box.parse |> requiredPaper)

[<Theory>]
[<InlineData(34, "2x3x4")>]
[<InlineData(14, "1x1x10")>]
let ``Day02 required ribbon works`` expected input =
    Assert.Equal(expected, input |> Box.parse |> requiredRibbon)

[<Fact>]
let ``Day02 Stars`` () =
    try
        Assert.Equal("1586300", job1 ())
        Assert.Equal("3737498", job2 ())
    with :? System.NotImplementedException ->
        ()
