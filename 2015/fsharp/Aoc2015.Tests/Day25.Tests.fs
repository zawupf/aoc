module Day25.Tests

open Xunit
open Utils
open Day25

[<Fact>]
let ``Day25 parse works`` () =
    Assert.Equal((2981, 3075), readInputText "25" |> parse)

[<Theory>]
[<InlineData(1, 1, 20151125UL)>]
[<InlineData(5, 1, 77061)>]
[<InlineData(6, 3, 25397450UL)>]
[<InlineData(3, 6, 16474243UL)>]
[<InlineData(6, 6, 27995004)>]
let ``Day25 codeAt works`` row col code = Assert.Equal(code, codeAt row col)

[<Fact>]
let ``Day25 Stars`` () =
    try
        Assert.Equal("9132360", job1 ())
        Assert.Equal("Ho Ho Ho!", job2 ())
    with :? System.NotImplementedException ->
        ()
