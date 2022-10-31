module Day11.Tests

open Xunit
open Utils
open Day11

[<Theory>]
[<InlineData("xx", "xy")>]
[<InlineData("xy", "xz")>]
[<InlineData("xz", "ya")>]
[<InlineData("ya", "yb")>]
let ``Day11 increment works`` current next =
    Assert.Equal(next, current |> encode |> increment |> decode)

[<Theory>]
[<InlineData("abcdffaa")>]
[<InlineData("ghjaabcc")>]
let ``Day11 has straight works`` valid = Assert.True(valid |> hasStraight)

[<Theory>]
[<InlineData("abcdefgh", "abcdffaa")>]
[<InlineData("ghijklmn", "ghjaabcc")>]
let ``Day11 next valid password works`` current next =
    Assert.Equal(next, current |> nextValidPassword)

[<Fact>]
let ``Day11 Stars`` () =
    try
        Assert.Equal("vzbxxyzz", job1 ())
        Assert.Equal("vzcaabcc", job2 ())
    with :? System.NotImplementedException ->
        ()
