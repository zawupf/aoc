#load "Utils.fsx"
open Utils

let SIZE = 300

let initGrid serialNumber =
    Array2D.initBased 1 1 SIZE SIZE (fun x y ->
        let rackId = x + 10
        (rackId * y + serialNumber) * rackId / 100 % 10 - 5)

let maxTotalPower (grid: int array2d) size =
    let sumOf_x_y1_y2 x y1 y2 =
        useCache<int * int * int, int> () (x, y1, y2) (fun _ ->
            Array.sum grid[x, y1..y2])

    let power11 = [| 1..size |] |> Array.sumBy (fun x -> sumOf_x_y1_y2 x 1 size)

    let row1 =
        Array.unfold
            (fun (power, x) ->
                if x = 0 then
                    None
                elif x + size > SIZE then
                    Some((power, (x, 1, size)), (0, 0))
                else
                    let power' =
                        power - sumOf_x_y1_y2 x 1 size
                        + sumOf_x_y1_y2 (x + size) 1 size

                    Some((power, (x, 1, size)), (power', x + 1)))
            (power11, 1)

    let rowDeltas y =
        Array.unfold
            (fun (d, x) ->
                if x = 0 then None
                elif x + size > SIZE then Some(d, (0, 0))
                else Some(d, (d - grid[x, y] + grid[x + size, y], x + 1)))
            (Array.sum grid[1..size, y], 1)

    Array.unfold
        (fun (row, y) ->
            if y = 0 then
                None
            elif y + size > SIZE then
                Some(Array.maxBy fst row, (Array.empty, 0))
            else
                let dSub = rowDeltas y
                let dAdd = rowDeltas (y + size)

                let row' =
                    Array.zip3 row dSub dAdd
                    |> Array.map (fun ((power, (x, y, s)), dSub, dAdd) ->
                        power - dSub + dAdd, (x, y + 1, s))

                Some(Array.maxBy fst row, (row', y + 1)))
        (row1, 1)
    |> Array.maxBy fst

let part1 input =
    maxTotalPower (input |> int |> initGrid) 3
    |> (fun (_, (x, y, _)) -> x, y)
    ||> sprintf "%d,%d"

let part2 input =
    let grid = input |> int |> initGrid

    [ 1..SIZE ]
    |> List.map (maxTotalPower grid)
    |> List.maxBy fst
    |> snd
    |> fun (x, y, s) -> sprintf "%d,%d,%d" x y s

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Part 1" "33,34" solution1
Test.run "Part 2" "235,118,14" solution2

#load "_benchmark.fsx"
