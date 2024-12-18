#load "Utils.fsx"

type Field = int
type Grid = Field[,]
type Pos = int * int
type State = { Grid: Grid; Bytes: Pos[] }

let parseBytes lines =
    lines
    |> Array.map (
        Utils.String.parseInts ','
        >> function
            | [| x; y |] -> x, y
            | _ -> failwith "Invalid input"
    )


let init dropCount bytes =
    let size = (bytes |> Array.map (fun (x, y) -> max x y) |> Array.max) + 1

    let grid = Array2D.init size size (fun _ _ -> System.Int32.MaxValue)

    bytes |> Seq.take dropCount |> Seq.iter (fun (x, y) -> grid[x, y] <- 0)

    { Grid = grid; Bytes = bytes }

let minimumSteps grid =
    let maxIndex = Array2D.length1 grid - 1
    grid[0, 0] <- 0

    let isEnd (x, y) = x = maxIndex && y = maxIndex

    let nextSteps (x, y) =
        seq {
            if x > 0 then
                yield x - 1, y

            if y > 0 then
                yield x, y - 1

            if x < maxIndex then
                yield x + 1, y

            if y < maxIndex then
                yield x, y + 1
        }

    let isValid steps (x, y) = steps < grid[x, y]

    let rec loop stack =
        match stack with
        | [] -> grid[maxIndex, maxIndex]
        | pos :: stack when isEnd pos -> loop stack
        | (x, y) :: stack ->
            let steps = grid[x, y] + 1

            let next =
                nextSteps (x, y)
                |> Seq.filter (isValid steps)
                |> Seq.map (fun (x, y) ->
                    grid[x, y] <- steps
                    x, y)
                |> Seq.toList

            loop (next @ stack)

    loop [ 0, 0 ]

let firstBlockingByte bytes =
    let rec loop okDrops failDrops =
        match failDrops - okDrops with
        | 1 -> okDrops
        | _ ->
            let dropCount = (okDrops + failDrops) / 2
            let state = init dropCount bytes
            let steps = minimumSteps state.Grid

            if steps = System.Int32.MaxValue then
                loop okDrops dropCount
            else
                loop dropCount failDrops

    let x, y = bytes[loop 0 (bytes.Length - 1)]
    sprintf "%d,%d" x y

let part1 input =
    (input |> parseBytes |> init 1024).Grid |> minimumSteps |> string

let part2 input =
    input |> parseBytes |> firstBlockingByte

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" "292" solution1
Utils.Test.run "Part 2" "58,44" solution2
