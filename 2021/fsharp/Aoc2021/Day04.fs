module Day04

open Utils

type Cell = { Number: int; Marked: bool }

type Board = { Cells: Cell list; Score: int option }

module Board =
    let ofLines lines =
        let cells =
            lines
            |> List.map (String.parseInts ' ')
            |> List.concat
            |> List.map (fun number -> { Number = number; Marked = false })

        { Cells = cells; Score = None }

    let score board =
        match board.Score with
        | Some score -> score
        | None -> 0

    let play n board =
        let cells =
            board.Cells
            |> List.map (fun cell ->
                match cell with
                | { Number = x } when x = n -> { cell with Marked = true }
                | _ -> cell)

        let rows = cells |> List.chunkBySize 5
        let colums = rows |> List.transpose

        let isAllMarked cells =
            cells |> List.forall (fun cell -> cell.Marked)

        let bingo =
            rows |> List.exists isAllMarked || colums |> List.exists isAllMarked

        let score =
            match bingo with
            | false -> None
            | true ->
                let sumOfUnmarked =
                    cells
                    |> List.sumBy (fun cell ->
                        if cell.Marked then 0 else cell.Number)

                n * sumOfUnmarked |> Some

        { Cells = cells; Score = score }

type Game =
    { Numbers: int list; Boards: Board seq }

module Game =
    let ofLines lines =
        let numbers =
            lines
            |> List.head
            |> String.split ','
            |> Array.toList
            |> List.map int

        let boards =
            lines
            |> List.tail
            |> List.chunkBySize 6
            |> List.map (fun chunk -> chunk |> List.tail |> Board.ofLines)

        { Numbers = numbers; Boards = boards }

    let rec play game =
        seq {
            let n = game.Numbers |> List.head
            let boards = game.Boards |> Seq.map (Board.play n)

            let winners =
                boards |> Seq.filter (fun board -> board.Score |> Option.isSome)

            yield! winners

            let loosers =
                boards |> Seq.filter (fun board -> board.Score |> Option.isNone)

            let numbers = game.Numbers |> List.tail

            if Seq.isEmpty loosers || List.isEmpty numbers then
                yield! Seq.empty
            else
                yield! play { Numbers = numbers; Boards = loosers }
        }

let input = "04" |> readInputLines |> Seq.toList

let job1 () =
    input |> Game.ofLines |> Game.play |> Seq.head |> Board.score |> string

let job2 () =
    input |> Game.ofLines |> Game.play |> Seq.last |> Board.score |> string
