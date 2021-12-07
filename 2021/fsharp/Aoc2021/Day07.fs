module Day07

open Utils

let findMinFuelPosition distanceToFuel positions =
    let positionCount =
        positions
        |> List.countBy (fun position -> position)

    let fuel (position: int) =
        positionCount
        |> List.sumBy (fun (pos, count) ->
            count
            * (System.Math.Abs(position - pos) |> distanceToFuel))

    let min = positionCount |> List.map fst |> Seq.min
    let max = positionCount |> List.map fst |> Seq.max

    [ min .. max ]
    |> List.map (fun pos -> pos, pos |> fuel)
    |> List.minBy snd

let findBestPosition = findMinFuelPosition (fun d -> d)

let findBestCrabPosition =
    findMinFuelPosition (fun d -> d * (d + 1) / 2)

let input =
    "07" |> readInputText |> String.parseInts ','

let job1 () =
    input |> findBestPosition |> snd |> string

let job2 () =
    input |> findBestCrabPosition |> snd |> string
