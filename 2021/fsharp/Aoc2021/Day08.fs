module Day08

open Utils

let countUniqueOutputDigits lines =
    lines
    |> List.map (fun line ->
        line
        |> String.split ' '
        |> Array.skip 11
        |> Array.sumBy (fun digit ->
            match digit.Length with
            | 2
            | 4
            | 3
            | 7 -> 1
            | _ -> 0))
    |> List.sum

let decodeSum lines =
    let digits =
        [ "abcefg"
          "cf"
          "acdeg"
          "acdfg"
          "bcdf"
          "abdfg"
          "abdefg"
          "acf"
          "abcdefg"
          "abcdfg" ]

    let sortSegments (segments: char seq) =
        System.String.Join("", segments |> Seq.sort)

    lines
    |> List.map (fun line ->
        let wrongDigits =
            line |> String.split ' ' |> Array.take 10 |> Array.map sortSegments

        let wrong1 = wrongDigits |> Array.find (fun digit -> digit.Length = 2) |> List.ofSeq

        let wrong4 = wrongDigits |> Array.find (fun digit -> digit.Length = 4) |> List.ofSeq

        let wrong7 = wrongDigits |> Array.find (fun digit -> digit.Length = 3) |> List.ofSeq

        let wrong8 = wrongDigits |> Array.find (fun digit -> digit.Length = 7) |> List.ofSeq

        seq {
            let cfPerms = wrong1 |> permutations

            for cf in cfPerms do
                let c = cf |> Seq.head
                let f = cf |> Seq.skip 1 |> Seq.head

                let bdPerms = wrong4 |> List.except wrong1 |> permutations

                for bd in bdPerms do
                    let b = bd |> Seq.head
                    let d = bd |> Seq.skip 1 |> Seq.head

                    let a = wrong7 |> List.except wrong4 |> List.head

                    let egPerms = wrong8 |> List.except wrong4 |> List.except wrong7 |> permutations

                    for eg in egPerms do
                        let e = eg |> Seq.head
                        let g = eg |> Seq.skip 1 |> Seq.head

                        let map = Map.ofList [ a, 'a'; b, 'b'; c, 'c'; d, 'd'; e, 'e'; f, 'f'; g, 'g' ]

                        let decodedDigits =
                            wrongDigits
                            |> Array.map (fun digit -> digit |> Seq.map (fun c -> map.[c]) |> sortSegments)

                        if digits |> List.forall (fun digit -> decodedDigits |> Array.contains digit) then
                            yield
                                line
                                |> String.split ' '
                                |> Array.skip 11
                                |> Array.map (fun digit ->
                                    let decodedDigit = digit |> Seq.map (fun c -> map.[c]) |> sortSegments

                                    let number = digits |> List.findIndex (fun d -> d = decodedDigit)

                                    number |> string)
                                |> String.join ""
                                |> int
        }
        |> Seq.head

    )
    |> List.sum

let input = "08" |> readInputLines |> Seq.toList

let job1 () =
    input |> countUniqueOutputDigits |> string

let job2 () = input |> decodeSum |> string
