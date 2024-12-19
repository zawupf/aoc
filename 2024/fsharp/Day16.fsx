#load "Utils.fsx"
open System

type Pos = int * int
type Path = Pos list

type Orientation =
    | North = 0
    | East = 1
    | South = 2
    | West = 3

type State = {
    Pos: Pos
    Orientation: Orientation
    Path: Path
}

type Score = int

type Field = { IsWall: bool; Scores: Score[] }

type Maze = Field[][]

let parse lines : Maze =
    lines
    |> Array.map (fun line ->
        line
        |> Utils.String.toCharArray
        |> Array.map (fun c -> {
            IsWall = c = '#'
            Scores = Array.init 4 (fun _ -> Int32.MaxValue)
        }))

let nextPos (x, y) orientation =
    match orientation with
    | Orientation.North -> x, y - 1
    | Orientation.East -> x + 1, y
    | Orientation.South -> x, y + 1
    | Orientation.West -> x - 1, y
    | _ -> failwith "Invalid orientation"

let left orientation =
    match orientation with
    | Orientation.North -> Orientation.West
    | Orientation.East -> Orientation.North
    | Orientation.South -> Orientation.East
    | Orientation.West -> Orientation.South
    | _ -> failwith "Invalid orientation"

let right orientation =
    match orientation with
    | Orientation.North -> Orientation.East
    | Orientation.East -> Orientation.South
    | Orientation.South -> Orientation.West
    | Orientation.West -> Orientation.North
    | _ -> failwith "Invalid orientation"

type Result = { Score: Score; Paths: Path list }

let walk cancelCurrentPath joinPath (maze: Maze) =

    let end_x, end_y = maze[0].Length - 2, 1
    let isEnd (x, y) = x = end_x && y = end_y

    let field (x, y) = maze[y][x]

    let tryMove state =
        let {
                Pos = pos
                Orientation = orientation
                Path = path
            } =
            state

        let current = field pos
        let pos' = nextPos pos orientation
        let next = field pos'

        match next.IsWall with
        | true -> None
        | false ->
            match current.Scores.[int orientation] + 1 with
            | score when cancelCurrentPath next.Scores.[int orientation] score ->
                None
            | score ->
                next.Scores.[int orientation] <- score

                Some {
                    state with
                        Pos = pos'
                        Path = joinPath pos' path
                }

    let tryTurnAndMove turn state =
        let {
                Pos = pos
                Orientation = orientation
                Path = path
            } =
            state

        let orientation' = turn orientation

        match ((pos, orientation') ||> nextPos |> field).IsWall with
        | true -> None
        | false ->
            let current = field pos

            match current.Scores.[int orientation] + 1000 with
            | score when
                cancelCurrentPath current.Scores.[int orientation'] score
                ->
                None
            | score ->
                current.Scores.[int orientation'] <- score

                Some {
                    state with
                        Orientation = orientation'
                        Path = joinPath pos path
                }

    let rec loop result stack =
        match stack with
        | [] -> result
        | state :: stack ->
            match isEnd state.Pos with
            | true ->
                let {
                        Pos = pos
                        Orientation = orientation
                        Path = path
                    } =
                    state

                match (field pos).Scores.[int orientation] with
                | score when score < result.Score ->
                    loop { Score = score; Paths = [ path ] } stack
                | score when score = result.Score ->
                    loop
                        {
                            result with
                                Paths = path :: result.Paths
                        }
                        stack
                | _ -> loop result stack
            | false ->
                let stack' =
                    [
                        tryMove state
                        tryTurnAndMove left state
                        tryTurnAndMove right state
                    ]
                    |> List.choose id
                    |> List.append stack

                loop result stack'

    let start = 1, maze.Length - 2
    (start |> field).Scores.[int Orientation.East] <- 0

    loop { Score = Int32.MaxValue; Paths = [] } [
        {
            Pos = start
            Orientation = Orientation.East
            Path = [ start ]
        }
    ]

let lowestScore maze =
    let result = walk (<=) (fun _ b -> b) maze
    result.Score

let posCount maze =
    let result = walk (<) (fun a b -> a :: b) maze
    result.Paths |> Seq.concat |> Seq.distinct |> Seq.length

let part1 input = input |> parse |> lowestScore

let part2 input = input |> parse |> posCount

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
        ###############
        #.......#....E#
        #.#.###.#.###.#
        #.....#.#...#.#
        #.###.#####.#.#
        #.#.#.......#.#
        #.#.#####.###.#
        #...........#.#
        ###.#.#####.#.#
        #...#.....#.#.#
        #.#.#.###.#.#.#
        #.....#...#.#.#
        #.###.#.#.#.#.#
        #S..#.....#...#
        ###############
        """
        """
        #################
        #...#...#...#..E#
        #.#.#.#.#.#.#.#.#
        #.#.#.#...#...#.#
        #.#.#.#.###.#.#.#
        #...#.#.#.....#.#
        #.#.#.#.#.#####.#
        #.#...#.#.#.....#
        #.#.#####.#.###.#
        #.#.#.......#...#
        #.#.###.#####.###
        #.#.#...#.....#.#
        #.#.#.#####.###.#
        #.#.#.........#.#
        #.#.#.#########.#
        #S#.............#
        #################
        """
    |]
    |> Array.map Utils.String.toLines

Utils.Test.run "Test 1" 7036 (fun () -> part1 testInput[0])
Utils.Test.run "Test 1" 11048 (fun () -> part1 testInput[1])
Utils.Test.run "Test 2" 45 (fun () -> part2 testInput[0])
Utils.Test.run "Test 2" 64 (fun () -> part2 testInput[1])

Utils.Test.run "Part 1" 85432 solution1
Utils.Test.run "Part 2" 465 solution2

#load "_benchmark.fsx"
