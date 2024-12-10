#load "Utils.fsx"

type Grid = int[,]

let parseGrid lines : Grid =
    lines
    |> Array.map (fun line ->
        line |> Utils.String.toCharArray |> Array.map (fun c -> c - '0' |> int))
    |> array2D

let nextSteps current =
    let x, y = current

    seq {
        yield x - 1, y
        yield x + 1, y
        yield x, y - 1
        yield x, y + 1
    }

let isValidStep (x1, y1) (x2, y2) (grid: Grid) =
    let isInside (x, y) =
        x >= 0 && x < grid.GetLength 0 && y >= 0 && y < grid.GetLength 1

    let length () = abs (x1 - x2) + abs (y1 - y2)
    let height () = grid[x2, y2] - grid[x1, y1]

    isInside (x2, y2) && length () = 1 && height () = 1

let rec walk (grid: Grid) start =
    seq {
        let x, y = start

        if grid[x, y] = 9 then
            yield start
        else
            for next in nextSteps start do
                if isValidStep start next grid then
                    yield! walk grid next
    }

let starts (grid: Grid) =
    seq {
        for x in 0 .. grid.GetLength 0 - 1 do
            for y in 0 .. grid.GetLength 1 - 1 do
                if grid[x, y] = 0 then
                    yield x, y
    }

let pathCount grid start = walk grid start |> Seq.length

let endCount grid start =
    walk grid start |> Seq.distinct |> Seq.length

let part1 input =
    let grid = input |> parseGrid
    starts grid |> Seq.sumBy (endCount grid)

let part2 input =
    let grid = input |> parseGrid
    starts grid |> Seq.sumBy (pathCount grid)

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    """
89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732
"""
    |> Utils.String.toLines

Utils.Test.run "Test 1" 36 (fun () -> part1 testInput)
Utils.Test.run "Test 2" 81 (fun () -> part2 testInput)

Utils.Test.run "Part 1" 789 solution1
Utils.Test.run "Part 2" 1735 solution2
