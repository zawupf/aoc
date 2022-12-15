module Day15

open Utils

type Pos = int * int
let distance (x, y) (x', y') = abs (x - x') + abs (y - y')

type Signal = { Sensor: Pos; Beacon: Pos }

module Signal =
    let parse line =
        let rx =
            @"^Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)$"

        match line with
        | Regex rx [ Int sx; Int sy; Int bx; Int by ] ->
            { Sensor = (sx, sy); Beacon = (bx, by) }
        | _ -> failwithf "Invalid line: %s" line

    let parseMany lines = lines |> Seq.map parse |> Seq.toArray

    let countSenderAndBeaconsInRow y signals =
        signals
        |> Array.fold
            (fun
                xs
                { Sensor = (x', y')
                  Beacon = (x'', y'') } ->
                xs
                |> if y = y' then Set.add x' else id
                |> if y = y'' then Set.add x'' else id)
            Set.empty
        |> Set.toList

    let countNonEmptyInRow y signals =
        let addFromSignal ranges s =
            let { Sensor = (x', y') } = s
            let d = distance s.Sensor s.Beacon
            let dy = abs (y - y')

            match d - dy with
            | dx when dx >= 0 -> (Range.init (x' - dx) (x' + dx)) :: ranges
            | _ -> ranges

        let ranges = signals |> Array.fold addFromSignal [] |> Range.combine

        let nonEmptyCount =
            signals
            |> countSenderAndBeaconsInRow y
            |> List.filter (fun x -> ranges |> List.exists (Range.contains x))
            |> List.length
            |> int64

        let emptyCount = ranges |> List.map (Range.longLength) |> List.sum

        max 0L (emptyCount - nonEmptyCount)

    let tryFindEmptyInRow y signals =
        let addFromSignal ranges s =
            let { Sensor = (x', y') } = s
            let d = distance s.Sensor s.Beacon
            let dy = abs (y - y')

            match d - dy with
            | dx when dx >= 0 -> (Range.init (x' - dx) (x' + dx)) :: ranges
            | _ -> ranges

        let ranges =
            signals
            |> Array.fold addFromSignal []
            |> Range.combine
            |> List.map (fun (a, b) ->
                let a' = max 0 a
                let b' = min 4000000 b
                a', b')
            |> List.sort

        let length = ranges |> List.map (Range.longLength) |> List.sum

        if length = 4000001L then
            None
        else
            let x =
                match ranges with
                | [ r1; _ ] -> (r1 |> snd) + 1
                | [ r ] -> if r |> fst = 0 then 4000000 else 0
                | _ -> failwithf "Impossible ranges: %A" ranges

            Some x


    let findTuningFrequency signals =
        seq {
            for y in 0..4000000 do
                match tryFindEmptyInRow y signals with
                | Some x -> yield (int64 x * 4000000L + int64 y)
                | None -> ()
        }
        |> Seq.head

let input = readInputLines "15"

let job1 () =
    input |> Signal.parseMany |> Signal.countNonEmptyInRow 2000000 |> string

let job2 () =
    input |> Signal.parseMany |> Signal.findTuningFrequency |> string
