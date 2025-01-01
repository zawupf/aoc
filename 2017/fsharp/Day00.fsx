#load "Utils.fsx"
open Utils

let part1 input = input |> notImplemented ()

let part2 input = input |> notImplemented ()

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Part 1" nan solution1
Test.run "Part 2" nan solution2

#load "_benchmark.fsx"
