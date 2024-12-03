#load "Utils.fsx"

type Report = int array

let parseReport (line: string) : Report =
    line.Split [|' '|] |> Array.map int

let isGraduallyChanging (report: Report) =
    let sign' = sign (int report[1] - int report[0])
    sign' <> 0 &&
    report
    |> Seq.pairwise
    |> Seq.forall (fun (x, y) ->
        let diff = y - x
        let delta = abs diff
        sign' = sign diff && delta <> 0 && delta <= 3)

let generateSubsets (report: Report) = seq {
    for i in 0 .. report.Length - 1 do
        yield report |> Array.removeAt i
}

let isGraduallyChangingWithProblemDampener (report: Report) =
    report |> isGraduallyChanging ||
    generateSubsets report |> Seq.exists isGraduallyChanging

let part1 input = input |> Array.sumBy (fun line ->
    if line |> parseReport |> isGraduallyChanging then 1 else 0)

let part2 input = input |> Array.sumBy (fun line ->
    if line |> parseReport |> isGraduallyChangingWithProblemDampener
        then 1 else 0)

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 490 solution1
Utils.Test.run "Part 2" 536 solution2
