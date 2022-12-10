module Day10

open Utils

let cycles input =
    seq {
        let mutable x = 1

        for line in input do
            match line with
            | Regex @"^noop$" [] -> yield x
            | Regex @"^addx (-?\d+)$" [ Int dx ] ->
                yield x
                yield x
                x <- x + dx
            | _ -> failwith $"Invalid instruction: %s{line}"
    }

let signalStrengthSum cycles =
    let signals = cycles |> Seq.toArray

    [ 20; 60; 100; 140; 180; 220 ]
    |> Seq.map (fun i -> signals[i - 1] * i)
    |> Seq.sum

let render cycles =
    cycles
    |> Seq.indexed
    |> Seq.map (fun (i, signal) ->
        let index = i % 40
        let spriteLeft, spriteRight = (signal - 1, signal + 1)

        if index >= spriteLeft && index <= spriteRight then
            '#'
        else
            '.')
    |> Seq.chunkBySize 40
    |> Seq.map String.ofChars
    |> Seq.toArray

let input = readInputLines "10"

let job1 () =
    input |> cycles |> signalStrengthSum |> string

let job2 () =
    sprintf "\n%s" (input |> cycles |> render |> String.join "\n")
