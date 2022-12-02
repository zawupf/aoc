module Day02.Tests

open Xunit
open Utils
open Day02

[<Fact>]
let ``Day02 totalScore1 works`` () =
    Assert.Equal(15, [| "A Y"; "B X"; "C Z" |] |> totalScore1)

[<Fact>]
let ``Day02 totalScore2 works`` () =
    Assert.Equal(12, [| "A Y"; "B X"; "C Z" |] |> totalScore2)

[<Fact>]
let ``Day02 Stars`` () =
    try
        Assert.Equal("8933", job1 ())
        Assert.Equal("11998", job2 ())
    with :? System.NotImplementedException ->
        ()
