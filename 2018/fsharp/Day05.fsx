#load "Utils.fsx"
open Utils

let trigger units =
    let rec loop result units =
        match result, units with
        | _, [] -> result
        | [], u :: us -> loop [ u ] us
        | r :: rs, u :: us when abs (int u - int r) = 32 -> loop rs us
        | rs, u :: us -> loop (u :: rs) us

    loop [] units

let part1 input =
    input |> String.toCharArray |> Array.toList |> trigger |> List.length

let part2 input =
    let toUpper = System.Char.ToUpper
    let units = input |> String.toCharArray |> Array.toList |> trigger

    [
        for unit in [ 'a' .. 'z' ] do
            yield
                units
                |> List.filter (fun u -> u <> unit && u <> toUpper unit)
                |> trigger
                |> List.length
    ]
    |> List.min

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Part 1" 9202 solution1
Test.run "Part 2" 6394 solution2

#load "_benchmark.fsx"
