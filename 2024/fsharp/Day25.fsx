#load "Utils.fsx"

type Heights =
    | Lock of int[]
    | Key of int[]

let parse input =
    input
    |> Utils.String.toSections
    |> Array.map (fun section ->
        let lines = section |> Utils.String.toLines
        let isLock = lines[0] = "#####"

        let heights =
            lines[1..5]
            |> Array.map Utils.String.toCharArray
            |> Array.transpose
            |> Array.map (fun row ->
                row |> Array.sumBy (fun c -> if c = '#' then 1 else 0))

        if isLock then Lock heights else Key heights)
    |> Array.partition (function
        | Lock _ -> true
        | _ -> false)

let isFitting lock key =
    match lock, key with
    | Lock lock, Key key ->
        Array.zip lock key |> Array.forall (fun (l, k) -> l + k <= 5)
    | _ -> failwith "Invalid input"

let countFits locks keys =
    locks
    |> Array.sumBy (fun lock ->
        keys |> Array.sumBy (fun key -> if isFitting lock key then 1 else 0))

let part1 input = input |> parse ||> countFits

let part2 input = input |> Utils.notImplemented ()

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 3077 solution1
// Utils.Test.run "Part 2" nan solution2

#load "_benchmark.fsx"
