module Day16

open Utils

let randomBits length count chars =
    let invert chars =
        chars
        |> Seq.rev
        |> Seq.map (function
            | '0' -> '1'
            | _ -> '0')

    let rec loop count chars =
        if length <= count then
            chars |> Seq.take length
        else
            loop
                (count * 2 + 1)
                (seq {
                    yield! chars
                    yield '0'
                    yield! chars |> invert
                })

    loop count chars

let checksum count chars =
    let calc chars =
        chars
        |> Seq.chunkBySize 2
        |> Seq.map (function
            | [| a; b |] when a = b -> '1'
            | _ -> '0')

    let rec loop count chars =
        match count with
        | Odd -> chars
        | Even -> loop (count / 2) (chars |> calc)

    loop (count / 2) (chars |> calc)

let input = readInputText "16"

let job1 () =
    input |> randomBits 272 input.Length |> checksum 272 |> String.ofChars

let job2 () =
    input
    |> randomBits 35651584 input.Length
    |> checksum 35651584
    |> String.ofChars
