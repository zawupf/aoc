module Day02

open Utils

type Submarine = {
    HorizontalPosition: int
    Depth: int
    Aim: int
}

type Command = { Direction: string; Value: int }

let parseCommands (lines: string seq) =
    lines
    |> Seq.map (fun line ->
        let chunks = line.Split(' ', 2)

        {
            Direction = chunks.[0]
            Value = (chunks.[1] |> int)
        })
    |> Seq.toList

let moveFirstTry submarine commands =
    commands
    |> List.fold
        (fun submarine command ->
            let {
                    HorizontalPosition = horizontalPosition
                    Depth = depth
                } =
                submarine

            let { Direction = direction; Value = value } = command

            match direction with
            | "forward" -> {
                submarine with
                    HorizontalPosition = horizontalPosition + value
              }
            | "down" -> { submarine with Depth = depth + value }
            | "up" -> { submarine with Depth = depth - value }
            | _ -> failwith "Invalid command direction")
        submarine

let moveSecondTry submarine commands =
    commands
    |> List.fold
        (fun submarine command ->
            let {
                    HorizontalPosition = horizontalPosition
                    Depth = depth
                    Aim = aim
                } =
                submarine

            let { Direction = direction; Value = value } = command

            match direction with
            | "forward" -> {
                submarine with
                    HorizontalPosition = horizontalPosition + value
                    Depth = depth + (aim * value)
              }
            | "down" -> { submarine with Aim = aim + value }
            | "up" -> { submarine with Aim = aim - value }
            | _ -> failwith "Invalid command direction")
        submarine


let input = readInputLines "02" |> parseCommands

let result (submarine: Submarine) =
    submarine.HorizontalPosition * submarine.Depth

let job1 () =
    input
    |> moveFirstTry {
        HorizontalPosition = 0
        Depth = 0
        Aim = 0
    }
    |> result
    |> string

let job2 () =
    input
    |> moveSecondTry {
        HorizontalPosition = 0
        Depth = 0
        Aim = 0
    }
    |> result
    |> string
