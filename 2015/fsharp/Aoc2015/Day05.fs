module Day05

open Utils


[<Literal>]
let vowels = "aeiou"

let isNice input =

    let isVowel (c: char) = vowels.Contains(c)

    let hasAtLeast3Vowels input =
        input
        |> String.toCharArray
        |> Seq.filter isVowel
        |> Seq.tryItem 2
        |> Option.isSome

    let hasAtLeast1Pair input =
        input
        |> String.toCharArray
        |> Seq.pairwise
        |> Seq.exists (fun (a, b) -> a = b)

    let forbiddenStrings = [ "ab"; "cd"; "pq"; "xy" ]

    let hasForbiddenString (input: string) =
        forbiddenStrings
        |> List.exists (fun substring -> input.Contains(substring))

    input |> hasAtLeast3Vowels
    && input |> hasAtLeast1Pair
    && input |> hasForbiddenString |> not

let isReallyNice input =
    let rec hasAtLeast1SpecialPair input =
        let isHeadRepeated input =
            if input |> String.length >= 4 then
                let head = input.Substring(0, 2)
                let tail = input.Substring(2, input.Length - 2)
                tail.Contains(head)
            else
                false

        if isHeadRepeated input then
            true
        else if input.Length > 4 then
            input |> String.substring 1 |> hasAtLeast1SpecialPair
        else
            false

    let hasAtLeast1SpecialLetter input =
        input
        |> String.toCharArray
        |> Seq.windowed 3
        |> Seq.exists (function
            | [| a; _; b |] when a = b -> true
            | _ -> false)

    input |> hasAtLeast1SpecialPair && input |> hasAtLeast1SpecialLetter

let input = readInputLines "05"

let job1 () =
    input |> Seq.sumBy (fun line -> if line |> isNice then 1 else 0) |> string

let job2 () =
    input
    |> Seq.sumBy (fun line -> if line |> isReallyNice then 1 else 0)
    |> string
