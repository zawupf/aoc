#load "Utils.fsx"
open Utils

let part1 input = input |> notImplemented ()

let part2 input = input |> notImplemented ()

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" None (fun () -> part1 testInput[0])
Test.run "Test 2" None (fun () -> part2 testInput[0])

Test.run "Part 1" None solution1
Test.run "Part 2" None solution2

#load "_benchmark.fsx"
