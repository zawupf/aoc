#load "Utils.fsx"

type Move =
    | Up
    | Down
    | Left
    | Right

type Field =
    | Wall
    | Empty
    | SmallBox
    | BigBoxLeft
    | BigBoxRight
    | Robot

type Pos = int * int
type Grid = Field[][]

type State = {
    Grid: Grid
    Moves: Move[]
    Robot: Pos
}

let parse input =
    match input |> Utils.String.toSections with
    | [| gridChunk; movesChunk |] ->
        let grid =
            gridChunk
            |> Utils.String.split "\n"
            |> Array.map (fun row ->
                row
                |> Seq.map (fun c ->
                    match c with
                    | '#' -> Wall
                    | '.' -> Empty
                    | 'O' -> SmallBox
                    | '@' -> Robot
                    | _ -> failwithf "Invalid field input: %A" c)
                |> Seq.toArray)

        let moves =
            movesChunk
            |> Utils.String.toLines
            |> Utils.String.join ""
            |> Seq.map (fun c ->
                match c with
                | '^' -> Up
                | 'v' -> Down
                | '<' -> Left
                | '>' -> Right
                | _ -> failwithf "Invalid move input: %A" c)
            |> Seq.toArray

        let robot =
            grid
            |> Seq.mapi (fun y row ->
                row
                |> Seq.mapi (fun x field ->
                    match field with
                    | Robot -> Some(x, y)
                    | _ -> None))
            |> Seq.concat
            |> Seq.pick id

        {
            Grid = grid
            Moves = moves
            Robot = robot
        }
    | _ -> failwith "Invalid input"

let scaleUp state =
    let grid =
        state.Grid
        |> Array.map (fun row ->
            row
            |> Array.collect (fun field ->
                match field with
                | Wall -> [| Wall; Wall |]
                | Empty -> [| Empty; Empty |]
                | SmallBox -> [| BigBoxLeft; BigBoxRight |]
                | Robot -> [| Robot; Empty |]
                | BigBoxLeft
                | BigBoxRight -> failwith "Grid already scaled up"))

    let robot = state.Robot |> fun (x, y) -> x * 2, y

    {
        state with
            Grid = grid
            Robot = robot
    }

let nextPos (x, y) move =
    match move with
    | Up -> x, y - 1
    | Down -> x, y + 1
    | Left -> x - 1, y
    | Right -> x + 1, y

let rec canMove (grid: Grid) (x, y) move =
    let nx, ny = nextPos (x, y) move

    match grid[ny][nx] with
    | Empty -> true
    | Wall -> false
    | Robot -> failwith "Robot should not be in the way"
    | SmallBox -> canMove grid (nx, ny) move
    | BigBoxLeft ->
        match move with
        | Left -> canMove grid (nx, ny) move
        | Right -> canMove grid (nx + 1, ny) move
        | Up
        | Down -> canMove grid (nx, ny) move && canMove grid (nx + 1, ny) move
    | BigBoxRight ->
        match move with
        | Left -> canMove grid (nx - 1, ny) move
        | Right -> canMove grid (nx, ny) move
        | Up
        | Down -> canMove grid (nx - 1, ny) move && canMove grid (nx, ny) move

let rec doMove (grid: Grid) (x, y) move =
    let nx, ny = nextPos (x, y) move

    match grid[ny][nx] with
    | Empty -> ()
    | Wall -> failwith "Cannot move into a wall"
    | Robot -> failwith "Robot should not be in the way"
    | SmallBox -> doMove grid (nx, ny) move |> ignore
    | BigBoxLeft ->
        match move with
        | Right ->
            doMove grid (nx + 1, ny) move |> ignore
            doMove grid (nx, ny) move |> ignore
        | _ ->
            doMove grid (nx, ny) move |> ignore
            doMove grid (nx + 1, ny) move |> ignore
    | BigBoxRight ->
        match move with
        | Left ->
            doMove grid (nx - 1, ny) move |> ignore
            doMove grid (nx, ny) move |> ignore
        | _ ->
            doMove grid (nx, ny) move |> ignore
            doMove grid (nx - 1, ny) move |> ignore

    grid[ny][nx] <- grid[y][x]
    grid[y][x] <- Empty

    nx, ny

let tryMove grid pos move =
    if canMove grid pos move then
        Some(doMove grid pos move)
    else
        None

let moveRobot state =
    let robot =
        state.Moves
        |> Array.fold
            (fun pos move ->
                tryMove state.Grid pos move |> Option.defaultValue pos)
            state.Robot

    {
        state with
            Moves = Array.empty
            Robot = robot
    }

let gpsSum state =
    state.Grid
    |> Seq.mapi (fun y row ->
        row
        |> Seq.mapi (fun x field ->
            match field with
            | SmallBox
            | BigBoxLeft -> Some(x, y)
            | _ -> None))
    |> Seq.concat
    |> Seq.sumBy (function
        | Some(x, y) -> x + 100 * y
        | None -> 0)

let part1 input = input |> parse |> moveRobot |> gpsSum

let part2 input =
    input |> parse |> scaleUp |> moveRobot |> gpsSum

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 1563092 solution1
Utils.Test.run "Part 2" 1582688 solution2

#load "_benchmark.fsx"
