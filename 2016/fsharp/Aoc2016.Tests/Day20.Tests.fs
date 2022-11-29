module Day20.Tests

open Xunit
open Utils
open Day20

[<Fact>]
let ``Day20 findFirstOutOfRange works`` () =
    Assert.Equal(
        3u,
        [ "5-8"; "0-2"; "4-7" ] |> List.map Range.parse |> findFirstOutOfRange
    )

[<Fact>]
let ``Day20 tryMerge works`` () =
    Assert.Equal(Some(1u, 3u), Range.tryMerge (1u, 2u) (2u, 3u))
    Assert.Equal(Some(1u, 4u), Range.tryMerge (1u, 2u) (3u, 4u))
    Assert.Equal(None, Range.tryMerge (1u, 2u) (4u, 5u))
    Assert.Equal(Some(1u, 4u), Range.tryMerge (1u, 4u) (2u, 3u))

[<Fact>]
let ``Day20 combine works`` () =
    Assert.Equal(
        ((1u, 8u), []),
        Range.combine (1u, 2u) [ (3u, 4u); (5u, 6u); (7u, 8u) ]
    )

    Assert.Equal(
        ((1u, 8u), []),
        Range.combine (1u, 2u) [ (3u, 4u); (7u, 8u); (5u, 6u) ]
    )

    Assert.Equal(
        ((1u, 8u), [ 10u, 11u ]),
        Range.combine (1u, 2u) [ (10u, 11u); (3u, 4u); (7u, 8u); (5u, 6u) ]
    )

[<Fact>]
let ``Day20 Stars`` () =
    try
        Assert.Equal("31053880", job1 ())
        Assert.Equal("117", job2 ())
    with :? System.NotImplementedException ->
        ()
