module Day03

open Utils

type Move =
    | Up of int
    | Down of int
    | Left of int
    | Right of int

let parseMove (text: string) =
    let move = text.[0]
    let distance = int text.[1..]
    match move with
    | 'U' -> Up distance
    | 'D' -> Down distance
    | 'L' -> Left distance
    | 'R' -> Right distance
    | _ -> failwith "Invalid move"

let parseMoves (line: string) =
    line.Split ','
    |> Seq.ofArray
    |> Seq.map parseMove

let jump (x, y) =
    function
    | Up distance -> x, y + distance
    | Down distance -> x, y - distance
    | Left distance -> x - distance, y
    | Right distance -> x + distance, y

let walk (x, y) =
    function
    | Up distance ->
        seq {
            for d in 1 .. distance -> x, y + d
        }
    | Down distance ->
        seq {
            for d in 1 .. distance -> x, y - d
        }
    | Left distance ->
        seq {
            for d in 1 .. distance -> x - d, y
        }
    | Right distance ->
        seq {
            for d in 1 .. distance -> x + d, y
        }

let walkMany moves =
    seq {
        let mutable p = (0, 0)
        for move in moves do
            let points = walk p move
            p <- jump p move
            for point in points -> point
    }

let countSteps pt points =
    (points
     |> Seq.takeWhile (fun p -> p <> pt)
     |> Seq.length)
    + 1

let minDistance lines =
    lines
    |> Seq.map
        (parseMoves
         >> walkMany
         >> Set.ofSeq)
    |> Set.intersectMany
    |> Seq.map (fun (x, y) -> abs x + abs y)
    |> Seq.min

let minSignalDelay lines =
    let wires = lines |> Seq.map (parseMoves >> walkMany)

    wires
    |> Seq.map Set.ofSeq
    |> Set.intersectMany
    |> Seq.map (fun pt -> wires |> Seq.sumBy (countSteps pt))
    |> Seq.min

let job1() =
    readInputLines "03"
    |> minDistance
    |> string

let job2() =
    readInputLines "03"
    |> minSignalDelay
    |> string
