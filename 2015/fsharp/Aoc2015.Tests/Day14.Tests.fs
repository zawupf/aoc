module Day14.Tests

open Xunit
open Utils
open Day14

[<Literal>]
let comet =
    "Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds."

[<Literal>]
let dancer =
    "Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds."

[<Theory>]
[<InlineData(14, comet, 1)>]
[<InlineData(140, comet, 10)>]
[<InlineData(140, comet, 11)>]
[<InlineData(140, comet, 12)>]
[<InlineData(16, dancer, 1)>]
[<InlineData(160, dancer, 10)>]
[<InlineData(176, dancer, 11)>]
[<InlineData(176, dancer, 12)>]
[<InlineData(1120, comet, 1000)>]
[<InlineData(1056, dancer, 1000)>]
let ``Day14 distanceAfter works`` distance reindeer seconds =
    Assert.Equal(
        distance,
        reindeer |> Reindeer.parse |> Reindeer.distanceAfter seconds
    )

[<Fact>]
let ``Day14 maxScoreAfter works`` () =
    Assert.Equal(
        689,
        [ comet; dancer ]
        |> List.map Reindeer.parse
        |> Reindeer.maxScoreAfter 1000
    )

[<Fact>]
let ``Day14 Stars`` () =
    try
        Assert.Equal("2696", job1 ())
        Assert.Equal("1084", job2 ())
    with :? System.NotImplementedException ->
        ()
