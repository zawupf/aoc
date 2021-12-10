module Day10

open Utils

let isOpening =
    function
    | '('
    | '['
    | '{'
    | '<' -> true
    | _ -> false

let other =
    function
    | '(' -> ')'
    | '[' -> ']'
    | '{' -> '}'
    | '<' -> '>'
    | ')' -> '('
    | ']' -> '['
    | '}' -> '{'
    | '>' -> '<'
    | _ -> 0 |> char

let syntaxErrorScore line =
    line
    |> Seq.fold
        (fun (score, openings) char ->
            match score with
            | 0 ->
                if char |> isOpening then
                    score, char :: openings
                else
                    let expected = openings |> List.head |> other

                    let score =
                        if char = expected then
                            score
                        else
                            match char with
                            | ')' -> 3
                            | ']' -> 57
                            | '}' -> 1197
                            | '>' -> 25137
                            | _ -> -1

                    score, openings |> List.tail
            | _ -> score, openings)
        (0, [])
    |> fst

let completionScore line =
    line
    |> Seq.fold
        (fun openings char ->
            if char |> isOpening then
                char :: openings
            else
                openings |> List.tail)
        List.empty
    |> Seq.map other
    |> Seq.fold
        (fun score char ->
            let points =
                match char with
                | ')' -> 1L
                | ']' -> 2L
                | '}' -> 3L
                | '>' -> 4L
                | _ -> 0L

            score * 5L + points)
        0L

let totalSyntaxErrorScore lines =
    lines |> List.map syntaxErrorScore |> List.sum

let middleCompletion lines =
    let scores =
        lines
        |> List.filter (syntaxErrorScore >> (fun score -> score = 0))
        |> List.map completionScore
        |> List.sort

    scores |> List.item (List.length scores / 2)

let input = "10" |> readInputLines |> Seq.toList

let job1 () =
    input |> totalSyntaxErrorScore |> string

let job2 () = input |> middleCompletion |> string
