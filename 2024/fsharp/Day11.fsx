#load "Utils.fsx"

type Cache = System.Collections.Generic.Dictionary<int64 * int, int64>

let parse line = line |> Utils.String.parseInt64s ' '

let rec count (cache: Cache) blinks stone =
    match blinks with
    | 0 -> 1L
    | _ ->
        let key = stone, blinks

        match cache.TryGetValue key with
        | true, value -> value
        | false, _ ->
            let result =
                match stone |> string with
                | "0" -> count cache (blinks - 1) 1
                | s when s.Length % 2 = 0 ->
                    let mid = s.Length / 2
                    let left = s.[0 .. mid - 1] |> int64
                    let right = s.[mid..] |> int64

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
