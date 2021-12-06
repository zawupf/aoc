module Day06

open Utils

let simulateFishPopulation (fishTimers: int list) =
    let timers =
        fishTimers
        |> List.countBy (fun timer -> timer)
        |> List.fold
            (fun (timers: int64 []) (timer, count) ->
                timers.[timer] <- count
                timers)
            [| 0L; 0L; 0L; 0L; 0L; 0L; 0L; 0L; 0L |]

    let fishCount timers = timers |> Array.sum

    seq {
        while true do
            yield timers |> fishCount

            let motherCount = timers.[0]
            Array.blit timers 1 timers 0 8
            timers.[8] <- motherCount
            timers.[6] <- timers.[6] + motherCount
    }

let input =
    "06"
    |> readInputText
    |> String.split ','
    |> Array.map int
    |> Array.toList

let job1 () =
    input
    |> simulateFishPopulation
    |> Seq.item 80
    |> string

let job2 () =
    input
    |> simulateFishPopulation
    |> Seq.item 256
    |> string
