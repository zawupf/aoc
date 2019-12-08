module Day01

open System
open System.IO

let fuel mass =
    let fuel = mass / 3 - 2
    if fuel > 0 then fuel
    else 0

let totalFuel mass =
    let rec _totalFuel mass previousFuel =
        let currentFuel = fuel mass
        let currentTotalFuel = previousFuel + currentFuel
        if currentFuel > 0 then _totalFuel currentFuel currentTotalFuel
        else currentTotalFuel
    _totalFuel mass 0

let job1 prefix =
    File.ReadLines(prefix + "inputs/Day01.txt")
    |> Seq.sumBy (int >> fuel)
    |> string

let job2 prefix =
    File.ReadLines(prefix + "inputs/Day01.txt")
    |> Seq.sumBy (int >> totalFuel)
    |> string
