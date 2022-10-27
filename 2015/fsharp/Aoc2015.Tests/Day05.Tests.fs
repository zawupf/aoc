module Day05.Tests

open Xunit
open Utils
open Day05

[<Theory>]
[<InlineData(true, "ugknbfddgicrmopn")>]
[<InlineData(true, "aaa")>]
[<InlineData(false, "jchzalrnumimnmhp")>]
[<InlineData(false, "haegwjzuvuyypxyu")>]
[<InlineData(false, "dvszwmarrgswjxmb")>]
let ``Day05 is nice works`` expected input =
    Assert.Equal(expected, input |> isNice)

[<Theory>]
[<InlineData(true, "qjhvhtzxzqqjkmpb")>]
[<InlineData(true, "xxyxx")>]
[<InlineData(false, "uurcxstgmygtbstg")>]
[<InlineData(false, "ieodomkazucvgmuy")>]
let ``Day05 is really nice works`` expected input =
    Assert.Equal(expected, input |> isReallyNice)

[<Fact>]
let ``Day05 Stars`` () =
    try
        Assert.Equal("238", job1 ())
        Assert.Equal("69", job2 ())
    with :? System.NotImplementedException ->
        ()
