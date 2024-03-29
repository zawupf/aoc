#load "Utils.fsx"
open System.Text.RegularExpressions

let toNumber =
    function
    | "one" -> 1
    | "two" -> 2
    | "three" -> 3
    | "four" -> 4
    | "five" -> 5
    | "six" -> 6
    | "seven" -> 7
    | "eight" -> 8
    | "nine" -> 9
    | _ as value -> int value

let getCalibrationSum digitPattern lines =
    lines
    |> Seq.map (fun line ->
        let a =
            Regex.Match(line, $".*?({digitPattern})").Groups[1].Value
            |> toNumber

        let b =
            Regex.Match(line, $".*({digitPattern})").Groups[1].Value
            |> toNumber

        int $"{a}{b}")
    |> Seq.sum

let input = Utils.readInputLines "01"

let getDay01_1 () = input |> getCalibrationSum @"\d"

let getDay01_2 () =
    input
    |> getCalibrationSum @"\d|one|two|three|four|five|six|seven|eight|nine"

Utils.Test.run "Part 1" 54990 getDay01_1
Utils.Test.run "Part 2" 54473 getDay01_2
