module Day14

open Utils

type Mode =
    | Part1
    | Part2

type Structure =
    | Air
    | Rock
    | Sand

let toPosition =
    function
    | Regex @"^(-?\d+),(-?\d+)$" [ Int x; Int y ] -> x, y
    | input -> failwith $"Invalid position: %s{input}"

let collectRocks rocks ((x1, y1), (x2, y2)) =
    let xmin, xmax = min x1 x2, max x1 x2
    let ymin, ymax = min y1 y2, max y1 y2

    [
        for x in xmin..xmax do
            for y in ymin..ymax -> x, y
    ]
    |> List.fold (fun rocks pos -> pos :: rocks) rocks

let dropStart = (500, 0)
let dropStartX, dropStartY = dropStart

let parse mode lines =
    let rocks =
        lines
        |> Seq.map (fun (line: string) ->
            line.Split(" -> ")
            |> Array.map toPosition
            |> Array.pairwise
            |> Array.fold collectRocks [])
        |> List.concat

    let xmin, xmax, ymin, ymax =
        dropStart :: rocks
        |> List.fold
            (fun (xmin, xmax, ymin, ymax) (x, y) ->
                let xmin', xmax' = min xmin x, max xmax x
                let ymin', ymax' = min ymin y, max ymax y
                xmin', xmax', ymin', ymax')
            (System.Int32.MaxValue,
             System.Int32.MinValue,
             System.Int32.MaxValue,
             System.Int32.MinValue)

    let ymax, xmin, xmax, rocks =
        match mode with
        | Part1 -> ymax, xmin, xmax, rocks
        | Part2 ->
            let ymax = ymax + 2
            let xmin = min xmin (dropStartX - 2 * ymax)
            let xmax = max xmax (dropStartX + 2 * ymax)
            let rocks = collectRocks rocks ((xmin, ymax), (xmax, ymax))
            ymax, xmin, xmax, rocks

    let grid =
        Array2D.createBased xmin ymin (xmax - xmin + 1) (ymax - ymin + 1) Air

    for (x, y) in rocks do
        grid[x, y] <- Rock

    grid

let isValid (x, y) grid =
    let xmin = Array2D.base1 grid
    let xmax = xmin + Array2D.length1 grid - 1
    let ymin = Array2D.base2 grid
    let ymax = ymin + Array2D.length2 grid - 1
    x >= xmin && x <= xmax && y >= ymin && y <= ymax

let simulate grid =
    let rec loop n prevs =
        match prevs with
        | [] -> n
        | ((x, y), dirs) :: prevs ->
            match isValid (x, y) grid with
            | false -> n
            | true ->
                match grid[x, y] with
                | Sand
                | Rock -> loop n prevs
                | Air ->
                    match dirs with
                    | [] ->
                        grid[x, y] <- Sand
                        loop (n + 1) prevs
                    | d :: dirs ->
                        loop
                            n
                            (((x + d, y + 1), [ 0; -1; 1 ])
                             :: ((x, y), dirs)
                             :: prevs)

    loop 0 [ dropStart, [ 0; -1; 1 ] ]

let input = readInputLines "14"

let job1 () =
    input |> parse Part1 |> simulate |> string

let job2 () =
    input |> parse Part2 |> simulate |> string
