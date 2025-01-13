#load "Utils.fsx"
open Utils

type Claim = { Id: int; X: int; Y: int; Width: int; Height: int }

let toClaim line =
    let pattern = @"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)"

    match line with
    | Regex pattern [ Int id; Int x; Int y; Int width; Int height ] -> {
        Id = id
        X = x
        Y = y
        Width = width
        Height = height
      }
    | _ -> failwithf "Invalid input: %s" line

let positions claim = [|
    for x in claim.X .. (claim.X + claim.Width - 1) do
        for y in claim.Y .. (claim.Y + claim.Height - 1) do
            x, y
|]

let isOverlapping claim1 claim2 =
    let { Id = id1; X = x1; Y = y1; Width = w1; Height = h1 } = claim1
    let { Id = id2; X = x2; Y = y2; Width = w2; Height = h2 } = claim2

    id1 <> id2 && x1 < x2 + w2 && x2 < x1 + w1 && y1 < y2 + h2 && y2 < y1 + h1

let part1 input =
    input
    |> Seq.collect (toClaim >> positions)
    |> Seq.countBy id
    |> Seq.sumBy (fun (_, count) -> if count > 1 then 1 else 0)

let part2 input =
    let rec loop i claims =
        let claim = claims |> Array.item i

        if claims |> Array.exists (isOverlapping claim) then
            loop (i + 1) claims
        else
            claim.Id

    input |> Array.map toClaim |> loop 0

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Part 1" 111630 solution1
Test.run "Part 2" 724 solution2

#load "_benchmark.fsx"
