module Day11

open Utils

type Point = int * int
type Grid = int array array

module Grid =
    let build (lines: string list) =
        lines
        |> List.map (fun line ->
            line.ToCharArray()
            |> Array.map (fun c -> int c - int '0'))
        |> List.toArray

    let contains (_grid: Grid) (x, y) =
        let nx, ny = 10, 10
        x >= 0 && x < nx && y >= 0 && y < ny

    let adjacentPoints (grid: Grid) (x, y) =
        [ x - 1, y
          x + 1, y
          x, y - 1
          x, y + 1
          x - 1, y - 1
          x + 1, y + 1
          x + 1, y - 1
          x - 1, y + 1 ]
        |> List.filter (grid |> contains)

    let flashCount (grid: Grid) =
        let incrementAll () =
            grid
            |> Array.indexed
            |> Array.fold
                (fun flashes (y, row) ->
                    row
                    |> Array.indexed
                    |> Array.fold
                        (fun flashes (x, power) ->
                            let power = power + 1
                            grid.[y].[x] <- power

                            if power = 10 then
                                (x, y) :: flashes
                            else
                                flashes)
                        flashes)
                List.empty

        let clearFlashed () =
            grid
            |> Array.iteri (fun y row ->
                row
                |> Array.iteri (fun x power ->
                    if power >= 10 then grid.[y].[x] <- 0
                    ())

                ())

        let flash flashes =
            let rec loop flashes count =
                match flashes with
                | [] -> count
                | p :: flashes' ->
                    let adjacentFlashes =
                        p
                        |> adjacentPoints grid
                        |> List.fold
                            (fun flashes (x, y) ->
                                let power = grid.[y].[x] + 1
                                grid.[y].[x] <- power

                                if power = 10 then
                                    (x, y) :: flashes
                                else
                                    flashes)
                            List.empty

                    loop (flashes' @ adjacentFlashes) (count + 1)

            loop flashes 0

        seq {
            while true do
                yield incrementAll () |> flash
                clearFlashed ()
        }

let totalFlashCount100 lines =
    lines
    |> Grid.build
    |> Grid.flashCount
    |> Seq.take 100
    |> Seq.sum

let firstStepFullFlash lines =
    lines
    |> Grid.build
    |> Grid.flashCount
    |> Seq.takeWhile (fun count -> count <> 100)
    |> Seq.length
    |> (+) 1

let input = "11" |> readInputLines |> Seq.toList

let job1 () = input |> totalFlashCount100 |> string

let job2 () = input |> firstStepFullFlash |> string
