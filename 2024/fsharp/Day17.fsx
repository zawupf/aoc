#load "Utils.fsx"
open Utils.FancyPatterns

type Opcode =
    | ADV
    | BXL
    | BST
    | JNZ
    | BXC
    | OUT
    | BDV
    | CDV

type Operand =
    | Combo of uint8
    | Literal of uint8
    | Unused

type Registers = { A: uint64; B: uint64; C: uint64 }

type Computer = {
    Program: uint8[]
    PC: uint8
    Reg: Registers
    Out: uint8 list
}

let parse input =
    match input |> Utils.String.toLines with
    | [| Regex @"Register A: (\d+)" [ UInt64 a ]
         Regex @"Register B: (\d+)" [ UInt64 b ]
         Regex @"Register C: (\d+)" [ UInt64 c ]
         _
         Regex @"Program: ([0-7](?:,[0-7])*)" [ program ] |] -> {
        Program = program |> Utils.String.split "," |> Array.map uint8
        PC = 0uy
        Reg = { A = a; B = b; C = c }
        Out = []
      }
    | _ -> failwithf "Invalid input:\n%s" input

let readInstruction computer =
    let { Program = program; PC = pc } = computer

    match program[int pc], program[int pc + 1] with
    | 0uy, v -> ADV, Combo v
    | 1uy, v -> BXL, Literal v
    | 2uy, v -> BST, Combo v
    | 3uy, v -> JNZ, Literal v
    | 4uy, _ -> BXC, Unused
    | 5uy, v -> OUT, Combo v
    | 6uy, v -> BDV, Combo v
    | 7uy, v -> CDV, Combo v
    | opcode, _ -> failwithf "Invalid opcode: %d" opcode

let value registers operand =
    match operand with
    | Combo 4uy -> registers.A
    | Combo 5uy -> registers.B
    | Combo 6uy -> registers.C
    | Combo 7uy -> failwith "Combo 7 is reserved for future use"
    | Combo v -> uint64 v
    | Literal v -> uint64 v
    | Unused -> failwith "Unused operand"

let execute computer =
    let c, reg =
        let { PC = pc; Reg = reg } = computer
        { computer with PC = pc + 2uy }, reg

    let u64_value = value reg
    let i32_value = u64_value >> int
    let u8_value = u64_value >> uint8

    match readInstruction computer with
    | ADV, operand -> {
        c with
            Reg.A = reg.A >>> i32_value operand
      }
    | BXL, operand -> {
        c with
            Reg.B = reg.B ^^^ u64_value operand
      }
    | BST, operand -> {
        c with
            Reg.B = u64_value operand &&& 0b111UL
      }
    | JNZ, operand -> {
        c with
            PC = if reg.A <> 0UL then u8_value operand else c.PC
      }
    | BXC, _ -> { c with Reg.B = reg.B ^^^ reg.C }
    | OUT, operand -> {
        c with
            Out = (u8_value operand &&& 0b111uy) :: computer.Out
      }
    | BDV, operand -> {
        c with
            Reg.B = reg.A >>> i32_value operand
      }
    | CDV, operand -> {
        c with
            Reg.C = reg.A >>> i32_value operand
      }

let output computer =
    computer.Out |> List.rev |> List.map string |> String.concat ","

[<TailCall>]
let rec run computer =
    if computer.PC >= uint8 computer.Program.Length then
        computer
    else
        run (execute computer)

[<TailCall>]
let rec runUntilOut computer =
    if
        computer.PC >= uint8 computer.Program.Length
        || computer.Out |> List.isEmpty |> not
    then
        computer
    else
        runUntilOut (execute computer)

let findRegisterValue computer =
    let program' = computer.Program |> Array.rev

    let rec loop stack =
        match stack with
        | [] -> failwith "No solution found"
        | (i, result) :: _ when i = program'.Length -> result
        | (i, result) :: stack ->
            let a' = result <<< 3

            let stack' =
                [ 0UL .. 7UL ]
                |> List.map (fun v -> i + 1, a' + v)
                |> List.filter (fun (_, a) ->
                    let c = runUntilOut { computer with Reg.A = a }
                    c.Out |> List.head = program'.[i])

            loop (stack' @ stack)

    loop [ 0, 0UL ]

let part1 input = input |> parse |> run |> output

let part2 input = input |> parse |> findRegisterValue

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" "4,1,5,3,1,5,3,5,7" solution1
Utils.Test.run "Part 2" 164542125272765UL solution2

#load "_benchmark.fsx"
