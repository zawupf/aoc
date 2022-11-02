module Day13.Tests

open Xunit
open Utils
open Day13

[<Literal>]
let sample =
    """
Alice would gain 54 happiness units by sitting next to Bob.
Alice would lose 79 happiness units by sitting next to Carol.
Alice would lose 2 happiness units by sitting next to David.
Bob would gain 83 happiness units by sitting next to Alice.
Bob would lose 7 happiness units by sitting next to Carol.
Bob would lose 63 happiness units by sitting next to David.
Carol would lose 62 happiness units by sitting next to Alice.
Carol would gain 60 happiness units by sitting next to Bob.
Carol would gain 55 happiness units by sitting next to David.
David would gain 46 happiness units by sitting next to Alice.
David would lose 7 happiness units by sitting next to Bob.
David would gain 41 happiness units by sitting next to Carol.
"""

[<Fact>]
let ``Day13 total happiness works`` () =
    let map = sample |> String.trim |> String.split '\n' |> parse

    Assert.Equal(
        330,
        [| "David"; "Alice"; "Bob"; "Carol" |] |> totalHappiness map
    )

[<Fact>]
let ``Day13 max total happiness works`` () =
    let map = sample |> String.trim |> String.split '\n' |> parse
    Assert.Equal(330, map |> maxTotalHappiness)

[<Fact>]
let ``Day13 Stars`` () =
    try
        Assert.Equal("709", job1 ())
        Assert.Equal("668", job2 ())
    with :? System.NotImplementedException ->
        ()
