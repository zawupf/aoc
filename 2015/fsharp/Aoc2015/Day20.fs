module Day20

open Utils

let presentCount1 () =
    Seq.unfold
        (fun house ->
            let singleSum = (house |> Math.divisors |> List.sum)
            Some(singleSum * 10, house + 1))
        1

let presentCount2 () =
    Seq.unfold
        (fun house ->
            let singleSum =
                (house
                 |> Math.divisors
                 |> List.filter (fun d -> d > (house - 1) / 50)
                 |> List.sum)

            Some(singleSum * 11, house + 1))
        1

let findFirstHouseWithAtLeast counting limit =
    1 + (counting () |> Seq.findIndex (fun n -> n >= limit))

let input = readInputText "20" |> int

let job1 () =
    input |> findFirstHouseWithAtLeast presentCount1 |> string

let job2 () =
    input |> findFirstHouseWithAtLeast presentCount2 |> string
