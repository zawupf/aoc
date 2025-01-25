#load "Utils.fsx"
open Utils

let parse (input: string[]) =
    let initialState = input[0][15..]
    let rules = input[2..]

    let parseRule (line: string) =
        let pattern = line[..4]
        let result = line[9]
        pattern, result

    initialState,
    rules
    |> Array.choose (fun rule ->
        match rule |> parseRule with
        | pattern, '#' -> Some pattern
        | _, '.' -> None
        | _ -> failwithf "Invalid rule: %s" rule)
    |> HashSet.ofSeq

let potSumAfter maxCount (state, rules) =
    let nextState (state: string) offset =
        let prefix = String.replicate (max 0 (4 - state.IndexOf '#')) "."

        let suffix =
            String.replicate
                (max 0 (state.LastIndexOf '#' + 4 - state.Length))
                "."

        let state =
            prefix + state + suffix
            |> Seq.windowed 5
            |> Seq.map String.ofChars
            |> Seq.map (fun w ->
                if rules |> HashSet.contains w then '#' else '.')
            |> String.ofChars

        state, offset + int64 prefix.Length - 2L

    let sumPots state offset =
        state
        |> Seq.indexed
        |> Seq.sumBy (fun (i, c) -> if c = '#' then int64 i - offset else 0L)

    let rec loop count prevState (state, offset) =
        if count = maxCount || state = prevState then
            sumPots state (offset + count - maxCount)
        else
            loop (count + 1L) state (nextState state offset)

    loop 0L "" (state, 0L)

let part1 input = input |> parse |> potSumAfter 20L

let part2 input = input |> parse |> potSumAfter 50000000000L

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" 325L (fun () -> part1 testInput[0])

Test.run "Part 1" 3798L solution1
Test.run "Part 2" 3900000002212L solution2

#load "_benchmark.fsx"
