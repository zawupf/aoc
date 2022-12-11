module Day11.Tests

open Xunit
open Utils
open Day11

let input =
    """
Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1
"""

[<Fact>]
let ``Day11 monkeyBusiness works`` () =
    Assert.Equal(
        10605UL,
        input |> Monkey.parseMany |> monkeyBusiness WithDiv 20
    )

    Assert.Equal(
        2713310158UL,
        input |> Monkey.parseMany |> monkeyBusiness WithoutDiv 10000
    )

[<Fact>]
let ``Day11 Stars`` () =
    try
        Assert.Equal("57838", job1 ())
        Assert.Equal("15050382231", job2 ())
    with :? System.NotImplementedException ->
        ()
