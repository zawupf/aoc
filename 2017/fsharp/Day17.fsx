#load "Utils.fsx"
open Utils

let findValueAfter lastValue steps =
    let spinlock = ResizeArray [ 0 ]

    let rec loop pos value =
        if value > lastValue then
            spinlock[(pos + 1) % value]
        else
            let pos = (pos + steps) % value + 1
            spinlock.Insert(pos, value)
            loop pos (value + 1)

    loop 0 1

let findValueAfterZero lastValue steps =
    let rec loop pos value (pos0, result) =
        if value > lastValue then
            result
        else
            let pos = (pos + steps) % value + 1
            let result = if pos = pos0 + 1 then value else result
            let pos0 = if pos > pos0 then pos0 else pos0 + 1
            loop pos (value + 1) (pos0, result)

    loop 0 1 (0, 0)

let part1 input = input |> int |> findValueAfter 2017

let part2 input = input |> int |> findValueAfterZero 50_000_000

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput = [| "3" |] |> Array.map String.trim

Test.run "Test 1" 638 (fun () -> part1 testInput[0])

Test.run "Part 1" 1642 solution1
Test.run "Part 2" 33601318 solution2

#load "_benchmark.fsx"
