#load "Utils.fsx"
open Utils

type Area = { MinX: int; MinY: int; MaxX: int; MaxY: int }

let area locations = {
    MinX = locations |> Array.map fst |> Array.min
    MinY = locations |> Array.map snd |> Array.min
    MaxX = locations |> Array.map fst |> Array.max
    MaxY = locations |> Array.map snd |> Array.max
}

let outlineLocations area =
    seq {
        yield! [ for x in area.MinX .. area.MaxX -> x, area.MinY ]
        yield! [ for x in area.MinX .. area.MaxX -> x, area.MaxY ]
        yield! [ for y in area.MinY + 1 .. area.MaxY - 1 -> area.MinX, y ]
        yield! [ for y in area.MinY + 1 .. area.MaxY - 1 -> area.MaxX, y ]
    }

let innerLocations area =
    seq {
        for x in area.MinX + 1 .. area.MaxX - 1 do
            for y in area.MinY + 1 .. area.MaxY - 1 do
                yield x, y
    }

let distance (x1, y1) (x2, y2) = abs (x1 - x2) + abs (y1 - y2)

let closestLocation locations location =
    locations
    |> Array.map (fun l -> {| Location = l; Distance = distance l location |})
    |> Array.groupBy _.Distance
    |> Array.minBy fst
    |> snd
    |> Array.tryExactlyOne
    |> Option.map _.Location

let totalDistance locations location =
    locations |> Array.sumBy (distance location)

let parseLocations lines =
    lines
    |> Array.map (fun line ->
        match line |> String.parseInts ", " with
        | [| x; y |] -> x, y
        | _ -> failwithf "Invalid location: %s" line)

let part1 input =
    let locations = input |> parseLocations
    let area = locations |> area

    let exclude =
        area
        |> outlineLocations
        |> Seq.map (closestLocation locations)
        |> Seq.distinct
        |> Seq.choose id
        |> Set.ofSeq

    area
    |> innerLocations
    |> Seq.map (closestLocation locations)
    |> Seq.choose id
    |> Seq.countBy id
    |> Seq.filter (fun (l, _) -> not (Set.contains l exclude))
    |> Seq.map snd
    |> Seq.max

let part2 input =
    let locations = input |> parseLocations

    locations
    |> area
    |> innerLocations
    |> Seq.map (totalDistance locations)
    |> Seq.sumBy ((>) 10000 >> Bool.toInt)

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Part 1" 4215 solution1
Test.run "Part 2" 40376 solution2

#load "_benchmark.fsx"
