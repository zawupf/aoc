module Day17.Tests

open Xunit
open Utils
open Day17

[<Theory>]
[<InlineData("DDRRRD", "ihgpwlah")>]
[<InlineData("DDUDRLRRUDRD", "kglvqrro")>]
[<InlineData("DRURDRUDDLLDLUURRDULRLDUUDDDRR", "ulqzkmiv")>]
let ``Day17 walk works`` path salt = Assert.Equal(path, salt |> walk)

[<Theory>]
[<InlineData(370, "ihgpwlah")>]
[<InlineData(492, "kglvqrro")>]
[<InlineData(830, "ulqzkmiv")>]
let ``Day17 explore works`` path salt = Assert.Equal(path, salt |> explore)

[<Fact>]
let ``Day17 Stars`` () =
    try
        Assert.Equal("DDRLRRUDDR", job1 ())
        Assert.Equal("556", job2 ())
    with :? System.NotImplementedException ->
        ()
