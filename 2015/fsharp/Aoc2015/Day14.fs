module Day14

open Utils

type Reindeer =
    { Name: string
      Speed: int
      FlyTime: int
      RestTime: int }

    member this.FullTime = this.FlyTime + this.RestTime

module Reindeer =
    let distanceAfter seconds (reindeer: Reindeer) =
        let fulltimeCount = seconds / reindeer.FullTime
        let remainingTime = seconds % reindeer.FullTime

        reindeer.Speed
        * (fulltimeCount * reindeer.FlyTime
           + (min remainingTime reindeer.FlyTime))

    let maxScoreAfter seconds (reindeers: Reindeer list) =
        let race = reindeers |> List.map (fun reindeer -> reindeer, 0)

        [ 1..seconds ]
        |> List.fold
            (fun race seconds ->
                let distances =
                    race
                    |> List.map (fun (reindeer, score) ->
                        (reindeer |> distanceAfter seconds), (reindeer, score))

                let maxDistance = distances |> List.maxBy fst |> fst

                distances
                |> List.map (fun (distance, (reindeer, score)) ->
                    if distance = maxDistance then
                        reindeer, score + 1
                    else
                        reindeer, score))
            race
        |> List.maxBy snd
        |> snd

    let parse input =
        let pattern =
            @"^(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds\.$"

        match input with
        | Regex pattern [ name; Int speed; Int flyTime; Int restTime ] ->
            { Name = name
              Speed = speed
              FlyTime = flyTime
              RestTime = restTime }
        | _ -> failwithf "Invalid input: %s" input

let input = readInputLines "14" |> Seq.toList |> List.map Reindeer.parse

let job1 () =
    input |> List.map (Reindeer.distanceAfter 2503) |> List.max |> string

let job2 () =
    input |> Reindeer.maxScoreAfter 2503 |> string
