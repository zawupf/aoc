#load "Utils.fsx"
open Utils.FancyPatterns

type Grid = { Width: int; Height: int }
type Position = { X: int; Y: int }
type Velocity = { X: int; Y: int }

type Robot = { Position: Position; Velocity: Velocity }

type State = { Grid: Grid; Robots: Robot[] }

let parseRobot (line: string) =
    match line with
    | Regex @"(-?\d+)\D+?(-?\d+)\D+?(-?\d+)\D+?(-?\d+)" [ Int x
                                                          Int y
                                                          Int vx
                                                          Int vy ] -> {
        Position = { X = x; Y = y }
        Velocity = { X = vx; Y = vy }
      }
    | _ -> failwithf "Invalid input: %s" line

let modulo x y = (x % y + y) % y

let moveRobot n state robot =
    let x = robot.Position.X + n * robot.Velocity.X
    let y = robot.Position.Y + n * robot.Velocity.Y

    {
        robot with
            Position = {
                X = modulo x state.Grid.Width
                Y = modulo y state.Grid.Height
            }
    }

let moveRobots n state = {
    state with
        Robots = state.Robots |> Array.map (moveRobot n state)
}

let quadrantCount state =
    let middleWidth = state.Grid.Width / 2
    let middleHeight = state.Grid.Height / 2
    let counts = [| 0; 0; 0; 0 |]

    state.Robots
    |> Array.iter (fun robot ->
        match
            sign (robot.Position.X - middleWidth),
            sign (robot.Position.Y - middleHeight)
        with
        | -1, -1 -> counts[0] <- counts[0] + 1
        | 1, -1 -> counts[1] <- counts[1] + 1
        | -1, 1 -> counts[2] <- counts[2] + 1
        | 1, 1 -> counts[3] <- counts[3] + 1
        | _ -> ())

    counts

let safetyFactor state = state |> quadrantCount |> Array.reduce (*)

let findTree state =
    let threshold = Array.length state.Robots / 2

    seq { 1..10000 }
    |> Seq.tryFind (fun n ->
        state
        |> moveRobots n
        |> quadrantCount
        |> Array.exists (fun x -> x > threshold))
    |> function
        | Some n -> n
        | None -> failwith "No solution found"

let part1 input =
    {
        Grid = { Width = 101; Height = 103 }
        Robots = input |> Array.map parseRobot
    }
    |> moveRobots 100
    |> safetyFactor

let part2 input =
    {
        Grid = { Width = 101; Height = 103 }
        Robots = input |> Array.map parseRobot
    }
    |> findTree

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 233709840 solution1
Utils.Test.run "Part 2" 6620 solution2

#load "_benchmark.fsx"
