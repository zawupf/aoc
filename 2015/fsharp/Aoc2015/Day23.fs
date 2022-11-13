module Day23

open Utils

type Register =
    | A
    | B

type Instruction =
    | Half of Register
    | Triple of Register
    | Increment of Register
    | Jump of int
    | JumpIfEven of Register * int
    | JumpIfOne of Register * int

module Instruction =
    let parse =
        let reg =
            function
            | "a" -> A
            | "b" -> B
            | _ -> failwith "Invalid register"

        function
        | Regex @"^hlf ([ab])$" [ r ] -> Half(reg r)
        | Regex @"^tpl ([ab])$" [ r ] -> Triple(reg r)
        | Regex @"^inc ([ab])$" [ r ] -> Increment(reg r)
        | Regex @"^jmp ([+\-]\d+)$" [ offset ] -> Jump(offset |> int)
        | Regex @"^jie ([ab]), ([+\-]\d+)$" [ r; offset ] ->
            JumpIfEven(reg r, offset |> int)
        | Regex @"^jio ([ab]), ([+\-]\d+)$" [ r; offset ] ->
            JumpIfOne(reg r, offset |> int)
        | _ -> failwith "Invalid instruction"

type Computer =
    { A: uint32
      B: uint32
      PC: int32
      Program: Instruction array }

module Computer =
    let empty =
        { A = 0u
          B = 0u
          PC = 0
          Program = [||] }

    let dump computer =
        let { A = a; B = b; PC = pc } = computer
        printfn "A:%d B:%d PC:%d" a b pc
        computer

    let load input computer =
        { computer with
            Program = input |> Seq.map Instruction.parse |> Seq.toArray }

    let get register computer =
        match register with
        | A -> computer.A
        | B -> computer.B

    let set register value computer =
        match register with
        | A -> { computer with A = value }
        | B -> { computer with B = value }

    let incPC computer = { computer with PC = computer.PC + 1 }

    let map register fn computer =
        let value = computer |> get register |> fn
        computer |> set register value

    let half register computer =
        computer |> map register (fun v -> v / 2u) |> incPC

    let triple register computer =
        computer |> map register (fun v -> v * 3u) |> incPC

    let increment register computer =
        computer |> map register (fun v -> v + 1u) |> incPC

    let jump offset computer =
        { computer with PC = computer.PC + offset }

    let (|Even|Odd|) value = if value % 2u = 0u then Even else Odd

    let jumpIfEven register offset computer =
        computer
        |> get register
        |> function
            | Even -> { computer with PC = computer.PC + offset }
            | Odd -> computer |> incPC

    let jumpIfOne register offset computer =
        computer
        |> get register
        |> function
            | 1u -> { computer with PC = computer.PC + offset }
            | _ -> computer |> incPC

    let run computer =
        let rec loop computer =
            match computer.Program |> Array.tryItem computer.PC with
            | Some instruction ->
                computer
                |> match instruction with
                   | Half r -> half r
                   | Triple r -> triple r
                   | Increment r -> increment r
                   | Jump o -> jump o
                   | JumpIfEven(r, o) -> jumpIfEven r o
                   | JumpIfOne(r, o) -> jumpIfOne r o
                |> loop
            | None -> computer

        loop computer

let input = readInputLines "23"

let job1 () =
    Computer.empty
    |> Computer.load input
    |> Computer.run
    |> Computer.get B
    |> string

let job2 () =
    { Computer.empty with A = 1u }
    |> Computer.load input
    |> Computer.run
    |> Computer.get B
    |> string
