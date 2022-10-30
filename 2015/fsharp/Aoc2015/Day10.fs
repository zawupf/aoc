module Day10

open System.Text

open Utils

let lookAndSay (s: string) =
    let look groups c =
        match groups with
        | (n, prevChar) :: groups' when prevChar = c -> (n + 1, c) :: groups'
        | _ -> (1, c) :: groups

    let say (sb: StringBuilder) (n: int, c: char) = sb.Append(n).Append(c)

    s |> Seq.fold look [] |> List.rev |> List.fold say (StringBuilder()) |> string

let play init =
    seq {
        let mutable current = init

        while true do
            yield current
            current <- current |> lookAndSay
    }

let input = readInputText "10"

let job1 () =
    input |> play |> Seq.item 40 |> String.length |> string

let job2 () =
    input |> play |> Seq.item 50 |> String.length |> string
