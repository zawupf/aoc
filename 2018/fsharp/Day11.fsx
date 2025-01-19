#load "Utils.fsx"
open Utils

let initGrid serialNumber =
    Array2D.initBased 1 1 300 300 (fun x y ->
        let rackId = x + 10
        (rackId * y + serialNumber) * rackId / 100 % 10 - 5)

let part1 input =
    let grid = input |> int |> initGrid

    [
        for x = 1 to 298 do
            for y = 1 to 298 do
                yield x, y
    ]
    |> List.maxBy (fun (x, y) ->
        [
            for x = x to x + 2 do
                for y = y to y + 2 do
                    grid[x, y]
        ]
        |> List.sum)
    ||> sprintf "%d,%d"

let part2 input =
    let grid = input |> int |> initGrid

    [
        for s = 1 to 300 do
            for x = 1 to 301 - s do
                for y = 1 to 301 - s do
                    yield x, y, s
    ]
    |> List.maxBy (fun (x, y, s) ->
        [
            for x = x to x + s - 1 do
                for y = y to y + s - 1 do
                    grid[x, y]
        ]
        |> List.sum)
    |> fun (x, y, s) -> sprintf "%d,%d,%d" x y s

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Part 1" "33,34" solution1
Test.run "Part 2" "235,118,14" solution2

#load "_benchmark.fsx"
