module Day02

open System.Text.RegularExpressions
open Utils

type Policy = { Letter: char; Min: int; Max: int }

type PasswordData = { Password: string; Policy: Policy }

let rx = Regex(@"^(\d+)-(\d+) (.): (.+)$")

let (|PasswordData|_|) input =
    let m = rx.Match(input)
    if (m.Success) then
        let g = m.Groups
        let min = g.[1].Value |> int
        let max = g.[2].Value |> int
        let letter = g.[3].Value.[0]
        let password = g.[4].Value
        Some
            { Password = password
              Policy =
                  { Letter = letter
                    Min = min
                    Max = max } }
    else
        None

let parse line =
    match line with
    | PasswordData pw -> pw
    | _ -> failwith "Invalid input data"

let isValid { Password = password; Policy = { Letter = letter; Min = min; Max = max } } =
    let inRange count = count >= min && count <= max
    password
    |> Seq.filter (fun c -> c = letter)
    |> Seq.length
    |> inRange

let isValid2 { Password = password; Policy = { Letter = letter; Min = min; Max = max } } =
    let c1, c2 = password.[min - 1], password.[max - 1]
    c1 <> c2 && (c1 = letter || c2 = letter)

let job1 () =
    readInputLines "02"
    |> Seq.map parse
    |> Seq.filter isValid
    |> Seq.length
    |> string

let job2 () =
    readInputLines "02"
    |> Seq.map parse
    |> Seq.filter isValid2
    |> Seq.length
    |> string
