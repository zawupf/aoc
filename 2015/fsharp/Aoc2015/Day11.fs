module Day11

open Utils

let encode password = password |> String.toCharArray

let decode (pw: char[]) = pw |> System.String |> string

let inc c = c + (1 |> char)

let hasStraight pw =
    pw
    |> Seq.windowed 3
    |> Seq.exists (function
        | [| a; b; c |] -> inc a = b && inc b = c
        | _ -> false)

let hasPairs pw =
    pw
    |> Seq.pairwise
    |> Seq.filter (fun (a, b) -> a = b)
    |> Seq.distinct
    |> Seq.length
    |> (fun l -> l >= 2)

let isInvalidChar c = c = 'i' || c = 'o' || c = 'l'

let hasInvalidChar pw =
    pw |> Seq.tryFind isInvalidChar |> Option.isSome

let isValid pw =
    hasStraight pw && hasPairs pw && (hasInvalidChar pw |> not)

let increment (pw: char[]) =
    let rec loop i (pw: char[]) =
        if i < 0 then
            ()
        else
            match pw.[i] |> inc with
            | c when isInvalidChar c -> pw.[i] <- c |> inc
            | c when c <= 'z' -> pw.[i] <- c
            | _ ->
                pw.[i] <- 'a'
                loop (i - 1) pw

    match pw |> Seq.tryFindIndex isInvalidChar with
    | Some i ->
        pw.[i] <- pw.[i] |> inc

        for j in (i + 1) .. (Array.length pw - 1) do
            pw.[j] <- 'a'
    | None -> loop (Array.length pw - 1) pw

    pw

let passwords init =
    Seq.unfold
        (fun pw ->
            increment pw |> ignore
            Some(pw, pw))
        init

let nextValidPassword current =
    current |> encode |> passwords |> Seq.filter isValid |> Seq.head |> decode

let input = readInputText "11"

let job1 () = input |> nextValidPassword

let job2 () =
    input |> nextValidPassword |> nextValidPassword
