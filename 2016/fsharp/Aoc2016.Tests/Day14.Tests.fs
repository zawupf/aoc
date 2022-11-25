module Day14.Tests

open Xunit
open Utils
open Day14

[<Theory>]
[<InlineData(39, 0)>]
[<InlineData(92, 1)>]
[<InlineData(22728, 63)>]
let ``Day14 keys with hashes1 works`` expected index =
    Assert.Equal(expected, "abc" |> hashes1 |> keys |> Seq.item index |> fst)

[<Theory>]
[<InlineData(10, 0)>]
[<InlineData(22551, 63)>]
let ``Day14 keys with hashes2 works`` expected index =
    Assert.Equal(expected, "abc" |> hashes2 |> keys |> Seq.item index |> fst)

[<Fact>]
let ``Day14 Stars`` () =
    try
        Assert.Equal("35186", job1 ())
        Assert.Equal("22429", job2 ())
    with :? System.NotImplementedException ->
        ()
