#load "Utils.fsx"
open System.Text.RegularExpressions

let day = __SOURCE_FILE__[3..4]

let input = Utils.readInputText day

let mul a b = int a * int b

let part1 input =
    Regex.Matches(input, @"mul\((\d{1,3}),(\d{1,3})\)")
    |> Seq.sumBy (fun m ->
        match m.Groups |> Seq.map _.Value |> Seq.toArray with
        | [|_; a; b|] -> mul a b
        | _ -> failwith "Invalid match")

let part2 input =
    Regex.Matches(input, @"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)")
    |> Seq.fold (fun (sum, enabled) m ->
        match m.Groups |> Seq.map _.Value |> Seq.toArray with
        | [|"do()"; _; _|] -> sum, true
        | [|"don't()"; _; _|] -> sum, false
        | [|_; a; b|] ->
            if enabled then
                sum + mul a b, enabled
            else
                sum, enabled
        | _ -> failwith "Invalid match")
        (0, true)
    |> fst

let solution1 () = part1 input

let solution2 () = part2 input

Utils.Test.run "Part 1" 173529487 solution1
Utils.Test.run "Part 2" 99532691 solution2
