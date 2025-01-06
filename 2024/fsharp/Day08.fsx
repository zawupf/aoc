#load "Utils.fsx"

type Pos = int * int

type Grid = { width: int; height: int; antennas: (char * Pos[])[] }

let isInside grid (x, y) = x >= 0 && x < grid.width && y >= 0 && y < grid.height

let deltas (x1, y1) (x2, y2) = x2 - x1, y2 - y1

let antinodesInLine grid pos (dx, dy) =
    Seq.unfold
        (fun (x, y) ->
            if isInside grid (x, y) then
                Some((x, y), (x + dx, y + dy))
            else
                None)
        pos

let pureAntinodes grid (a, b) =
    let dx, dy = deltas a b

    Seq.concat [
        antinodesInLine grid a (-dx, -dy) |> Seq.skip 1 |> Seq.truncate 1
        antinodesInLine grid b (dx, dy) |> Seq.skip 1 |> Seq.truncate 1
    ]

let harmonicAntinodes grid (a, b) =
    let dx, dy = deltas a b

    Seq.concat [
        antinodesInLine grid a (-dx, -dy)
        antinodesInLine grid b (dx, dy)
    ]

let pairs (positions: Pos[]) =
    seq {
        for i in 0 .. positions.Length - 2 do
            for j in i + 1 .. positions.Length - 1 do
                yield positions.[i], positions.[j]
    }

let countAntinodes findAntinodes grid =
    grid.antennas
    |> Seq.collect (fun (_, positions) ->
        positions |> pairs |> Seq.collect (findAntinodes grid))
    |> Seq.distinct
    |> Seq.length

let parse (lines: string[]) =
    let width = lines.[0].Length
    let height = lines.Length

    let antennas =
        lines
        |> Array.mapi (fun y line ->
            line
            |> Seq.mapi (fun x c -> c, (x, y))
            |> Seq.filter (fun (c, _) -> c <> '.')
            |> Seq.toArray)
        |> Array.concat
        |> Array.groupBy fst
        |> Array.map (fun (c, pos) -> c, pos |> Array.map snd)

    { width = width; height = height; antennas = antennas }

let part1 input = input |> parse |> countAntinodes pureAntinodes

let part2 input = input |> parse |> countAntinodes harmonicAntinodes

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 318 solution1
Utils.Test.run "Part 2" 1126 solution2

#load "_benchmark.fsx"
