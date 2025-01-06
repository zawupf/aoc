#load "Utils.fsx"
open Utils

type Generation =
    | Gen1
    | Gen2

type State =
    | Clean
    | Weakened
    | Infected
    | Flagged

let countInfections generation burstCount lines =
    let turnLeft (dx, dy) =
        match dx, dy with
        | 0, -1 -> -1, 0
        | -1, 0 -> 0, 1
        | 0, 1 -> 1, 0
        | 1, 0 -> 0, -1
        | _ -> failwith "Invalid direction"

    let turnRight (dx, dy) =
        turnLeft (dx, dy) |> fun (dx, dy) -> -dx, -dy

    let reverse (dx, dy) = -dx, -dy

    let rec loop count infected (x, y) (dx, dy) infections =
        match
            count,
            generation,
            infected
            |> Dictionary.tryGetValue (x, y)
            |> Option.defaultValue Clean
        with
        | 0, _, _ -> infections
        | _, Gen1, Infected ->
            let dx, dy = turnRight (dx, dy)

            loop
                (count - 1)
                (infected |> Dictionary.remove (x, y))
                (x + dx, y + dy)
                (dx, dy)
                infections
        | _, Gen1, Clean ->
            let dx, dy = turnLeft (dx, dy)

            loop
                (count - 1)
                (infected |> Dictionary.set (x, y) Infected)
                (x + dx, y + dy)
                (dx, dy)
                (infections + 1)
        | _, Gen1, _ -> unreachable ()
        | _, Gen2, Clean ->
            let dx, dy = turnLeft (dx, dy)

            loop
                (count - 1)
                (infected |> Dictionary.set (x, y) Weakened)
                (x + dx, y + dy)
                (dx, dy)
                infections
        | _, Gen2, Weakened ->
            loop
                (count - 1)
                (infected |> Dictionary.set (x, y) Infected)
                (x + dx, y + dy)
                (dx, dy)
                (infections + 1)
        | _, Gen2, Infected ->
            let dx, dy = turnRight (dx, dy)

            loop
                (count - 1)
                (infected |> Dictionary.set (x, y) Flagged)
                (x + dx, y + dy)
                (dx, dy)
                infections
        | _, Gen2, Flagged ->
            let dx, dy = reverse (dx, dy)

            loop
                (count - 1)
                (infected |> Dictionary.remove (x, y))
                (x + dx, y + dy)
                (dx, dy)
                infections

    let infected =
        lines
        |> Array.mapi (fun y line ->
            line
            |> String.toCharArray
            |> Array.mapi (fun x c -> x, y, c)
            |> Array.filter (fun (_, _, c) -> c = '#')
            |> Array.map (fun (x, y, _) -> (x, y), Infected))
        |> Array.concat
        |> Dictionary.ofSeq

    let m = lines.Length / 2
    loop burstCount infected (m, m) (0, -1) 0

let part1 input = input |> countInfections Gen1 10000

let part2 input = input |> countInfections Gen2 10000000

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
..#
#..
...
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" 5587 (fun () -> part1 testInput[0])
Test.run "Test 2" 2511944 (fun () -> part2 testInput[0])

Test.run "Part 1" 5450 solution1
Test.run "Part 2" 2511957 solution2

#load "_benchmark.fsx"
