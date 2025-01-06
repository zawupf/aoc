#load "Utils.fsx"
open Utils

let parsePattern pattern =
    pattern |> String.split "/" |> Array.map String.toCharArray |> array2D

let rotate pattern =
    let n = pattern |> Array2D.length1
    Array2D.init n n (fun y x -> pattern.[n - x - 1, y])

let flipVertical pattern =
    let n = pattern |> Array2D.length1
    Array2D.init n n (fun y x -> pattern.[n - y - 1, x])

let flipHorizontal pattern =
    let n = pattern |> Array2D.length1
    Array2D.init n n (fun y x -> pattern.[y, n - x - 1])

let buildRules lines =
    lines
    |> Array.collect (fun line ->
        match line |> String.split " => " |> Array.map parsePattern with
        | [| input; output |] ->
            [|
                input
                input |> rotate
                input |> rotate |> rotate
                input |> rotate |> rotate |> rotate
                input |> flipVertical
                input |> flipHorizontal
                input |> rotate |> flipVertical
                input |> rotate |> flipHorizontal
            |]
            |> Array.distinct
            |> Array.map (fun input -> input, output)
        | _ -> failwithf "Invalid input: %s" line)
    |> Map.ofArray

let render count' rules =
    let finalLength =
        let mutable len = 3

        for _ in 1..count' do
            let size = if len % 2 = 0 then 2 else 3
            let size' = size + 1
            len <- len / size * size'

        len

    let grid = Array2D.create finalLength finalLength '.'

    let rec loop count len =
        match count with
        | 0 -> grid
        | _ ->
            let size, n =
                match len % 2, len % 3 with
                | 0, _ -> 2, len / 2
                | _, 0 -> 3, len / 3
                | _ -> unreachable ()

            let size' = size + 1

            for y in n - 1 .. -1 .. 0 do
                for x in n - 1 .. -1 .. 0 do
                    Array2D.blit
                        (rules
                         |> Map.find
                             grid[y * size .. (y + 1) * size - 1,
                                  x * size .. (x + 1) * size - 1])
                        0
                        0
                        grid
                        (y * size')
                        (x * size')
                        size'
                        size'

            loop (count - 1) (n * size')

    Array2D.blit (".#./..#/###" |> parsePattern) 0 0 grid 0 0 3 3
    loop count' 3

let countPixels grid =
    [ 0 .. Array2D.length1 grid - 1 ]
    |> List.map (fun y -> grid[y, *])
    |> List.sumBy (Array.sumBy (fun c -> if c = '#' then 1 else 0))

let part1 input =
    input |> buildRules |> render 5 |> countPixels

let part2 input =
    input |> buildRules |> render 18 |> countPixels

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
../.# => ##./#../...
.#./..#/### => #..#/..../..../#..#
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" 12 (fun () ->
    testInput[0] |> buildRules |> render 2 |> countPixels)

Test.run "Part 1" 136 solution1
Test.run "Part 2" 1911767 solution2

#load "_benchmark.fsx"
