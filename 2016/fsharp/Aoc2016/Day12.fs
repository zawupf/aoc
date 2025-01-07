module Day12

open Utils

[<Measure>]
type value

[<Measure>]
type offset

type Value = int<value>
type Offset = int<offset>

type Reg =
    | RA
    | RB
    | RC
    | RD

type Data =
    | Value of Value
    | Reg of Reg

let (|Reg'|_|) string =
    match string with
    | "a" -> Some RA
    | "b" -> Some RB
    | "c" -> Some RC
    | "d" -> Some RD
    | _ -> None

let (|Value'|_|) string =
    match string with
    | Int i -> Some(i * 1<value>)
    | _ -> None

let (|Offset'|_|) string =
    match string with
    | Int i -> Some(i * 1<offset>)
    | _ -> None

let (|Data|_|) string =
    match string with
    | Reg' r -> r |> Reg |> Some
    | Value' i -> i |> Value |> Some
    | _ -> None

type Instruction =
    | Copy of Data * Reg
    | Inc of Reg
    | Dec of Reg
    | JumpIfNonZero of Data * Offset

module Instruction =
    let parse line =
        match line with
        | Regex @"^cpy (\w+) (\w+)$" [ Data data; Reg' reg ] -> Copy(data, reg)
        | Regex @"^inc (\w+)$" [ Reg' reg ] -> Inc reg
        | Regex @"^dec (\w+)$" [ Reg' reg ] -> Dec reg
        | Regex @"^jnz (\w+) (-?\d+)$" [ Data data; Offset' offset ] ->
            JumpIfNonZero(data, offset)
        | _ -> failwith $"Invalid code: %s{line}"

type Computer = {
    RA: Value
    RB: Value
    RC: Value
    RD: Value
    PC: Offset
    Code: Instruction array
}

module Computer =
    let empty = {
        RA = 0<value>
        RB = 0<value>
        RC = 0<value>
        RD = 0<value>
        PC = 0<offset>
        Code = [||]
    }

    let get reg computer =
        match reg with
        | RA -> computer.RA
        | RB -> computer.RB
        | RC -> computer.RC
        | RD -> computer.RD

    let set value reg computer =
        match reg with
        | RA -> { computer with RA = value }
        | RB -> { computer with RB = value }
        | RC -> { computer with RC = value }
        | RD -> { computer with RD = value }

    let add value reg computer =
        match reg with
        | RA -> { computer with RA = computer.RA + value }
        | RB -> { computer with RB = computer.RB + value }
        | RC -> { computer with RC = computer.RC + value }
        | RD -> { computer with RD = computer.RD + value }

    let value data computer =
        match data with
        | Value v -> v
        | Reg r -> computer |> get r

    let incPC offset computer = { computer with PC = computer.PC + offset }

    let load input computer = {
        computer with
            Code = input |> Array.map Instruction.parse
    }

    let hasNext computer =
        let pc = int computer.PC
        pc >= 0 && pc < computer.Code.Length

    let next computer =
        match computer.Code |> Array.tryItem (int computer.PC) with
        | Some instruction ->
            match instruction with
            | Copy(data, reg) ->
                computer |> set (value data computer) reg |> incPC 1<_>
            | Inc reg -> computer |> add 1<_> reg |> incPC 1<_>
            | Dec reg -> computer |> add -1<_> reg |> incPC 1<_>
            | JumpIfNonZero(data, offset) ->
                match computer |> value data with
                | 0<value> -> computer |> incPC 1<_>
                | _ -> computer |> incPC offset
        | None -> computer

    let rec run computer =
        match computer |> hasNext with
        | true -> computer |> next |> run
        | false -> computer

let input = readInputLines "12" |> Seq.toArray

let job1 () =
    Computer.empty
    |> Computer.load input
    |> Computer.run
    |> Computer.get RA
    |> string

let job2 () =
    { Computer.empty with RC = 1<_> }
    |> Computer.load input
    |> Computer.run
    |> Computer.get RA
    |> string
