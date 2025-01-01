#load "Utils.fsx"
open Utils.FancyPatterns

let parse line = line |> Utils.String.parseInt64s ' '

let (|SplitEven|_|) value =
    let s = value |> string

    match s.Length with
    | Even ->
        let m = s.Length / 2
        Some(s.[0 .. m - 1] |> int64, s.[m..] |> int64)
    | Odd -> None

let count blinks stone =
    let getStoneBlinks = Utils.useCache<int64 * int, int64> ()

    let rec loop blinks stone =
        match blinks with
        | 0 -> 1L
        | _ ->
            getStoneBlinks (stone, blinks)
            <| fun () ->
                match stone with
                | 0L -> loop (blinks - 1) 1
                | SplitEven(left, right) ->
                    loop (blinks - 1) left + loop (blinks - 1) right
                | _ -> loop (blinks - 1) (stone * 2024L)

    loop blinks stone

let sumByCount blinks stones = stones |> Seq.sumBy (count blinks)

let part1 input = input |> parse |> sumByCount 25

let part2 input = input |> parse |> sumByCount 75

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 220722L solution1
Utils.Test.run "Part 2" 261952051690787L solution2

#load "_benchmark.fsx"
