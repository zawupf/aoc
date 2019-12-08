module Day02

open System
open System.IO
open System.Collections.Generic

module Computer =
    type Opcode =
        | Add = 1
        | Mul = 2
        | Input = 3
        | Output = 4
        | JumpIfTrue = 5
        | JumpIfFalse = 6
        | LessThan = 7
        | Equals = 8
        | Halt = 99

    type Mode =
        | Positional
        | Immediate

    type Context =
        { memory: int []
          mutable position: int
          input: Queue<int>
          output: Queue<int> }

    type Event =
        | Halted
        | Paused
        | Output of int

    let compile (source: string) =
        { position = 0
          memory = source.Split ',' |> Array.map int
          input = new Queue<int>()
          output = new Queue<int>() }

    let opcode context =
        let _opcode = context.memory.[context.position] % 100
        match _opcode with
        | 1 -> Opcode.Add
        | 2 -> Opcode.Mul
        | 3 -> Opcode.Input
        | 4 -> Opcode.Output
        | 5 -> Opcode.JumpIfTrue
        | 6 -> Opcode.JumpIfFalse
        | 7 -> Opcode.LessThan
        | 8 -> Opcode.Equals
        | 99 -> Opcode.Halt
        | _ -> failwith "Invalid opcode"

    let modes context = context.memory.[context.position] / 100

    let mode index modes =
        let pow (a: int) (x: int) =
            let mutable y = x
            let mutable result = 1
            while y <> 0 do
                result <- result * a
                y <- y - 1
            result

        let pow10 = pow 10
        match modes / pow10 (index) % 10 with
        | 0 -> Positional
        | 1 -> Immediate
        | _ -> failwith "Invalid read mode"

    let readParameter context index =
        let posOrVal = context.memory.[context.position + index + 1]
        match mode index (modes context) with
        | Positional -> context.memory.[posOrVal]
        | Immediate -> posOrVal

    let writeParameter context index value =
        let pos = context.memory.[context.position + index + 1]
        match mode index (modes context) with
        | Positional -> context.memory.[pos] <- value
        | _ -> failwith "Invalid write mode"

    let binaryOp op context =
        let par = readParameter context
        let out = writeParameter context
        out 2 (op (par 0) (par 1))
        context.position <- context.position + 4
        None

    let add = binaryOp (+)

    let mul = binaryOp (*)

    let input context =
        if context.input.Count = 0 then
            Some(Paused)
        else
            writeParameter context 0 (context.input.Dequeue())
            context.position <- context.position + 2
            None

    let output context =
        let value = readParameter context 0
        context.output.Enqueue(value)
        context.position <- context.position + 2
        Some(Output value)

    let jump pred context =
        if pred then context.position <- readParameter context 1
        else context.position <- context.position + 3
        None

    let jumpIfTrue context = jump ((readParameter context 0) <> 0) context

    let jumpIfFalse context = jump ((readParameter context 0) = 0) context

    let lessThan context =
        let par = readParameter context
        let out = writeParameter context
        out 2
            (if (par 0) < (par 1) then 1
             else 0)
        context.position <- context.position + 4
        None

    let equals context =
        let par = readParameter context
        let out = writeParameter context
        out 2
            (if (par 0) = (par 1) then 1
             else 0)
        context.position <- context.position + 4
        None

    let halt() = Some(Halted)

    let handleOpcode context =
        let _opcode = opcode context
        match _opcode with
        | Opcode.Add -> add context
        | Opcode.Mul -> mul context
        | Opcode.Input -> input context
        | Opcode.Output -> output context
        | Opcode.JumpIfTrue -> jumpIfTrue context
        | Opcode.JumpIfFalse -> jumpIfFalse context
        | Opcode.LessThan -> lessThan context
        | Opcode.Equals -> equals context
        | Opcode.Halt -> halt()
        | _ -> failwith "Invalid opcode"

    let rec run context =
        let event = handleOpcode context
        match event with
        | None -> run context
        | Some(evt) ->
            match evt with
            | Halted -> evt
            | Paused -> evt
            | Output _ -> evt

    let rec runSilent context =
        let event = run context
        match event with
        | Halted
        | Paused -> event
        | Output _ -> runSilent context

    let patch context noun verb =
        let ctx = { context with memory = context.memory |> Array.map id }
        ctx.memory.[1] <- noun
        ctx.memory.[2] <- verb
        ctx

open Computer

let job1 prefix =
    let code = File.ReadAllText(prefix + "inputs/Day02.txt")

    let context: Context = patch (compile code) 12 2
    runSilent context |> ignore
    context.memory.[0] |> string

let job2 prefix =
    let code = File.ReadAllText(prefix + "inputs/Day02.txt")

    let originalContext = compile code

    let exec noun verb =
        let context = patch originalContext noun verb
        runSilent context |> ignore
        context.memory.[0]

    seq {
        for noun in 0 .. 99 do
            for verb in 0 .. 99 -> noun, verb
    }
    |> Seq.map (fun (noun, verb) -> (exec noun verb), noun * 100 + verb)
    |> Seq.filter (fun (exitCode, _) -> exitCode = 19690720)
    |> Seq.head
    |> snd
    |> string
