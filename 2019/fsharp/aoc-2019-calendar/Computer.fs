module Computer

open System.Collections.Generic

type private Opcode =
    | Add = 1
    | Mul = 2
    | Input = 3
    | Output = 4
    | JumpIfTrue = 5
    | JumpIfFalse = 6
    | LessThan = 7
    | Equals = 8
    | AdjustRelativeBase = 9
    | Halt = 99

type private Mode =
    | Positional
    | Immediate
    | Relative

type Context =
    { mutable memory: int64[]
      mutable position: int
      mutable relativeBase: int
      input: Queue<int64>
      output: Queue<int64> }

type Event =
    | Halted
    | Paused
    | Output of int64

let compile (source: string) =
    { position = 0
      relativeBase = 0
      memory = source.Split ',' |> Array.map int64
      input = new Queue<int64>()
      output = new Queue<int64>() }

let ensureMemory position context =
    let resize () =
        let rec guessNewLength len =
            if position < len then len else guessNewLength (2 * len)

        let oldLength = context.memory.Length
        let newLength = guessNewLength (2 * oldLength)

        context.memory <-
            Array.append
                context.memory
                (Array.zeroCreate (newLength - oldLength))

        ()

    if position >= context.memory.Length then
        resize ()

    ()

let private readMemory position context =
    ensureMemory position context
    context.memory.[position]

let private writeMemory position value context =
    ensureMemory position context
    context.memory.[position] <- value

let private opcode context =
    let _opcode = (readMemory context.position context) % 100L

    match _opcode with
    | 1L -> Opcode.Add
    | 2L -> Opcode.Mul
    | 3L -> Opcode.Input
    | 4L -> Opcode.Output
    | 5L -> Opcode.JumpIfTrue
    | 6L -> Opcode.JumpIfFalse
    | 7L -> Opcode.LessThan
    | 8L -> Opcode.Equals
    | 9L -> Opcode.AdjustRelativeBase
    | 99L -> Opcode.Halt
    | _ -> failwithf "Invalid opcode %d" _opcode

let private modes context =
    (readMemory context.position context) / 100L

let private mode index modes =
    let pow (a: int64) (x: int) =
        let mutable y = x
        let mutable result = 1L

        while y <> 0 do
            result <- result * a
            y <- y - 1

        result

    let pow10 = pow 10L

    match modes / (pow10 index) % 10L with
    | 0L -> Positional
    | 1L -> Immediate
    | 2L -> Relative
    | _ -> failwith "Invalid read mode"

let private readParameter context index =
    let posOrVal = readMemory (context.position + index + 1) context

    match mode index (modes context) with
    | Positional -> readMemory (int posOrVal) context
    | Immediate -> posOrVal
    | Relative -> readMemory (context.relativeBase + int posOrVal) context

let private writeParameter context index value =
    let pos = int (readMemory (context.position + index + 1) context)

    match mode index (modes context) with
    | Positional -> writeMemory pos value context
    | Relative -> writeMemory (context.relativeBase + pos) value context
    | _ -> failwith "Invalid write mode"

let private binaryOp op context =
    let par = readParameter context
    let out = writeParameter context
    out 2 (op (par 0) (par 1))
    context.position <- context.position + 4
    None

let private add = binaryOp (+)

let private mul = binaryOp (*)

let private input context =
    if context.input.Count = 0 then
        Some(Paused)
    else
        writeParameter context 0 (context.input.Dequeue())
        context.position <- context.position + 2
        None

let private output context =
    let value = readParameter context 0
    context.output.Enqueue(value)
    context.position <- context.position + 2
    Some(Output value)

let private jump pred context =
    if pred then
        context.position <- int (readParameter context 1)
    else
        context.position <- context.position + 3

    None

let private jumpIfTrue context =
    jump ((readParameter context 0) <> 0L) context

let private jumpIfFalse context =
    jump ((readParameter context 0) = 0L) context

let private lessThan context =
    let par = readParameter context
    let out = writeParameter context
    out 2 (if (par 0) < (par 1) then 1L else 0L)
    context.position <- context.position + 4
    None

let private equals context =
    let par = readParameter context
    let out = writeParameter context
    out 2 (if (par 0) = (par 1) then 1L else 0L)
    context.position <- context.position + 4
    None

let private adjustRelativeBase context =
    let value = int (readParameter context 0)
    context.relativeBase <- context.relativeBase + value
    context.position <- context.position + 2
    None

let private halt () = Some(Halted)

let private handleOpcode context =
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
    | Opcode.AdjustRelativeBase -> adjustRelativeBase context
    | Opcode.Halt -> halt ()
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

let isHalted context = Opcode.Halt = opcode context

let rec runSilent context =
    let event = run context

    match event with
    | Halted
    | Paused -> event
    | Output _ -> runSilent context

let patch context noun verb =
    let ctx =
        { context with
            memory = context.memory |> Array.map id }

    ctx.memory.[1] <- noun
    ctx.memory.[2] <- verb
    ctx
