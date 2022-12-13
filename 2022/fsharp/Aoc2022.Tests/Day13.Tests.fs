module Day13.Tests

open Xunit
open Utils
open Day13

let input =
    """
    [1,1,3,1,1]
    [1,1,5,1,1]

    [[1],[2,3,4]]
    [[1],4]

    [9]
    [[8,7,6]]

    [[4,4],4,4]
    [[4,4],4,4,4]

    [7,7,7,7]
    [7,7,7]

    []
    [3]

    [[[]]]
    [[]]

    [1,[2,[3,[4,[5,6,7]]]],8,9]
    [1,[2,[3,[4,[5,6,0]]]],8,9]
    """
    |> String.toLines
    |> String.concat "\n"

[<Theory>]
[<InlineData("[]", "[]")>]
[<InlineData("[1]", "[1]")>]
[<InlineData("[1,[12,3]]", "[1,[12,3]]")>]
[<InlineData("[[12,3],4]", "[[12,3],4]")>]
[<InlineData("[[12,3],[4,56]]", "[[12,3],[4,56]]")>]
[<InlineData("[[12,3],99,[4,56]]", "[[12,3],99,[4,56]]")>]
let ``Day13 parse works`` expected input =
    Assert.Equal(expected, input |> Data.parse |> Data.dump)

[<Fact>]
let ``Day13 part1 works`` () =
    Assert.Equal(13, input |> Data.parsePairs |> part1)

[<Fact>]
let ``Day13 part2 works`` () =
    Assert.Equal(140, input |> Data.parsePairs |> part2)

[<Fact>]
let ``Day13 Stars`` () =
    try
        Assert.Equal("5208", job1 ())
        Assert.Equal("25792", job2 ())
    with :? System.NotImplementedException ->
        ()
