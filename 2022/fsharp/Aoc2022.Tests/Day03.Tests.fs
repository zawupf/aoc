module Day03.Tests

open Xunit
open Utils
open Day03

let input = [|
    "vJrwpWtwJgWrhcsFMMfFFhFp"
    "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL"
    "PmmdzqPrVvPwwTWBwg"
    "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn"
    "ttgJtRGJQctTZtZT"
    "CrZsJsPPZsGzwwsLwLmpwMDw"
|]

[<Fact>]
let ``Day03 sumOfDoubleItemPrios works`` () =
    Assert.Equal(157, input |> sumOfDoubleItemPrios)

[<Fact>]
let ``Day03 sumOfBadgeItemPrios works`` () =
    Assert.Equal(70, input |> sumOfBadgeItemPrios)

[<Fact>]
let ``Day03 Stars`` () =
    try
        Assert.Equal("8109", job1 ())
        Assert.Equal("2738", job2 ())
    with :? System.NotImplementedException ->
        ()
