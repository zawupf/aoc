module Day02

open Utils

type Shape =
    | Rock
    | Paper
    | Scissors

module Shape =
    let parse =
        function
        | 'A'
        | 'X' -> Rock
        | 'B'
        | 'Y' -> Paper
        | 'C'
        | 'Z' -> Scissors
        | c -> failwith $"Invalid shape: %c{c}"

type Winner =
    | Me
    | Opponent
    | Draw

module Winner =
    let parse =
        function
        | 'X' -> Opponent
        | 'Y' -> Draw
        | 'Z' -> Me
        | c -> failwith $"Invalid winning result: %c{c}"

let winner me opponent =
    match me, opponent with
    | myShape, oppShape when myShape = oppShape -> Draw
    | Rock, Scissors
    | Scissors, Paper
    | Paper, Rock -> Me
    | _ -> Opponent

let score (myResult, myShape) =
    let scoreShape =
        match myShape with
        | Rock -> 1
        | Paper -> 2
        | Scissors -> 3

    let winningScore =
        match myResult with
        | Opponent -> 0
        | Draw -> 3
        | Me -> 6

    scoreShape + winningScore

let score1 (me, opponent) = score ((winner me opponent), me)

let parse1 input =
    match input with
    | Regex @"^(.) (.)$" [ Char opp; Char me ] ->
        Shape.parse me, Shape.parse opp
    | _ -> failwith $"Invalid input: %s{input}"

let parse2 input =
    let chooseShapeTo winner oppShape =
        match winner, oppShape with
        | Draw, _ -> oppShape
        | Me, Rock -> Paper
        | Me, Paper -> Scissors
        | Me, Scissors -> Rock
        | Opponent, Rock -> Scissors
        | Opponent, Paper -> Rock
        | Opponent, Scissors -> Paper

    match input with
    | Regex @"^(.) (.)$" [ Char opp; Char result ] ->
        let oppShape = Shape.parse opp
        let myResult = Winner.parse result
        let myShape = chooseShapeTo myResult oppShape
        myResult, myShape
    | _ -> failwith $"Invalid input: %s{input}"

let totalScore mapping input = input |> Seq.map mapping |> Seq.sum

let totalScore1 = totalScore (parse1 >> score1)

let totalScore2 = totalScore (parse2 >> score)


let input = readInputLines "02"

let job1 () = input |> totalScore1 |> string

let job2 () = input |> totalScore2 |> string
