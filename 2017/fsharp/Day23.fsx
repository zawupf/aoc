#load "Utils.fsx"
open Utils

type Register = Reg of char

type Value =
    | RegValue of Register
    | Const of int64

type Registers = Dictionary<Register, int64>

type Instruction =
    | Set of Register * Value
    | Sub of Register * Value
    | Mul of Register * Value
    | Jump of Value * Value

type Mode =
    | Debug
    | Release

type Duet = {
    Mode: Mode
    Instructions: Instruction[]
    CurrentInstruction: int
    Registers: Registers
    MulCount: int
} with

    member private this.getR x =
        this.Registers |> Dictionary.tryGetValue x |> Option.defaultValue 0

    member private this.get y =
        match y with
        | RegValue x -> this.getR x
        | Const i -> i

    member private this.next() = {
        this with
            CurrentInstruction = this.CurrentInstruction + 1
    }

    member private this.setR x y =
        this.Registers[x] <- y
        this.next ()

    member private this.set x y = this.setR x (this.get y)

    member private this.sub x y = this.setR x (this.getR x - this.get y)

    member private this.mul x y = {
        this.setR x (this.getR x * this.get y) with
            MulCount = this.MulCount + 1
    }

    member private this.jump x y =
        if this.get x <> 0 then
            {
                this with
                    CurrentInstruction =
                        this.CurrentInstruction + (this.get y |> int)
            }
        else
            this.next ()

    member private this.isTerminated =
        this.CurrentInstruction < 0
        || this.CurrentInstruction >= this.Instructions.Length

    member private this.execute _other =
        match this.Instructions |> Array.tryItem this.CurrentInstruction with
        | None -> failwith "Program terminated"
        | Some(Set(x, y)) -> this.set x y
        | Some(Sub(x, y)) -> this.sub x y
        | Some(Mul(x, y)) -> this.mul x y
        | Some(Jump(x, y)) -> this.jump x y

    member this.execute() = this.execute this

    static member execute(duet: Duet) =
        match duet.Mode with
        | Debug ->
            let rec loop (duet: Duet) =
                match duet.isTerminated with
                | true -> duet.MulCount
                | false -> loop (duet.execute ())

            loop duet
        | Release ->
            let rec loop (duet: Duet) =
                match duet.isTerminated with
                | true -> duet.MulCount
                | false -> loop (duet.execute ())

            loop {
                duet with
                    Registers = [ Reg 'a', 1L ] |> Dictionary.ofSeq
            }

    static member parse mode lines = {
        Mode = mode
        Instructions =
            let (|Register|_|) =
                function
                | Char c when 'a' <= c && c <= 'z' -> Some(Reg c)
                | _ -> None

            let (|Value|_|) =
                function
                | Register r -> Some(RegValue r)
                | Int i -> Some(Const i)
                | _ -> None

            lines
            |> Array.map (fun line ->
                match line |> String.split ' ' with
                | [| "set"; Register x; Value y |] -> Set(x, y)
                | [| "sub"; Register x; Value y |] -> Sub(x, y)
                | [| "mul"; Register x; Value y |] -> Mul(x, y)
                | [| "jnz"; Value x; Value y |] -> Jump(x, y)
                | _ -> failwithf "Invalid instruction: %s" line)
        CurrentInstruction = 0
        Registers = Dictionary()
        MulCount = 0
    }

let part1 input =
    input |> Duet.parse Debug |> Duet.execute

let part2 _input = // WTF!! 🤣
    let start, stop, step = 107900, 107900 + 17000, 17

    seq { start..step..stop }
    |> Seq.sumBy (fun b ->
        seq { 2 .. b / 2 } |> Seq.exists (fun d -> b % d = 0) |> Bool.toInt)

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Part 1" 5929 solution1
Test.run "Part 2" 907 solution2

#load "_benchmark.fsx"
