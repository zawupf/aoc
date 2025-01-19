#load "Utils.fsx"
open Utils

let part1 input =
    let input = input |> String.parseInts " "

    let rec loop result i stack =
        match stack with
        | [] -> result
        | (0, metaCount) :: stack ->
            loop
                (result + Array.sum input[i .. i + metaCount - 1])
                (i + metaCount)
                stack
        | (childCount, metaCount) :: stack ->
            let childCount' = input[i]
            let metaCount' = input.[i + 1]

            let stack' =
                (childCount', metaCount')
                :: (childCount - 1, metaCount)
                :: stack

            loop result (i + 2) stack'

    loop 0 2 [ input[0], input[1] ]

let part2 input =
    let input = input |> String.parseInts " "

    let rec value i =
        let childCount = input[i]
        let metaCount = input[i + 1]
        let i = i + 2

        match childCount with
        | 0 -> Array.sum input[i .. i + metaCount - 1], i + metaCount
        | _ ->
            let values, i =
                [ 1..childCount ]
                |> List.fold
                    (fun (values, i) _ ->
                        let value, i = value i
                        value :: values, i)
                    ([], i)

            let values = values |> List.rev

            let value =
                input[i .. i + metaCount - 1]
                |> Array.choose (fun i -> values |> List.tryItem (i - 1))
                |> Array.sum

            value, i + metaCount

    value 0 |> fst

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Part 1" 41849 solution1
Test.run "Part 2" 32487 solution2

#load "_benchmark.fsx"
