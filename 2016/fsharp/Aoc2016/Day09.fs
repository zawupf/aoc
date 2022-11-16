module Day09

open Utils

let decompressedLength len data =
    let end' = data |> String.length
    let mutable i = 0
    let mutable length = 0L

    while i < end' do
        match data.[i] with
        | '(' ->
            let j = data.IndexOf(')', i + 1)

            match data.[i + 1 .. j - 1] with
            | Regex @"^(\d+)x(\d+)$" [ Int n; Int repeat ] ->
                let k = j + n
                let s = data.[j + 1 .. k]
                let l = s |> len
                length <- length + l * int64 repeat
                i <- k
            | marker -> failwith $"Invalid marker: %s{marker}"
        | ' ' -> ()
        | _c -> length <- length + 1L

        i <- i + 1

    length

let decompressedLength1 =
    decompressedLength (Seq.sumBy (fun c -> if c = ' ' then 0L else 1L))

let rec decompressedLength2 data =
    decompressedLength decompressedLength2 data

let input = readInputText "09"

let job1 () = input |> decompressedLength1 |> string

let job2 () = input |> decompressedLength2 |> string
