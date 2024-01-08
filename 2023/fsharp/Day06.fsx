#load "Utils.fsx"

open System.Text.RegularExpressions

let rec findSwitchTime a b time predicate =
    let i = (a + b) / 2L
    let a, b = if predicate ((time - i) * i) then a, i else i, b
    if b - a > 1L then findSwitchTime a b time predicate else b

let winCount time dist =
    let first = findSwitchTime 0 (time / 2L) time (fun d -> d > dist)
    let last = findSwitchTime (time / 2L) time time (fun d -> d <= dist)
    last - first

[<Literal>]
let rx = @"\d+"

let part1 input = //
    let times =
        Regex.Matches(input |> Array.item 0, rx)
        |> Seq.map (_.Value >> int64)
        |> Seq.toArray

    let distances =
        Regex.Matches(input |> Array.item 1, rx)
        |> Seq.map (_.Value >> int64)
        |> Seq.toArray

    (times, distances) ||> Array.map2 winCount |> Array.reduce (*)

let part2 input = //
    let time =
        Regex.Matches(input |> Array.item 0, rx)
        |> Seq.map _.Value
        |> Utils.String.join ""
        |> int64

    let distance =
        Regex.Matches(input |> Array.item 1, rx)
        |> Seq.map _.Value
        |> Utils.String.join ""
        |> int64

    (time, distance) ||> winCount



let testInput = [|
    """
Time:      7  15   30
Distance:  9  40  200
"""
    |> Utils.String.toLines
|]

Utils.Test.run "Test part 1" 288L (fun () -> part1 testInput[0])
Utils.Test.run "Test part 2" 71503L (fun () -> part2 testInput[0])



let input = Utils.readInputLines "06"

let getDay06_1 () = part1 input

let getDay06_2 () = part2 input

Utils.Test.run "Part 1" 861300L getDay06_1
Utils.Test.run "Part 2" 28101347L getDay06_2
