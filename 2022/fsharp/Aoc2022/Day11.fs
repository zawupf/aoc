module Day11

open Utils

type Operation =
    | Add of uint64
    | Mul of uint64
    | Sq

type Mode =
    | WithDiv
    | WithoutDiv

type Monkey = {
    items: uint64 list
    operation: Operation
    test: uint64
    onTrueFalse: int * int
}

module Monkey =
    let parse input =
        let lines = input |> String.toLines

        let items =
            match lines[1] with
            | Regex @"^Starting items: ([\d, ]+)$" [ items ] ->
                items
                |> String.split ','
                |> Array.map (String.trim >> uint64)
                |> Array.toList
            | line -> failwith $"Invalid input[1]: %s{line}"

        let operation =
            match lines[2] with
            | Regex @"^Operation: new = old (\+|\*) (\d+|old)$" [ op; value ] ->
                match op, value with
                | "+", UInt64 v -> Add v
                | "*", UInt64 v -> Mul v
                | "*", "old" -> Sq
                | _ -> failwith $"Invalid op/value: %s{op}/%s{value}"
            | line -> failwith $"Invalid input[2]: %s{line}"

        let test =
            match lines[3] with
            | Regex @"^Test: divisible by (\d+)$" [ UInt64 d ] -> d
            | line -> failwith $"Invalid input[3]: %s{line}"

        let onTrue =
            match lines[4] with
            | Regex @"^If true: throw to monkey (\d+)$" [ Int i ] -> i
            | line -> failwith $"Invalid input[4]: %s{line}"

        let onFalse =
            match lines[5] with
            | Regex @"^If false: throw to monkey (\d+)$" [ Int i ] -> i
            | line -> failwith $"Invalid input[5]: %s{line}"

        {
            items = items
            operation = operation
            test = test
            onTrueFalse = onTrue, onFalse
        }

    let parseMany input =
        (input |> String.trim).Split("\n\n") |> Array.map parse

    let playMonkey magicNumber mode index (monkeys: Monkey[]) =
        let monkey = monkeys[index]

        monkey.items
        |> List.iter (fun item ->
            let worryLevel =
                match mode with
                | WithDiv ->
                    match monkey.operation with
                    | Add v -> (item + v) / 3UL
                    | Mul v -> (item * v) / 3UL
                    | Sq -> (item * item) / 3UL
                | WithoutDiv ->
                    match monkey.operation with
                    | Add v -> item + v
                    | Mul v -> item * v
                    | Sq -> item * item

            let target =
                monkey.onTrueFalse
                |> match worryLevel % monkey.test with
                   | 0UL -> fst
                   | _ -> snd

            monkeys[target] <- {
                monkeys[target] with
                    items =
                        monkeys[target].items @ [ worryLevel % magicNumber ]
            })

        monkeys[index] <- { monkey with items = [] }

let monkeyBusiness mode rounds (monkeys: Monkey[]) =
    let magicNumber = monkeys |> Array.map (fun m -> m.test) |> Array.reduce (*)

    [ 0 .. rounds * monkeys.Length - 1 ]
    |> List.fold
        (fun (acc: _[]) i ->
            let index = i % monkeys.Length

            acc[index] <- acc[index] + uint64 monkeys[index].items.Length

            Monkey.playMonkey magicNumber mode index monkeys
            acc)
        (monkeys |> Array.map (fun _ -> 0UL))
    |> Array.sortDescending
    |> Array.take 2
    |> Array.reduce (*)

let input = readInputText "11"

let job1 () =
    input |> Monkey.parseMany |> monkeyBusiness WithDiv 20 |> string

let job2 () =
    input |> Monkey.parseMany |> monkeyBusiness WithoutDiv 10000 |> string
