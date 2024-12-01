#load "Utils.fsx"
open Utils.FancyPatterns

let day = __SOURCE_FILE__[3..4]

let input = Utils.readInputLines day

let parseInput (lines: string array) =
    lines
    |> Array.map (fun line ->
        match line with
        | Regex @"(\d+)\s+(\d+)" [ Int l; Int r ] -> l, r
        | _ -> failwithf "Invalid line: %A" line)
    |> Array.unzip

let part1 input =
    let left, right = input |> parseInput
    Array.sortInPlace left
    Array.sortInPlace right

    Array.zip left right |> Array.sumBy (fun (l, r) -> abs (l - r))

let part2 input =
    let left, right = input |> parseInput
    let left = Array.countBy id left
    let right = Array.countBy id right |> Map.ofArray

    left |> Array.sumBy (fun (k, v) ->
        v * k * (right |> Map.tryFind k |> Option.defaultValue 0))

let solution1 () = part1 input

let solution2 () = part2 input

Utils.Test.run "Part 1" 1651298 solution1
Utils.Test.run "Part 2" 21306195 solution2
