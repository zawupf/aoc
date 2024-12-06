#load "Utils.fsx"
open System.Collections.Generic

type Field = char
type Row = Field[]
type Grid = Row[]

type Pos = int * int

type Dir =
    | Up
    | Down
    | Left
    | Right

type Guard = Pos * Dir
type State = Grid * Guard

type FinalKind =
    | Exited
    | Caught

type FinalState = State * FinalKind

let parse lines =
    let grid = lines |> Array.map Utils.String.toCharArray

    let guard =
        grid
        |> Seq.mapi (fun y row ->
            (row
             |> Seq.tryFindIndex (fun c -> c = '^')
             |> Option.defaultValue -1,
             y),
            Up)
        |> Seq.find (fun ((x, _), _) -> x >= 0)

    let x, y = fst guard
    grid[y][x] <- 'X'
    grid, guard

let walk (state: State) : FinalState =
    let turnRight dir =
        match dir with
        | Up -> Right
        | Down -> Left
        | Left -> Up
        | Right -> Down

    let rec loop (state: State) (guardStates: HashSet<Guard>) =
        let grid, ((x, y), dir) = state

        let x', y' =
            match dir with
            | Up -> x, y - 1
            | Down -> x, y + 1
            | Left -> x - 1, y
            | Right -> x + 1, y


        let isExited =
            x' < 0 || y' < 0 || y' >= grid.Length || x' >= grid[0].Length

        let guard' = (x', y'), dir
        let isCaught = guardStates.Contains guard'

        match isExited, isCaught with
        | true, true -> failwith "Impossible"
        | true, _ -> state, Exited
        | _, true -> state, Caught
        | false, false ->
            guardStates.Add guard' |> ignore

            match grid[y'][x'] with
            | '#' -> loop (grid, ((x, y), turnRight dir)) guardStates
            | _ ->
                grid[y'][x'] <- 'X'
                loop (grid, guard') guardStates


    loop state (HashSet<Guard>())

let visitedCount grid =
    grid
    |> Array.sumBy (fun row ->
        row |> Array.sumBy (fun c -> if c = 'X' then 1 else 0))

let part1 input =
    input |> parse |> walk |> fst |> fst |> visitedCount

let part2 input =
    let originalGrid, startPos = parse input

    let escapeGrid =
        (originalGrid |> Array.map Array.copy, startPos) |> walk |> fst |> fst

    let (sx, sy), _ = startPos
    escapeGrid[sy][sx] <- '.'

    seq {
        for y in 0 .. originalGrid.Length - 1 do
            for x in 0 .. originalGrid[0].Length - 1 do
                if escapeGrid[y][x] = 'X' && originalGrid[y][x] = '.' then
                    yield x, y
    }
    |> Seq.sumBy (fun (x, y) ->
        let grid = originalGrid |> Array.map Array.copy
        grid[y][x] <- '#'
        if (grid, startPos) |> walk |> snd = Caught then 1 else 0)

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    """
....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...
"""
    |> Utils.String.toLines

Utils.Test.run "Test 1" 41 (fun () -> part1 testInput)
Utils.Test.run "Test 2" 6 (fun () -> part2 testInput)

Utils.Test.run "Part 1" 5177 solution1
Utils.Test.run "Part 2" 1686 solution2
