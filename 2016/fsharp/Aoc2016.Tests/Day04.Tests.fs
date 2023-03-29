module Day04.Tests

open Xunit
open Utils
open Day04

[<Theory>]
[<InlineData(true, "aaaaa-bbb-z-y-x-123[abxyz]")>]
[<InlineData(true, "a-b-c-d-e-f-g-h-987[abcde]")>]
[<InlineData(true, "not-a-real-room-404[oarel]")>]
[<InlineData(false, "totally-real-room-200[decoy]")>]
let ``Day04 is real room works`` expected room =
    Assert.Equal(expected, room |> Room.parse |> Room.isReal)

[<Fact>]
let ``Day04 sum of real sector ids works`` () =
    Assert.Equal(
        1514,
        [
            "aaaaa-bbb-z-y-x-123[abxyz]"
            "a-b-c-d-e-f-g-h-987[abcde]"
            "not-a-real-room-404[oarel]"
            "totally-real-room-200[decoy]"
        ]
        |> List.map Room.parse
        |> List.filter Room.isReal
        |> List.sumBy (fun room -> room.SectorID)
    )

[<Fact>]
let ``Day04 real name works`` () =
    Assert.Equal(
        "very encrypted name",
        "qzmt-zixmtkozy-ivhz-343[decoy]" |> Room.parse |> Room.realName
    )

[<Fact>]
let ``Day04 Stars`` () =
    try
        Assert.Equal("158835", job1 ())
        Assert.Equal("993", job2 ())
    with :? System.NotImplementedException ->
        ()
