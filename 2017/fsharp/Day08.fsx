#load "Utils.fsx"
open Utils

type Reg = Reg of string

type Op =
    | Inc of Reg * int
    | Dec of Reg * int

type Cond =
    | Eq of Reg * int
    | Neq of Reg * int
    | Gt of Reg * int
    | Gte of Reg * int
    | Lt of Reg * int
    | Lte of Reg * int

type Instr = Instr of Op * Cond

let (|Op|_|) =
    function
    | "inc" -> Some Inc
    | "dec" -> Some Dec
    | _ -> None

let (|Cond|_|) =
    function
    | "==" -> Some Eq
    | "!=" -> Some Neq
    | ">" -> Some Gt
    | ">=" -> Some Gte
    | "<" -> Some Lt
    | "<=" -> Some Lte
    | _ -> None

let parseInstr line =
    let pattern = @"(\w+) (inc|dec) (-?\d+) if (\w+) (==|!=|>|<|>=|<=) (-?\d+)"

    match line with
    | Regex pattern [ r1; Op op; Int v1; r2; Cond cond; Int v2 ] ->
        Instr(op (Reg r1, v1), cond (Reg r2, v2))
    | _ -> failwithf "Invalid instruction: %s" line

let getReg (Reg r) regs =
    Map.tryFind r regs |> Option.defaultValue 0

let changeReg (Reg r) fn regs =
    let mutable value = 0

    Map.change
        r
        (fun v ->
            let v' = v |> Option.defaultValue 0 |> fn
            value <- v'
            v' |> Some)
        regs,
    value

let eval cond regs =
    let valueOf reg = getReg reg regs

    match cond with
    | Eq(r, v) -> valueOf r = v
    | Neq(r, v) -> valueOf r <> v
    | Gt(r, v) -> valueOf r > v
    | Gte(r, v) -> valueOf r >= v
    | Lt(r, v) -> valueOf r < v
    | Lte(r, v) -> valueOf r <= v

let run instructions =
    instructions
    |> Array.fold
        (fun (regs, maxValue) instr ->
            let (Instr(op, cond)) = instr

            if eval cond regs then
                let regs, newValue =
                    match op with
                    | Inc(r, v) -> regs |> changeReg r ((+) v)
                    | Dec(r, v) -> regs |> changeReg r ((+) -v)

                regs, max newValue maxValue
            else
                regs, maxValue)
        (Map.empty, 0)

let part1 input =
    input |> Array.map parseInstr |> run |> fst |> Map.values |> Seq.max

let part2 input =
    input |> Array.map parseInstr |> run |> snd


let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
b inc 5 if a > 1
a inc 1 if b < 5
c dec -10 if a >= 1
c inc -20 if c == 10
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" 1 (fun () -> part1 testInput[0])
Test.run "Test 2" 10 (fun () -> part2 testInput[0])

Test.run "Part 1" 4163 solution1
Test.run "Part 2" 5347 solution2

#load "_benchmark.fsx"
