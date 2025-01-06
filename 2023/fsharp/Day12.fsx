#load "Utils.fsx"

open System.Collections.Generic

let isWildcardMatch input pattern =
    Seq.forall2 (fun i p -> p = '?' || i = p) input pattern

let isAnyOnly input = Seq.forall (fun i -> i = '?') input

let isAllAnyOnly inputs = Seq.forall isAnyOnly inputs

let searchPatterns = Array.init 16 (fun i -> String.replicate i "#")

let cache = Dictionary<string * int, string list * string list>()

let rec arrange chunk count =
    let key = chunk, count

    match cache.TryGetValue(key) with
    | true, result -> result
    | false, _ ->

        let result =
            match sign (chunk.Length - count) with
            | 0 ->
                let search = searchPatterns[count]
                let matching = isWildcardMatch search chunk

                (if matching then [ "" ] else []),
                (if isAnyOnly chunk then [ "" ] else [])
            | 1 ->
                let search = searchPatterns[count] + "."
                let rest = chunk[search.Length ..]

                let matching =
                    isWildcardMatch search chunk[.. search.Length - 1]

                match matching, chunk[0] with
                | true, '?' ->
                    let l1, l2 = arrange chunk[1..] count
                    (rest :: l1), l2
                | false, '?' -> arrange chunk[1..] count
                | true, _ -> [ rest ], []
                | false, _ -> [], []
            | -1
            | _ -> [], (if isAnyOnly chunk then [ "" ] else [])

        cache.Add(key, result)
        // (key, result) |> Utils.dump |> ignore
        result

[<TailCall>]
let rec countArrangements result (data: (string list * int list * int64) list) =
    match data with
    | [] -> result
    | (springChunks, counts, r) :: data ->
        // Utils.dump (springChunks, counts, r, result) |> ignore

        match counts, springChunks with
        | [], [] -> countArrangements (result + r) data
        | [], chunks when isAllAnyOnly chunks ->
            countArrangements (result + r) data
        | [], _
        | _, [] -> countArrangements result data
        | counts, ("" :: chunks) ->
            countArrangements result ((chunks, counts, r) :: data)
        | (count :: counts), (chunk :: chunks) ->
            let matchingRests, nonMatchingRests = arrange chunk count

            let data =
                nonMatchingRests
                |> List.fold
                    (fun acc rest ->
                        ((rest :: chunks), (count :: counts), r) :: acc)
                    data

            let data =
                matchingRests
                |> List.fold
                    (fun acc rest -> ((rest :: chunks), counts, r) :: acc)
                    data

            countArrangements result data

// match springs.Length, List.length counts, isDotsOnly springs with
// | 0, 0, _
// | _, 0, true -> countArrangements (result + 1L) data
// | 0, _, _
// | _, 0, _ -> countArrangements result data
// | _ ->
//     let data =
//         match springs[0] with
//         | '.'
//         | '?' -> (springs[1..], counts) :: data
//         | _ -> data

//     let count = counts.Head
//     let search = searchPatterns[count]

//     match search.Length with
//     | l when l > springs.Length -> countArrangements result data
//     | l ->
//         let pattern = springs[..l]

//         match isWildcardMatch search pattern with
//         | false -> countArrangements result data
//         | true ->
//             countArrangements
//                 result
//                 ((springs[l..], counts.Tail) :: data)

let arrangementCountWithRepeat repeat line =
    let lineParts = Utils.String.split ' ' line

    let springChunks =
        List.replicate repeat lineParts[0]
        |> Utils.String.join "?"
        |> Utils.String.splitNoEmpty '.'
        |> Array.toList

    let counts =
        List.replicate repeat lineParts[1]
        |> Utils.String.join ","
        |> Utils.String.parseInts ','
        |> Array.toList

    countArrangements 0L [ springChunks, counts, 1L ]

let part1 input = input |> Array.sumBy (arrangementCountWithRepeat 1)

let part2 input =
    input
    |> Array.indexed
    |> Array.sumBy (fun (_, line) -> arrangementCountWithRepeat 5 line)



let testInput =
    """
???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1
"""
    |> Utils.String.toLines

Utils.Test.run "Test part 1 (1/7)" 1L (fun () -> part1 testInput[0..0])
Utils.Test.run "Test part 1 (2/7)" 4L (fun () -> part1 testInput[1..1])
Utils.Test.run "Test part 1 (3/7)" 1L (fun () -> part1 testInput[2..2])
Utils.Test.run "Test part 1 (4/7)" 1L (fun () -> part1 testInput[3..3])
Utils.Test.run "Test part 1 (5/7)" 4L (fun () -> part1 testInput[4..4])
Utils.Test.run "Test part 1 (6/7)" 10L (fun () -> part1 testInput[5..5])
Utils.Test.run "Test part 1 (7/7)" 21L (fun () -> part1 testInput)
Utils.Test.run "Test part 2" 525152L (fun () -> part2 testInput)



let input = Utils.readInputLines "12"

let getDay12_1 () = part1 input

let getDay12_2 () = part2 input

Utils.Test.run "Part 1" 8419L getDay12_1
// Utils.Test.run "Part 2" 0L getDay12_2
