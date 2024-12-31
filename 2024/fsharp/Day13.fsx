#load "Utils.fsx"
open Utils.FancyPatterns

type Step = { dx: int64; dy: int64 }
type Pos = { x: int64; y: int64 }

type Machine = {
    ButtonA: Step
    ButtonB: Step
    Prize: Pos
}

let parseMachine chunk =
    chunk
    |> Utils.String.toLines
    |> function
        | [| Regex @"Button A: X\+(\d+), Y\+(\d+)" [ Int64 dx_a; Int64 dy_a ]
             Regex @"Button B: X\+(\d+), Y\+(\d+)" [ Int64 dx_b; Int64 dy_b ]
             Regex @"Prize: X=(\d+), Y=(\d+)" [ Int64 x; Int64 y ] |] -> {
            ButtonA = { dx = dx_a; dy = dy_a }
            ButtonB = { dx = dx_b; dy = dy_b }
            Prize = { x = x; y = y }
          }
        | _ -> failwithf "Invalid game input:\n%s" chunk

let solve offset machine =
    let a, b, p =
        let { ButtonA = a; ButtonB = b; Prize = p } = machine

        a, b, { x = p.x + offset; y = p.y + offset }

    let det = a.dx * b.dy - a.dy * b.dx
    let n_a = (p.x * b.dy - p.y * b.dx) / det
    let n_b = (a.dx * p.y - a.dy * p.x) / det

    if a.dx * n_a + b.dx * n_b = p.x && a.dy * n_a + b.dy * n_b = p.y then
        Some(n_a, n_b)
    else
        None

let tokenCost (na, nb) = 3L * na + nb

let part1 input =
    input
    |> Utils.String.toSections
    |> Array.map parseMachine
    |> Array.choose (solve 0L)
    |> Array.sumBy tokenCost

let part2 input =
    input
    |> Utils.String.toSections
    |> Array.map parseMachine
    |> Array.choose (solve 10000000000000L)
    |> Array.sumBy tokenCost

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 35729L solution1
Utils.Test.run "Part 2" 88584689879723L solution2

#load "_benchmark.fsx"
