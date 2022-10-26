module Day03.Tests

open Xunit
open Utils
open Day03

[<Theory>]
[<InlineData(2, ">")>]
[<InlineData(4, "^>v<")>]
[<InlineData(2, "^v^v^v^v^v")>]
let ``Day03 santa house count works`` expected input =
    Assert.Equal(expected, visitedHousesCount input)

[<Theory>]
[<InlineData(3, "^v")>]
[<InlineData(3, "^>v<")>]
[<InlineData(11, "^v^v^v^v^v")>]
let ``Day03 robo-santa house count works`` expected input =
    Assert.Equal(expected, visitedHousesWithRoboCount input)

[<Fact>]
let ``Day03 Stars`` () =
    try
        Assert.Equal("2565", job1 ())
        Assert.Equal("2639", job2 ())
    with :? System.NotImplementedException ->
        ()
