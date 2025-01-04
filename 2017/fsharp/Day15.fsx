#load "Utils.fsx"
open Utils

let makeGenerator factor pred =
    Seq.unfold (fun prev ->
        let next = prev * factor % 2147483647UL
        Some(next, next))
    >> Seq.filter pred

let genA = makeGenerator 16807UL

let genB = makeGenerator 48271UL

let genPairs (predA, predB) (seedA, seedB) =
    Seq.zip (genA predA seedA) (genB predB seedB)

let isMatch (a, b) =
    let mask = 0xFFFFUL
    a &&& mask = (b &&& mask)

let countMatches n pairs =
    pairs |> Seq.truncate n |> Seq.filter isMatch |> Seq.length

let parseSeeds lines =
    let parseSeed line =
        line |> String.split ' ' |> Array.item 4 |> uint64

    lines |> Array.item 0 |> parseSeed, lines |> Array.item 1 |> parseSeed

let predAll _ = true
let predA x = x % 4UL = 0UL
let predB x = x % 8UL = 0UL

let part1 input =
    input
    |> parseSeeds
    |> genPairs (predAll, predAll)
    |> countMatches 40_000_000

let part2 input =
    input |> parseSeeds |> genPairs (predA, predB) |> countMatches 5_000_000

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
