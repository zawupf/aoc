#load "Utils.fsx"
open Utils

let inline predAll _ = true
let inline predA x = x % 4UL = 0UL
let inline predB x = x % 8UL = 0UL

let countMatches count (predA, predB) (seedA, seedB) =
    let mutable a = seedA
    let mutable b = seedB

    let inline nextA () = a <- a * 16807UL % 2147483647UL
    let inline nextB () = b <- b * 48271UL % 2147483647UL

    let inline nextValidA () =
        nextA ()

        while not (predA a) do
            nextA ()

    let inline nextValidB () =
        nextB ()

        while not (predB b) do
            nextB ()

    let inline isMatch () = a &&& 0xFFFFUL = (b &&& 0xFFFFUL)

    let mutable matches = 0

    for _ in 1..count do
        nextValidA ()
        nextValidB ()

        if isMatch () then
            matches <- matches + 1

    matches

let parseSeeds lines =
    let parseSeed line =
        line |> String.split ' ' |> Array.item 4 |> uint64

    lines |> Array.item 0 |> parseSeed, lines |> Array.item 1 |> parseSeed

let part1 input =
    input |> parseSeeds |> countMatches 40_000_000 (predAll, predAll)

let part2 input =
    input |> parseSeeds |> countMatches 5_000_000 (predA, predB)

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
Generator A starts with 65
Generator B starts with 8921
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" 588 (fun () -> part1 testInput[0])
Test.run "Test 2" 309 (fun () -> part2 testInput[0])

Test.run "Part 1" 594 solution1
Test.run "Part 2" 328 solution2

#load "_benchmark.fsx"
