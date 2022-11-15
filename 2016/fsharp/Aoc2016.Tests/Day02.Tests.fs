module Day02.Tests

open Xunit
open Utils
open Day02

[<Fact>]
let ``Day02 moveInSquare works`` () =
    let test direction keys expected =
        keys
        |> List.map (moveInSquare direction)
        |> List.zip expected
        |> List.iter Assert.Equal

    test "U" [ 1; 2; 3; 4; 5; 6; 7; 8; 9 ] [ 1; 2; 3; 1; 2; 3; 4; 5; 6 ]
    test "D" [ 1; 2; 3; 4; 5; 6; 7; 8; 9 ] [ 4; 5; 6; 7; 8; 9; 7; 8; 9 ]
    test "L" [ 1; 2; 3; 4; 5; 6; 7; 8; 9 ] [ 1; 1; 2; 4; 4; 5; 7; 7; 8 ]
    test "R" [ 1; 2; 3; 4; 5; 6; 7; 8; 9 ] [ 2; 3; 3; 5; 6; 6; 8; 9; 9 ]

[<Fact>]
let ``Day02 moveInDiamond works`` () =
    let test direction keys expected =
        keys
        |> List.map (moveInDiamond direction)
        |> List.zip expected
        |> List.iter Assert.Equal

    test
        "U"
        [ 1; 2; 3; 4; 5; 6; 7; 8; 9; 10; 11; 12; 13 ]
        [ 1; 2; 1; 4; 5; 2; 3; 4; 9; 6; 7; 8; 11 ]

    test
        "D"
        [ 1; 2; 3; 4; 5; 6; 7; 8; 9; 10; 11; 12; 13 ]
        [ 3; 6; 7; 8; 5; 10; 11; 12; 9; 10; 13; 12; 13 ]

    test
        "L"
        [ 1; 2; 3; 4; 5; 6; 7; 8; 9; 10; 11; 12; 13 ]
        [ 1; 2; 2; 3; 5; 5; 6; 7; 8; 10; 10; 11; 13 ]

    test
        "R"
        [ 1; 2; 3; 4; 5; 6; 7; 8; 9; 10; 11; 12; 13 ]
        [ 1; 3; 4; 4; 6; 7; 8; 9; 9; 11; 12; 12; 13 ]

[<Fact>]
let ``Day02 square code works`` () =
    Assert.Equal(
        "1985",
        "ULL\nRRDDD\nLURDL\nUUUUD"
        |> String.split '\n'
        |> Array.toList
        |> code moveInSquare
    )

[<Fact>]
let ``Day02 diamond code works`` () =
    Assert.Equal(
        "5DB3",
        "ULL\nRRDDD\nLURDL\nUUUUD"
        |> String.split '\n'
        |> Array.toList
        |> code moveInDiamond
    )

[<Fact>]
let ``Day02 Stars`` () =
    try
        Assert.Equal("84452", job1 ())
        Assert.Equal("D65C3", job2 ())
    with :? System.NotImplementedException ->
        ()
