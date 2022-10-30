module Day09.Tests

open Xunit
open Utils
open Day09

[<Fact>]
let ``Day09 routes work`` () =
    let roads =
        [ "London to Dublin = 464"
          "London to Belfast = 518"
          "Dublin to Belfast = 141" ]
        |> parseRoads

    Assert.Equal(6, roads |> List.length)
    Assert.Equal(3, roads |> cities |> List.length)

    let routes = roads |> routes
    Assert.Equal(6, routes |> Seq.length)
    Assert.Equal(605, routes |> Seq.minBy snd |> snd)
    Assert.Equal(982, routes |> Seq.maxBy snd |> snd)

[<Fact>]
let ``Day09 Stars`` () =
    try
        Assert.Equal("141", job1 ())
        Assert.Equal("736", job2 ())
    with :? System.NotImplementedException ->
        ()
