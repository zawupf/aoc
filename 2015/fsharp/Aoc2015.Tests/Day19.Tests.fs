module Day19.Tests

open Xunit
open Utils
open Day19

let replacementsInput = [ "H => HO"; "H => OH"; "O => HH"; "e => H"; "e => O" ]
let replacements = replacementsInput |> List.map parse

[<Theory>]
[<InlineData(4, "HOH")>]
[<InlineData(7, "HOHOHO")>]
let ``Day19 generate molecules works`` expected molecule =
    Assert.Equal(expected, replacements |> generateMoleculesCount molecule)

[<Theory>]
[<InlineData(3, "HOH")>]
[<InlineData(6, "HOHOHO")>]
let ``Day19 reduce molecules works`` expected molecule =
    Assert.Equal(expected, replacements |> reduceMoleculeMinCount molecule)

[<Fact>]
let ``Day19 Stars`` () =
    try
        Assert.Equal("518", job1 ())
        Assert.Equal("200", job2 ())
    with :? System.NotImplementedException ->
        ()
