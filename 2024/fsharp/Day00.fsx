#load "Utils.fsx"
open Utils.FancyPatterns

let day = __SOURCE_FILE__[3..4]

let input = Utils.readInputLines day

let part1 input = input |> Utils.notImplemented ()

let part2 input = input |> Utils.notImplemented ()

let solution1 () = part1 input

let solution2 () = part2 input

Utils.Test.run "Part 1" nan solution1
Utils.Test.run "Part 2" nan solution2
