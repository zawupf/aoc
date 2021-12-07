module Day07

open Utils

let findPositionWithMinFuel distanceToFuel positions =
    let positionCount =
        positions
        |> List.countBy (fun position -> position)

    let fuel (position: int) =
        positionCount
        |> List.sumBy (fun (pos, count) ->
            count
            * (System.Math.Abs(position - pos) |> distanceToFuel))

    let min = positionCount |> List.minBy fst |> fst
    let max = positionCount |> List.maxBy fst |> fst

    [ min .. max ]
    |> List.map (fun pos -> pos, pos |> fuel)
    |> List.minBy snd

let findBestPositionWithMinFuel = findPositionWithMinFuel (fun d -> d)

let findBestCrabPositionWithMinFuel =
    findPositionWithMinFuel (fun d -> d * (d + 1) / 2)

let input =
    "07" |> readInputText |> String.parseInts ','

let job1 () =
    input
    |> findBestPositionWithMinFuel
    |> snd
    |> string

let job2 () =
    input
    |> findBestCrabPositionWithMinFuel
    |> snd
    |> string
