#load "Utils.fsx"
open Utils.FancyPatterns

type Wire =
    | X of int
    | Y of int
    | Z of int
    | Inner of string

type Operation =
    | AND
    | OR
    | XOR

type Gate = {
    Op: Operation
    Inputs: Wire[]
    Output: Wire
}

type Gates = Map<Wire, Gate>

type Circuit = { Gates: Gates; X: uint64; Y: uint64 }

let (|Wire|) =
    function
    | Regex @"x(\d\d)" [ Int x ] -> X x
    | Regex @"y(\d\d)" [ Int y ] -> Y y
    | Regex @"z(\d\d)" [ Int z ] -> Z z
    | wire -> Inner wire

let isZWire =
    function
    | Z _ -> true
    | _ -> false

let isXYWire =
    function
    | X _
    | Y _ -> true
    | _ -> false

let (|Operation|_|) =
    function
    | "AND" -> Some AND
    | "OR" -> Some OR
    | "XOR" -> Some XOR
    | _ -> None

let parse input =
    match input |> Utils.String.toSections with
    | [| xy; gates |] ->
        let sx, sy =
            xy
            |> Utils.String.toLines
            |> Array.partition (Utils.String.startsWith "x")

        let x =
            sx
            |> Array.sort
            |> Array.mapi (fun i line -> line[5] - '0' |> uint64 <<< i)
            |> Array.sum

        let y =
            sy
            |> Array.sort
            |> Array.mapi (fun i line -> line[5] - '0' |> uint64 <<< i)
            |> Array.sum

        let gates =
            gates
            |> Utils.String.toLines
            |> Array.map (fun line ->
                match line |> Utils.String.split " " with
                | [| Wire in1; Operation op; Wire in2; _; Wire output |] ->
                    output,
                    {
                        Op = op
                        Inputs = [| in1; in2 |]
                        Output = output
                    }
                | _ -> failwithf "Invalid gate: %s" line)

        {
            Gates = gates |> Map.ofArray
            X = x
            Y = y
        }
    | _ -> failwith "Invalid input"

let evaluate circuit =
    let getWireValue = Utils.useCache<Wire, uint64> ()

    let rec eval wire =
        getWireValue wire
        <| fun () ->
            match wire with
            | X x -> if circuit.X &&& (1UL <<< x) = 0UL then 0UL else 1UL
            | Y y -> if circuit.Y &&& (1UL <<< y) = 0UL then 0UL else 1UL
            | wire ->
                let gate = circuit.Gates[wire]

                match gate.Op, gate.Inputs |> Array.map eval with
                | AND, [| a; b |] -> a &&& b
                | OR, [| a; b |] -> a ||| b
                | XOR, [| a; b |] -> a ^^^ b
                | _ -> failwithf "Invalid gate: %A" gate

    circuit.Gates
    |> Map.keys
    |> Seq.sumBy (function
        | Z z -> eval (Z z) <<< z
        | _ -> 0UL)

let isInvalidGate circuit gate =
    let isZOutput = gate.Output |> isZWire

    let isLastZBit =
        gate.Output = (circuit.Gates
                       |> Map.keys
                       |> Seq.filter isZWire
                       |> Seq.max)

    let isXYInput = gate.Inputs |> Array.forall isXYWire

    isZOutput && not isLastZBit && gate.Op <> XOR
    || not isZOutput && not isXYInput && gate.Op = XOR

let rec resolveOutputs circuit gate =
    if gate.Output |> isZWire then
        [ gate.Output ]
    else
        circuit.Gates
        |> Map.values
        |> Seq.filter (fun g ->
            g.Inputs |> Array.exists (fun input -> input = gate.Output))
        |> Seq.toList
        |> List.collect (resolveOutputs circuit)

let fixGates circuit =
    let invalidZWires, invalidInnerWires =
        circuit.Gates
        |> Map.values
        |> Seq.toList
        |> List.choose (fun g ->
            if isInvalidGate circuit g then Some g.Output else None)
        |> List.partition isZWire

    let invalidZWiresNext =
        invalidZWires
        |> List.map (function
            | Z z -> Z(z + 1)
            | _ -> failwith "Invalid wire")

    let isInNext wire = invalidZWiresNext |> List.contains wire

    let wireMapping =
        invalidInnerWires
        |> List.map (fun w ->
            w, resolveOutputs circuit circuit.Gates[w] |> List.filter isInNext)
        |> List.sortBy (fun (_, outputs) -> outputs.Length)
        |> List.fold
            (fun (result, prev) (w1, ws) ->
                match ws |> List.except prev with
                | [ Z z ] -> (w1, Z(z - 1)) :: result, ws
                | _ -> failwith "Invalid output")
            ([], [])
        |> fst

    let circuit = {
        circuit with
            Gates =
                wireMapping
                |> List.fold
                    (fun gates (w1, w2) ->
                        let g1, g2 = gates[w1], gates[w2]

                        gates
                        |> Map.add w2 { g1 with Output = w2 }
                        |> Map.add w1 { g2 with Output = w1 })
                    circuit.Gates
    }

    let expected = circuit.X + circuit.Y

    let x, y =
        (expected ^^^ evaluate circuit).ToString("b").Replace("1", "").Length
        |> fun n -> X n, Y n

    let wireMapping, gates =
        circuit.Gates
        |> Map.values
        |> Seq.filter (fun g ->
            g.Inputs |> Array.contains x && g.Inputs |> Array.contains y)
        |> Seq.toList
        |> function
            | [ g1; g2 ] ->
                let w1, w2 = g1.Output, g2.Output

                (w1, w2) :: wireMapping,
                circuit.Gates
                |> Map.add w2 { g1 with Output = w2 }
                |> Map.add w1 { g2 with Output = w1 }
            | _ -> failwith "Invalid gates"

    let circuit = { circuit with Gates = gates }
    let z = evaluate circuit

    if z <> expected then
        failwithf "Invalid solution: %d" z

    wireMapping
    |> List.unzip
    ||> (@)
    |> List.map (function
        | X x -> sprintf "x%2d" x
        | Y y -> sprintf "y%2d" y
        | Z z -> sprintf "z%2d" z
        | Inner wire -> wire)
    |> List.sort
    |> Utils.String.join ","

let part1 input = input |> parse |> evaluate

let part2 input = input |> parse |> fixGates

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 49430469426918UL solution1
Utils.Test.run "Part 2" "fbq,pbv,qff,qnw,qqp,z16,z23,z36" solution2

#load "_benchmark.fsx"
