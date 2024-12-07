#load "Utils.fsx"

type Equation = int64 * int64 list

type Operator =
    | Add
    | Multiply
    | Concatenate

let apply a b operator =
    match operator with
    | Add -> a + b
    | Multiply -> a * b
    | Concatenate -> $"{a}{b}" |> int64

let canSolveWith (operators: Operator list) (equation: Equation) : bool =
    let result, numbers = equation

    let rec canSolve (stack: int64 list list) : bool =
        match stack with
        | [] -> false
        | numbers :: stack ->
            match numbers with
            | [ result' ] when result' = result -> true
            | [ _ ] -> canSolve stack
            | a :: b :: numbers ->
                canSolve (
                    operators
                    |> List.fold
                        (fun stack operator ->
                            let value = apply a b operator

                            if value > result then
                                stack
                            else
                                (value :: numbers) :: stack)
                        stack
                )
            | _ -> failwith "Invalid stack"

    canSolve [ numbers ]

let parseEquation line : Equation =
    match line |> Utils.String.split ": " with
    | [| resultChunk; numbersChunk |] ->
        resultChunk |> int64,
        numbersChunk |> Utils.String.parseInt64s " " |> List.ofArray
    | _ -> failwith "Invalid equation"

let part1 input =
    input
    |> Array.map parseEquation
    |> Array.sumBy (fun equation ->
        if canSolveWith [ Add; Multiply ] equation then
            equation |> fst
        else
            0)

let part2 input =
    input
    |> Array.map parseEquation
    |> Array.sumBy (fun equation ->
        if canSolveWith [ Add; Multiply; Concatenate ] equation then
            equation |> fst
        else
            0)

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 1260333054159L solution1
Utils.Test.run "Part 2" 162042343638683L solution2
