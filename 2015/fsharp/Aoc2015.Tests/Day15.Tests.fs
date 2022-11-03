module Day15.Tests

open Xunit
open Utils
open Day15

let ingredients =
    [ "Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8"
      "Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3" ]
    |> List.map Ingredient.parse

[<Fact>]
let ``Day15 total score works`` () =
    Assert.Equal(62_842_880L, ingredients |> List.zip [ 44; 56 ] |> totalScore)

[<Fact>]
let ``Day15 max total score works`` () =
    Assert.Equal(62_842_880L, ingredients |> maxTotalScore)

[<Fact>]
let ``Day15 amounts works`` () =
    amounts (ingredients |> List.length)
    |> List.iter (fun l -> Assert.Equal(100, l |> List.sum))

[<Fact>]
let ``Day15 Stars`` () =
    try
        Assert.Equal("13882464", job1 ())
        Assert.Equal("11171160", job2 ())
    with :? System.NotImplementedException ->
        ()
