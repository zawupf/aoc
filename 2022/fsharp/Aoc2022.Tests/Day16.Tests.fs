module Day16.Tests

open Xunit
open Utils
open Day16

let input =
    """
    Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
    Valve BB has flow rate=13; tunnels lead to valves CC, AA
    Valve CC has flow rate=2; tunnels lead to valves DD, BB
    Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
    Valve EE has flow rate=3; tunnels lead to valves FF, DD
    Valve FF has flow rate=0; tunnels lead to valves EE, GG
    Valve GG has flow rate=0; tunnels lead to valves FF, HH
    Valve HH has flow rate=22; tunnel leads to valve GG
    Valve II has flow rate=0; tunnels lead to valves AA, JJ
    Valve JJ has flow rate=21; tunnel leads to valve II
    """
    |> String.toLines

[<Theory>]
[<InlineData(1651, 1, 30)>]
[<InlineData(1707, 2, 26)>]
let ``Day16 maxPressure works`` expected numRunners totalTime =
    Assert.Equal(expected, input |> maxPressure numRunners totalTime)

[<Fact>]
let ``Day16 Stars`` () =
    try
        Assert.Equal("2183", job1 ())
        // Assert.Equal("2911", job2 ()) // FIXME: performance (>1.5h)
        ()
    with :? System.NotImplementedException ->
        ()
