#load "Utils.fsx"

open Utils
open System.Linq

let appendScores (scores: ResizeArray<_>) elf1 elf2 =
    let score1 = scores[elf1]
    let score2 = scores[elf2]
    let sum = score1 + score2

    if sum < 10 then scores.Add sum else scores.AddRange [ sum / 10; sum % 10 ]

    let newElf1 = (elf1 + score1 + 1) % scores.Count
    let newElf2 = (elf2 + score2 + 1) % scores.Count
    newElf1, newElf2

let scoreAfter n =
    let scores = ResizeArray [| 3; 7 |]

    let rec loop elf1 elf2 =
        if scores.Count >= n + 10 then
            scores.Slice(n, 10) |> Seq.map string |> Seq.reduce (+)
        else
            let newElf1, newElf2 = appendScores scores elf1 elf2
            loop newElf1 newElf2

    loop 0 1

let countBefore pattern =
    let pattern = pattern |> Seq.map (fun c -> int c - int '0') |> Seq.toArray

    let scores = ResizeArray [| 3; 7 |]

    let isEqualAt i =
        if scores.Slice(i, pattern.Length).SequenceEqual pattern then
            Some i
        else
            None

    let endsWithPattern () =
        if scores.Count - 1 < pattern.Length then
            None
        else
            isEqualAt (scores.Count - pattern.Length - 1)
            |> Option.orElseWith (fun () ->
                isEqualAt (scores.Count - pattern.Length))

    let rec loop elf1 elf2 =
        match endsWithPattern () with
        | Some n -> n
        | None ->
            let newElf1, newElf2 = appendScores scores elf1 elf2
            loop newElf1 newElf2

    loop 0 1

let part1 input = input |> int |> scoreAfter

let part2 input = input |> countBefore

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Test 1" "5158916779" (fun () -> scoreAfter 9)
Test.run "Test 1" "0124515891" (fun () -> scoreAfter 5)
Test.run "Test 1" "9251071085" (fun () -> scoreAfter 18)
Test.run "Test 1" "5941429882" (fun () -> scoreAfter 2018)

Test.run "Test 2" 9 (fun () -> countBefore "51589")
Test.run "Test 2" 5 (fun () -> countBefore "01245")
Test.run "Test 2" 18 (fun () -> countBefore "92510")
Test.run "Test 2" 2018 (fun () -> countBefore "59414")

Test.run "Part 1" "7116398711" solution1
Test.run "Part 2" 20316365 solution2

#load "_benchmark.fsx"
