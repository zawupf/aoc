module Day13.Tests

open Xunit
open Utils
open Day13

[<Fact>]
let ``Day13 Area works`` () =
    let area = Area.empty 10u

    let row y =
        [| for x in 0u .. 9u -> area |> Area.space (x, y) |> Space.toChar |]
        |> String.ofChars

    Assert.Equal(".#.####.##", row 0u)
    Assert.Equal("..#..#...#", row 1u)
    Assert.Equal("#....##...", row 2u)
    Assert.Equal("###.#.###.", row 3u)
    Assert.Equal(".##..#..#.", row 4u)
    Assert.Equal("..##....#.", row 5u)
    Assert.Equal("#...##.###", row 6u)

[<Fact>]
let ``Day13 PriorityQueue works`` () =
    let q =
        PriorityQueue.empty
        |> PriorityQueue.enqueue PriorityQueue.Low "low"
        |> PriorityQueue.enqueue PriorityQueue.High "high"
        |> PriorityQueue.enqueue PriorityQueue.Normal "normal"
        |> PriorityQueue.enqueue PriorityQueue.Low "low2"

    let item, q = q |> PriorityQueue.dequeue
    Assert.Equal("high", item)
    let item, q = q |> PriorityQueue.dequeue
    Assert.Equal("normal", item)
    let item, q = q |> PriorityQueue.dequeue
    Assert.Equal("low", item)
    let item, q = q |> PriorityQueue.dequeue
    Assert.Equal("low2", item)

[<Fact>]
let ``Day13 PrioList works`` () =
    let l =
        PriorityList.empty
        |> PriorityList.push PriorityList.Low "low"
        |> PriorityList.push PriorityList.High "high"
        |> PriorityList.push PriorityList.Low "low2"

    let item, l = l |> PriorityList.pop
    Assert.Equal("high", item)
    let item, l = l |> PriorityList.pop
    Assert.Equal("low2", item)
    let item, l = l |> PriorityList.pop
    Assert.Equal("low", item)

[<Theory>]
[<InlineData(0u, 1u, 1u)>]
[<InlineData(1u, 1u, 2u)>]
[<InlineData(4u, 3u, 1u)>]
[<InlineData(5u, 4u, 1u)>]
[<InlineData(11u, 7u, 4u)>]
let ``Day13 minSteps works`` expected x y =
    Assert.Equal(expected, Area.empty 10u |> minSteps (1u, 1u) (x, y))

[<Theory>]
[<InlineData(1u, 0u)>]
[<InlineData(3u, 1u)>]
[<InlineData(5u, 2u)>]
[<InlineData(11u, 5u)>]
let ``Day13 maxPlaces works`` expected maxSteps =
    Assert.Equal(expected, Area.empty 10u |> maxPlaces maxSteps (1u, 1u))

[<Fact>]
let ``Day13 Stars`` () =
    try
        Assert.Equal("90", job1 ())
        Assert.Equal("135", job2 ())
    with :? System.NotImplementedException ->
        ()
