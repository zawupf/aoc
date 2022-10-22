module Day01.Tests

open Xunit
open Day01
open System.Collections.Generic

[<Fact>]
let ``combine works`` () =
    let expected = []
    let result = List.empty |> combine
    Assert.Equal<IEnumerable<int * int>>(expected, result)

    let expected = []
    let result = [ 1 ] |> combine
    Assert.Equal<IEnumerable<int * int>>(expected, result)

    let expected = [ (1, 2) ]
    let result = [ 1; 2 ] |> combine
    Assert.Equal<IEnumerable<int * int>>(expected, result)

    let expected = [ (1, 2); (1, 3); (2, 3) ]
    let result = [ 1; 2; 3 ] |> combine
    Assert.Equal<IEnumerable<int * int>>(expected, result)
    ()

[<Fact>]
let ``combine2 works`` () =
    let expected = []
    let result = List.empty |> combine2
    Assert.Equal<IEnumerable<int * int * int>>(expected, result)

    let expected = []
    let result = [ 1 ] |> combine2
    Assert.Equal<IEnumerable<int * int * int>>(expected, result)

    let expected = []
    let result = [ 1; 2 ] |> combine2
    Assert.Equal<IEnumerable<int * int * int>>(expected, result)

    let expected = [ (1, 2, 3) ]
    let result = [ 1; 2; 3 ] |> combine2
    Assert.Equal<IEnumerable<int * int * int>>(expected, result)

    let expected = [ (1, 2, 3); (1, 2, 4); (1, 3, 4); (2, 3, 4) ]

    let result = [ 1; 2; 3; 4 ] |> combine2
    Assert.Equal<IEnumerable<int * int * int>>(expected, result)
    ()

[<Fact>]
let ``Day01 Stars`` () =
    Assert.Equal("1018944", job1 ())
    Assert.Equal("8446464", job2 ())
    ()
