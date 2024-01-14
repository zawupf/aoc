#load "Utils.fsx"

let diffs = Array.pairwise >> Array.map (fun (a, b) -> b - a)

let calculateNumber valueGetter reducer =
    Array.unfold (fun xs ->
        if xs |> Array.forall ((=) 0) then
            None
        else
            Some(xs |> valueGetter, xs |> diffs))
    >> reducer

let nextNumber = calculateNumber Array.last Array.sum

let previousNumber = calculateNumber Array.head (Array.reduceBack (-))

let parse = Array.map (Utils.String.parseInts ' ' >> List.toArray)

let part1 input = //
    input |> parse |> Array.map nextNumber |> Array.sum

let part2 input = //
    input |> parse |> Array.map previousNumber |> Array.sum



let testInput =
    [|
        """
0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45
"""
    |]
    |> (Array.map Utils.String.toLines)

Utils.Test.run "Test part 1" 114 (fun () -> part1 testInput[0])
Utils.Test.run "Test part 2" 2 (fun () -> part2 testInput[0])



let input = Utils.readInputLines "09"

let getDay09_1 () = part1 input

let getDay09_2 () = part2 input

Utils.Test.run "Part 1" 1953784198 getDay09_1
Utils.Test.run "Part 2" 957 getDay09_2
