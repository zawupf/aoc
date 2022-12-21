module Day21.Tests

open Xunit
open Utils
open Day21

let input =
    """
    root: pppw + sjmn
    dbpl: 5
    cczh: sllz + lgvd
    zczc: 2
    ptdq: humn - dvpt
    dvpt: 3
    lfqf: 4
    humn: 5
    ljgn: 2
    sjmn: drzm * dbpl
    sllz: 4
    pppw: cczh / lfqf
    lgvd: ljgn * ptdq
    drzm: hmdt - zczc
    hmdt: 32
    """
    |> String.toLines

[<Fact>]
let ``Day21 eval works`` () =
    Assert.Equal(152L, input |> parse |> eval "root" |> fst)

[<Fact>]
let ``Day21 guessNumber works`` () =
    Assert.Equal(301L, input |> parse |> guessNumber)

[<Fact>]
let ``Day21 Stars`` () =
    try
        Assert.Equal("291425799367130", job1 ())
        Assert.Equal("3219579395609", job2 ())
    with :? System.NotImplementedException ->
        ()
