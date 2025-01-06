#load "Utils.fsx"

let XMAS = "XMAS" |> Utils.String.toCharArray |> Array.indexed
let MAS = XMAS |> Array.skip 1

type Row = char[]
type Rows = Row[]

module Rows =
    let item x y rows =
        rows
        |> Array.tryItem y
        |> Option.bind (fun row -> row |> Array.tryItem x)
        |> Option.defaultValue ' '

let count_XMAS (rows: Rows) x y =
    match rows[y][x] with
    | 'X' ->
        MAS
        |> Array.fold
            (fun directions (i, c) ->
                directions
                |> Array.filter (fun (dx, dy) ->
                    let nx, ny = x + dx * i, y + dy * i
                    c = Rows.item nx ny rows))
            [| 1, 0; -1, 0; 0, 1; 0, -1; 1, 1; -1, -1; 1, -1; -1, 1 |]
        |> Array.length
    | _ -> 0

let count_MAS_cross rows x y =
    let item x y = rows |> Rows.item x y
    let diag1 = item (x + 1) (y + 1), item (x - 1) (y - 1)
    let diag2 = item (x + 1) (y - 1), item (x - 1) (y + 1)

    let isMS a b = a = 'M' && b = 'S' || a = 'S' && b = 'M'

    match item x y, diag1 ||> isMS, diag2 ||> isMS with
    | 'A', true, true -> 1
    | _ -> 0

let count counter rows =
    rows
    |> Array.indexed
    |> Array.sumBy (fun (y, row) ->
        row |> Array.indexed |> Array.sumBy (fun (x, _) -> counter rows x y))

let part1 input = input |> count count_XMAS

let part2 input = input |> count count_MAS_cross

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day |> Array.map Utils.String.toCharArray
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 2517 solution1
Utils.Test.run "Part 2" 1960 solution2

#load "_benchmark.fsx"
