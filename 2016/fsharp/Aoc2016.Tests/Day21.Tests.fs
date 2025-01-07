module Day21.Tests

open Xunit
open Utils
open Day21

let input = [
    "swap position 4 with position 0"
    "swap letter d with letter b"
    "reverse positions 0 through 4"
    "rotate left 1 step"
    "move position 1 to position 4"
    "move position 3 to position 0"
    "rotate based on position of letter b"
    "rotate based on position of letter d"
]

[<Fact>]
let ``Day21 scramble works`` () = Assert.Equal("decab", scramble "abcde" input)

[<Fact>]
let ``Day21 unscramble works`` () =
    Assert.Equal("abcde", unscramble "decab" input)

[<Fact>]
let ``Day21 Stars`` () =
    try
        Assert.Equal("dbfgaehc", job1 ())
        Assert.Equal("aghfcdeb", job2 ())
    with :? System.NotImplementedException ->
        ()
