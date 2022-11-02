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

    let maxScoreAfter seconds (reindeers: Reindeer seq) =
        let race = reindeers |> Seq.map (fun reindeer -> reindeer, 0)

        { 1..seconds }
        |> Seq.fold
            (fun race seconds ->
                let distances =
                    race
                    |> Seq.map (fun (reindeer, score) ->
                        (reindeer |> distanceAfter seconds), (reindeer, score))

                let maxDistance = distances |> Seq.maxBy fst |> fst

                distances
                |> Seq.map (fun (distance, (reindeer, score)) ->
                    if distance = maxDistance then
                        reindeer, score + 1
                    else
                        reindeer, score))
            race
        |> Seq.maxBy snd
        |> snd

    let parse input =
        match input with
        | Regex @"^(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds\.$"
                [ name; speed; flyTime; restTime ] ->
            { Name = name
              Speed = speed |> int
              FlyTime = flyTime |> int
              RestTime = restTime |> int }
        | _ -> failwithf "Invalid input: %s" input

let input = readInputLines "14"

let job1 () =
    input
    |> Seq.map (Reindeer.parse >> Reindeer.distanceAfter 2503)
    |> Seq.max
    |> string

let job2 () =
    input |> Seq.map Reindeer.parse |> Reindeer.maxScoreAfter 2503 |> string
