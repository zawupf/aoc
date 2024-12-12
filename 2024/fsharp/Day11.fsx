#load "Utils.fsx"
open Utils.FancyPatterns

type Cache = System.Collections.Generic.Dictionary<int64 * int, int64>

let parse line = line |> Utils.String.parseInt64s ' '

let (|SplitEven|_|) value =
    let s = value |> string

    match s.Length with
    | Even ->
        let m = s.Length / 2
        Some(s.[0 .. m - 1] |> int64, s.[m..] |> int64)
    | Odd -> None

let rec count (cache: Cache) blinks stone =
    match blinks with
    | 0 -> 1L
    | _ ->
        let key = stone, blinks

        match cache.TryGetValue key with
        | true, value -> value
        | false, _ ->
            let result =
                match stone with
                | 0L -> count cache (blinks - 1) 1
                | SplitEven(left, right) ->
                    count cache (blinks - 1) left
                    + count cache (blinks - 1) right
                | _ -> count cache (blinks - 1) (stone * 2024L)

            cache.Add(key, result)
            result

let sumByCount blinks stones =
    let cache = Cache()
    stones |> Seq.sumBy (count cache blinks)

let part1 input = input |> parse |> sumByCount 25

let part2 input = input |> parse |> sumByCount 75

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 220722L solution1
Utils.Test.run "Part 2" 261952051690787L solution2
