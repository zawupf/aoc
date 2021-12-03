module Day03

open Utils

let mostCharAt (lines: string list) (index: int) =
    let sign =
        lines
        |> List.sumBy (fun line -> if line.[index] = '1' then 1 else -1)
        |> System.Math.Sign

    match sign with
    | 1 -> '1'
    | -1 -> '0'
    | _ -> '1'

let leastCharAt (lines: string list) (index: int) =
    let sign =
        lines
        |> List.sumBy (fun line -> if line.[index] = '1' then 1 else -1)
        |> System.Math.Sign

    match sign with
    | 1 -> '0'
    | -1 -> '1'
    | _ -> '0'

let bitstringToInt bits = System.Convert.ToInt32(bits, 2)

let joinChars (chars: char list) = System.String.Join("", chars)

type PowerConsumtionRate = { Gamma: int; Epsilon: int }

module PowerConsumtionRate =
    let result (rate: PowerConsumtionRate) = rate.Gamma * rate.Epsilon

    let ofReport (report: string list) =
        let numChars = report.Head.Length

        let gamma =
            [ 0 .. numChars - 1 ]
            |> List.map (mostCharAt report)
            |> joinChars
            |> bitstringToInt

        let epsilon = (~~~gamma) &&& ((1 <<< numChars) - 1)

        { Gamma = gamma; Epsilon = epsilon }

type LifeSupportRate =
    { OxygenGenerator: int
      CO2Scrubber: int }

module LifeSupportRate =
    let result (rate: LifeSupportRate) = rate.OxygenGenerator * rate.CO2Scrubber

    let private _filter func lines =
        let rec _filter_rec func index (lines: string list) =
            match lines |> List.tryExactlyOne with
            | Some line -> line
            | None ->
                let useChar = func lines index

                lines
                |> List.filter (fun line -> line.[index] = useChar)
                |> _filter_rec func (index + 1)

        _filter_rec func 0 lines

    let ofReport (report: string list) =
        { OxygenGenerator = report |> _filter mostCharAt |> bitstringToInt
          CO2Scrubber = report |> _filter leastCharAt |> bitstringToInt }


let report = "03" |> readInputLines |> Seq.toList

let job1 () =
    report
    |> PowerConsumtionRate.ofReport
    |> PowerConsumtionRate.result
    |> string

let job2 () =
    report
    |> LifeSupportRate.ofReport
    |> LifeSupportRate.result
    |> string
