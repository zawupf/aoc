#load "Utils.fsx"
open Utils

let directPairs (input: string) =
    Seq.concat [ input; input.Substring(0, 1) ] |> Seq.pairwise

let oppositePairs (input: string) =
    let half = input.Length / 2
    Seq.zip input (input.Substring(half, half))

let part1 input =
    input
    |> directPairs
    |> Seq.sumBy (fun (a, b) -> if a = b then int (a - '0') else 0)

let part2 input =
    input
    |> oppositePairs
    |> Seq.sumBy (fun (a, b) -> if a = b then int (a - '0') else 0)
    |> (*) 2

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Test 1" 3 (fun () -> part1 "1122")
Test.run "Test 1" 4 (fun () -> part1 "1111")
Test.run "Test 1" 0 (fun () -> part1 "1234")
Test.run "Test 1" 9 (fun () -> part1 "91212129")

Test.run "Test 2" 6 (fun () -> part2 "1212")
Test.run "Test 2" 0 (fun () -> part2 "1221")
Test.run "Test 2" 4 (fun () -> part2 "123425")
Test.run "Test 2" 12 (fun () -> part2 "123123")
Test.run "Test 2" 4 (fun () -> part2 "12131415")

Test.run "Part 1" 1141 solution1
Test.run "Part 2" 950 solution2

#load "_benchmark.fsx"
