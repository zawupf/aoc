module Day18

open Utils

type Grid = { Data: int[]; Size: int }

module Grid =
    let parse overrides input =
        let lines = input |> String.trim |> String.split '\n'
        let size = lines |> Seq.length

        let data =
            lines
            |> String.join ""
            |> Seq.map (fun c -> if c = '#' then 1 else 0)
            |> Seq.toArray

        { Data = data; Size = size } |> overrides

    let toString grid =
        grid.Data
        |> Array.map (fun on -> if on = 0 then "." else "#")
        |> String.join ""
        |> Seq.chunkBySize grid.Size
        |> Seq.map (fun o -> System.String(o) |> string)
        |> String.join "\n"

    let item index { Data = data } =
        if index >= 0 && index < data.Length then
            data.[index]
        else
            0

    let countAdjacentLights i grid =
        let { Size = size } = grid

        let toPos index = index % size, index / size

        let toIndex pos =
            match pos with
            | x, y when x >= 0 && x < size && y >= 0 && y < size -> y * size + x
            | _ -> -1

        let x, y = i |> toPos

        [| x - 1, y - 1
           x, y - 1
           x + 1, y - 1
           x - 1, y
           x + 1, y
           x - 1, y + 1
           x, y + 1
           x + 1, y + 1 |]
        |> Array.sumBy (fun pos -> grid |> item (pos |> toIndex))

    let countLights { Data = data } = data |> Array.sum

    let next overrides grid =
        let nextState index on =
            match on, countAdjacentLights index grid with
            | 1, 2
            | 1, 3
            | 0, 3 -> 1
            | _, _ -> 0

        { grid with
            Data = grid.Data |> Array.mapi nextState }
        |> overrides

let grids overrides init =
    Seq.unfold (fun grid -> Some(grid, Grid.next overrides grid)) init

let cornerLightsStuckOn grid =
    grid.Data.[0] <- 1
    grid.Data.[grid.Size - 1] <- 1
    grid.Data.[grid.Data.Length - grid.Size] <- 1
    grid.Data.[grid.Data.Length - 1] <- 1
    grid

let input = readInputText "18"

let job1 () =
    input
    |> Grid.parse id
    |> grids id
    |> Seq.item 100
    |> Grid.countLights
    |> string

let job2 () =
    input
    |> Grid.parse cornerLightsStuckOn
    |> grids cornerLightsStuckOn
    |> Seq.item 100
    |> Grid.countLights
    |> string
