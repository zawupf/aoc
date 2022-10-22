module Day09

open Utils

type Point = int * int
type Grid = int array array

module Grid =
    let build (lines: string list) =
        lines
        |> List.map (fun line -> line.ToCharArray() |> Array.map (fun c -> int c - int '0'))
        |> List.toArray

    let height (grid: Grid) (x, y) = grid.[y].[x]

    let dimensions (grid: Grid) = grid.[0].Length, grid.Length

    let contains (grid: Grid) (x, y) =
        let nx, ny = grid |> dimensions
        x >= 0 && x < nx && y >= 0 && y < ny

    let adjacentPoints (grid: Grid) (x, y) =
        [ x - 1, y; x + 1, y; x, y - 1; x, y + 1 ] |> List.filter (grid |> contains)

    let lowPoints (grid: Grid) =
        let nx, ny = grid |> dimensions

        seq {
            for y = 0 to ny - 1 do
                for x = 0 to nx - 1 do
                    let h = (x, y) |> height grid

                    if (x, y) |> adjacentPoints grid |> List.forall (fun p -> h < (p |> height grid)) then
                        yield x, y
        }

    let basinSize (grid: Grid) p =
        let rec build basinPoints p =
            p
            |> adjacentPoints grid
            |> List.filter (fun p -> basinPoints |> Set.contains p |> not && (p |> height grid) <> 9)
            |> List.fold (fun acc p -> p |> build (acc |> Set.add p)) basinPoints

        p |> build (Set.empty |> Set.add p)

let riskLevelSum lines =
    let grid = lines |> Grid.build

    grid |> Grid.lowPoints |> Seq.sumBy (fun p -> 1 + (p |> Grid.height grid))

let basinProduct lines =
    let grid = lines |> Grid.build

    grid
    |> Grid.lowPoints
    |> Seq.map (Grid.basinSize grid)
    |> Seq.map Set.count
    |> Seq.sortDescending
    |> Seq.take 3
    |> Seq.fold (fun acc value -> acc * value) 1

let input = "09" |> readInputLines |> Seq.toList

let job1 () = input |> riskLevelSum |> string

let job2 () = input |> basinProduct |> string
