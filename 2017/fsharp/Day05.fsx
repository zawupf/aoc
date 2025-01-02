#load "Utils.fsx"
open Utils

let exitCount update jumps =
    let maxIndex = Array.length jumps - 1

    let rec loop index count =
        if index < 0 || index > maxIndex then
            count
        else
            let jump = jumps[index]
            jumps[index] <- update (jump)
            loop (index + jump) (count + 1)

    loop 0 0

let part1 input =
    input |> Array.map int |> exitCount (fun x -> x + 1)

let part2 input =
    input
    |> Array.map int
    |> exitCount (fun x -> if x >= 3 then x - 1 else x + 1)

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
0
3
0
1
-3
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" 5 (fun () -> part1 testInput[0])
Test.run "Test 2" 10 (fun () -> part2 testInput[0])

Test.run "Part 1" 359348 solution1
Test.run "Part 2" 27688760 solution2

#load "_benchmark.fsx"
