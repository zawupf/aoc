module Day08

open Utils

type Screen = { Grid: char[][]; Width: int; Height: int }

module Screen =
    let empty width height = {
        Grid = [|
            for _ in 1..height do
                Array.replicate width '.'
        |]
        Width = width
        Height = height
    }

    let normalize row col screen = row % screen.Height, col % screen.Width

    let copy screen = { screen with Grid = screen.Grid |> Array.map Array.copy }

    let get row col screen =
        let row, col = normalize row col screen
        screen.Grid.[row].[col]

    let set value row col screen =
        let row, col = normalize row col screen
        screen.Grid.[row].[col] <- value
        screen

    let setRow value row screen =
        let row, _ = normalize row 0 screen
        screen.Grid.[row] <- value
        screen

    let setCol value col screen =
        let _, col = normalize 0 col screen

        value
        |> Array.iteri (fun row char -> screen |> set char row col |> ignore)

        screen

    let rect w h screen =
        for row in 0 .. h - 1 do
            for col in 0 .. w - 1 do
                (set '#' row col screen) |> ignore

        screen

    let rotateRow row by screen =
        let first = screen.Width - (by % screen.Width)
        let last = first + screen.Width - 1
        let row' = [| for col in first..last -> screen |> get row col |]
        setRow row' row screen

    let rotateCol col by screen =
        let first = screen.Height - (by % screen.Height)
        let last = first + screen.Height - 1
        let col' = [| for row in first..last -> screen |> get row col |]
        setCol col' col screen

    let toString screen =
        screen.Grid |> Array.map String.ofChars |> String.join "\n"

    let next instruction screen =
        match instruction with
        | Regex @"^rect (\d+)x(\d+)$" [ Int w; Int h ] ->
            screen |> copy |> rect w h
        | Regex @"^rotate row y=(\d+) by (\d+)$" [ Int row; Int by ] ->
            screen |> copy |> rotateRow row by
        | Regex @"^rotate column x=(\d+) by (\d+)$" [ Int col; Int by ] ->
            screen |> copy |> rotateCol col by
        | _ -> failwith $"Invalid instruction: %s{instruction}"

    let litCount screen =
        screen.Grid
        |> Array.map (Array.sumBy (fun c -> if c = '#' then 1 else 0))
        |> Array.sum


let input = readInputLines "08" |> Seq.toList

let job1 () =
    input
    |> List.fold
        (fun screen instruction -> screen |> Screen.next instruction)
        (Screen.empty 50 6)
    |> Screen.litCount
    |> string

let job2 () =
    "\n"
    + (input
       |> List.fold
           (fun screen instruction -> screen |> Screen.next instruction)
           (Screen.empty 50 6)
       |> Screen.toString)
