module Tests22

open Xunit
open Day22
open Utils

[<Fact>]
let ``Day22 dealIntoNewStack works`` () =
    Assert.Equal<List<_>>([ 9; 8; 7; 6; 5; 4; 3; 2; 1; 0 ], deck 10 |> dealIntoNewStack |> Seq.toList)

[<Fact>]
let ``Day22 cut works`` () =
    Assert.Equal<List<_>>([ 3; 4; 5; 6; 7; 8; 9; 0; 1; 2 ], deck 10 |> cut 3 |> Seq.toList)
    Assert.Equal<List<_>>([ 6; 7; 8; 9; 0; 1; 2; 3; 4; 5 ], deck 10 |> cut -4 |> Seq.toList)

[<Fact>]
let ``Day22 dealWithIncrement works`` () =
    Assert.Equal<List<_>>([ 0; 7; 4; 1; 8; 5; 2; 9; 6; 3 ], deck 10 |> dealWithIncrement 3 |> Seq.toList)

[<Theory>]
[<InlineData("""
    deal with increment 7
    deal into new stack
    deal into new stack
    Result: 0 3 6 9 2 5 8 1 4 7
""")>]
[<InlineData("""
    cut 6
    deal with increment 7
    deal into new stack
    Result: 3 0 7 4 1 8 5 2 9 6
""")>]
[<InlineData("""
    deal with increment 7
    deal with increment 9
    cut -2
    Result: 6 3 0 7 4 1 8 5 2 9
""")>]
[<InlineData("""
    deal into new stack
    cut -2
    deal with increment 7
    cut 8
    cut -4
    deal with increment 7
    cut 3
    deal with increment 9
    deal with increment 3
    cut -1
    Result: 9 2 5 8 1 4 7 0 3 6
""")>]
let ``Day22 shuffle works`` input =
    let data lines =
        let all = lines |> String.trim |> String.split '\n' |> Array.map String.trim

        let expected =
            all
            |> Array.last
            |> String.substring 8
            |> String.split ' '
            |> Array.map int
            |> Array.toList

        let result =
            all |> Array.take ((all |> Array.length) - 1) |> shuffle 10 |> Seq.toList

        expected, result

    input |> data |> Assert.Equal<List<_>>

[<Fact>]
let ``Day22 Stars`` () =
    Assert.Equal("3939", job1 ())
    // Assert.Equal("", job2())
    ()
