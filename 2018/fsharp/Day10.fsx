#load "Utils.fsx"
open Utils

type Light = { x: int; y: int; dx: int; dy: int }

let parseLine line =
    let rx =
        @"position=<\s*(-?\d+),\s*(-?\d+)> velocity=<\s*(-?\d+),\s*(-?\d+)>"

    match line with
    | Regex rx [ Int x; Int y; Int dx; Int dy ] -> {
        x = x
        y = y
        dx = dx
        dy = dy
      }
    | _ -> failwithf "Invalid input: %s" line

let posAfter time light = light.x + time * light.dx, light.y + time * light.dy

let size lights =
    let minX = lights |> Array.map (fun (x, _) -> x) |> Array.min
    let maxX = lights |> Array.map (fun (x, _) -> x) |> Array.max
    let minY = lights |> Array.map (fun (_, y) -> y) |> Array.min
    let maxY = lights |> Array.map (fun (_, y) -> y) |> Array.max
    maxX - minX + maxY - minY

let upperBound lights =
    let size0 = lights |> Array.map (posAfter 0) |> size

    let rec loop time =
        let s = lights |> Array.map (posAfter time) |> size
        if s > size0 then time else loop (time + 1000)

    loop 1000

let findTime lights =
    let posAfter time = lights |> Array.map (posAfter time)
    let sizeAfter time = time |> posAfter |> size

    let rec loop time1 time2 =
        match time2 - time1 with
        | 0 -> time1
        | 1 -> [ time1; time2 ] |> List.minBy sizeAfter
        | _ ->
            let t = (time1 + time2) / 2
            let t' = t + 1
            let s, s' = sizeAfter t, sizeAfter t'

            match sign (s' - s) with
            | 1 -> loop time1 t
            | -1 -> loop t time2
            | _ -> unreachable ()

    let t0, t1 = 0, upperBound lights
    loop t0 t1

let renderLights lights =
    let minX = lights |> Array.map (fun (x, _) -> x) |> Array.min
    let maxX = lights |> Array.map (fun (x, _) -> x) |> Array.max
    let minY = lights |> Array.map (fun (_, y) -> y) |> Array.min
    let maxY = lights |> Array.map (fun (_, y) -> y) |> Array.max

    let width, height = maxX - minX + 1, maxY - minY + 1
    let grid = Array2D.init width height (fun _ _ -> '.')

    for x, y in lights do
        grid.[x - minX, y - minY] <- '#'

    seq {
        ""

        for y in 0 .. height - 1 do
            sprintf "%s" (grid[*, y] |> String.ofChars)
    }
    |> String.join "\n"

let part1 input =
    let lights = input |> Array.map parseLine
    let t = lights |> findTime
    lights |> Array.map (posAfter t) |> renderLights

let part2 input = input |> Array.map parseLine |> findTime

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let result1 =
    "
######.....###..#....#..#....#...####....####...#....#..#....#
#...........#...#....#..##...#..#....#..#....#..##...#..#....#
#...........#....#..#...##...#..#.......#.......##...#...#..#.
#...........#....#..#...#.#..#..#.......#.......#.#..#...#..#.
#####.......#.....##....#.#..#..#.......#.......#.#..#....##..
#...........#.....##....#..#.#..#.......#.......#..#.#....##..
#...........#....#..#...#..#.#..#.......#.......#..#.#...#..#.
#.......#...#....#..#...#...##..#.......#.......#...##...#..#.
#.......#...#...#....#..#...##..#....#..#....#..#...##..#....#
######...###....#....#..#....#...####....####...#....#..#....#"

Test.run "Part 1" result1 solution1
Test.run "Part 2" 10612 solution2

#load "_benchmark.fsx"
