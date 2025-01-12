#load "Utils.fsx"
open Utils

let part1 input = input |> Array.sumBy int

let part2 input =
    let deltas = input |> Array.map int

    let rec loop i frequency cache =
        if cache |> HashSet.contains frequency then
            frequency
        else
            loop
                ((i + 1) % deltas.Length)
                (frequency + deltas[i])
                (cache |> HashSet.add frequency)

    loop 0 0 (HashSet())

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Part 1" 490 solution1
Test.run "Part 2" 70357 solution2

#load "_benchmark.fsx"
