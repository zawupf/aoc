#load "Utils.fsx"
open Utils

type Register = Reg of char

type Value =
    | RegValue of Register
    | Const of int64

type Registers = Dictionary<Register, int64>

type Instruction =
    | Set of Register * Value
    | Add of Register * Value
    | Mul of Register * Value
    | Mod of Register * Value
    | Jump of Value * Value
    | Send of Register
    | Receive of Register

type Mode =
    | Single
    | Duet

type MessageQueue = System.Collections.Generic.Queue<int64>

type Duet = {
    Mode: Mode
    Instructions: Instruction[]
    CurrentInstruction: int
    Registers: Registers
    Messages: MessageQueue
    LastMessage: int64 option
    SendCount: int
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

    member private this.add x y = this.setR x (this.getR x + this.get y)
    member private this.mul x y = this.setR x (this.getR x * this.get y)

    member private this.mod' x y = this.setR x (this.getR x % this.get y)

    member private this.jump x y =
        if this.get x > 0 then
            {
                this with
                    CurrentInstruction =
                        this.CurrentInstruction + (this.get y |> int)
            }
        else
            this.next ()

    member private this.send x other =
        other.Messages.Enqueue(this.getR x)

        { this.next () with SendCount = this.SendCount + 1 }

    member private this.receive x =
        match this.Mode, this.Messages.Count with
        | Duet, 0 -> this
        | Duet, _ -> this.setR x (this.Messages.Dequeue())
        | Single, 0 -> failwith "No sound to recover"
        | Single, _ ->
            match this.getR x with
            | 0L -> this.next ()
            | _ ->
                let msg = this.Messages |> Seq.last

                { this.setR x msg with LastMessage = Some msg }

    member private this.isTerminated =
        this.CurrentInstruction < 0
        || this.CurrentInstruction >= this.Instructions.Length

    member private this.isWaiting =
        match this.Mode with
        | Duet ->
            match
                this.Instructions |> Array.tryItem this.CurrentInstruction
            with
            | Some(Receive _) -> this.Messages.Count = 0
            | _ -> false
        | Single -> false

    static member private isDeadlocked (a: Duet) (b: Duet) =
        a.isWaiting && b.isWaiting

    static member private isFinished (a: Duet) (b: Duet) =
        a.isTerminated && b.isTerminated || Duet.isDeadlocked a b

    member private this.execute other =
        match this.Instructions |> Array.tryItem this.CurrentInstruction with
        | None -> failwith "Program terminated"
        | Some(Set(x, y)) -> this.set x y
        | Some(Add(x, y)) -> this.add x y
        | Some(Mul(x, y)) -> this.mul x y
        | Some(Mod(x, y)) -> this.mod' x y
        | Some(Jump(x, y)) -> this.jump x y
        | Some(Send x) -> this.send x other
        | Some(Receive x) -> this.receive x

    member this.execute() = this.execute this

    static member execute(duet: Duet) =
        match duet.Mode with
        | Single ->
            let rec loop (duet: Duet) =
                match duet.LastMessage with
                | Some x -> x
                | None -> loop (duet.execute ())

            loop duet
        | Duet ->
            let rec loop d0 d1 =
                if Duet.isFinished d0 d1 then
                    d1.SendCount
                else
                    let d0' = d0.execute d1
                    let d1' = d1.execute d0
                    loop d0' d1'

            let d0 = {
                duet with
                    Registers = [ Reg 'p', 0L ] |> Dictionary.ofSeq
                    Messages = MessageQueue()
            }

            let d1 = {
                duet with
                    Registers = [ Reg 'p', 1L ] |> Dictionary.ofSeq
                    Messages = MessageQueue()
            }

            loop d0 d1

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
                | [| "add"; Register x; Value y |] -> Add(x, y)
                | [| "mul"; Register x; Value y |] -> Mul(x, y)
                | [| "mod"; Register x; Value y |] -> Mod(x, y)
                | [| "jgz"; Value x; Value y |] -> Jump(x, y)
                | [| "snd"; Register x |] -> Send x
                | [| "rcv"; Register x |] -> Receive x
                | _ -> failwithf "Invalid instruction: %s" line)
        CurrentInstruction = 0
        Registers = Dictionary()
        Messages = MessageQueue()
        LastMessage = None
        SendCount = 0
    }

let part1 input = input |> Duet.parse Single |> Duet.execute

let part2 input = input |> Duet.parse Duet |> Duet.execute

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
set a 1
add a 2
mul a a
mod a 5
snd a
set a 0
rcv a
jgz a -1
set a 1
jgz a -2
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" 4L (fun () -> part1 testInput[0])

Test.run "Part 1" 1187L solution1
Test.run "Part 2" 5969L solution2

#load "_benchmark.fsx"
