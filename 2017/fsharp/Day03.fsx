#load "Utils.fsx"
open Utils

let locations =
    seq {
        yield 0, 0
        yield 0, 0

        yield!
            Seq.initInfinite (fun i ->
                let n = i + 1
                let start = -n + 1
                let stop = n

                seq {
                    for y in start..stop -> n, y
                    for x in -start .. -1 .. -stop -> x, n
                    for y in -start .. -1 .. -stop -> -n, y
                    for x in start..stop -> x, -n
                })
            |> Seq.concat
    }

let values =
    let getValue = useCacheWith [ (0, 0), 1 ]

    locations
    |> Seq.map (fun (x, y) ->
        getValue (x, y)
        <| fun cache ->
            [
                for dx in -1 .. 1 do
                    for dy in -1 .. 1 do
                        cache
                        |> Dictionary.tryGetValue (x + dx, y + dy)
                        |> Option.defaultValue 0
            ]
            |> List.sum)

let distance (x, y) = abs x + abs y

let part1 input = input |> int |> Seq.item <| locations |> distance

let part2 input =
    let input = input |> int
    values |> Seq.find (fun v -> v > input)

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Test 1" 0 (fun () -> part1 "1")
Test.run "Test 1" 3 (fun () -> part1 "12")
Test.run "Test 1" 2 (fun () -> part1 "23")
Test.run "Test 1" 31 (fun () -> part1 "1024")

Test.run "Part 1" 430 solution1
Test.run "Part 2" 312453 solution2

#load "_benchmark.fsx"
