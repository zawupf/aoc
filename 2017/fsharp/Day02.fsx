#load "Utils.fsx"
open Utils

let normalize (line: string) = line.Replace("\t", " ")

let minMaxDiff row = Array.max row - Array.min row

let evenDiv row =
    let isEvenDivisible x y = 0 = if x > y then x % y else y % x
    let divide x y = if x > y then x / y else y / x

    let rec loop row =
        match row with
        | [] -> 0
        | x :: xs ->
            match xs |> List.tryFind (isEvenDivisible x) with
            | Some y -> divide x y
            | None -> loop xs

    loop row

let part1 input =
    input |> Array.sumBy (normalize >> String.parseInts " " >> minMaxDiff)

let part2 input =
    input
    |> Array.sumBy (
        normalize >> String.parseInts " " >> Array.toList >> evenDiv
    )

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
        5 1 9 5
        7 5 3
        2 4 6 8
        """
        """
        5 9 2 8
        9 4 7 3
        3 8 6 5
        """
    |]
    |> Array.map String.toLines

Test.run "Test 1" 18 (fun () -> part1 testInput[0])
Test.run "Test 2" 9 (fun () -> part2 testInput[1])

Test.run "Part 1" 50376 solution1
Test.run "Part 2" 267 solution2

#load "_benchmark.fsx"
