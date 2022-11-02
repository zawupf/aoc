module Day07

open Utils

type Signal = uint16
type WireId = string

type Input =
    | InSignal of Signal
    | InWireId of WireId

type Gate =
    | SIGNAL of Input
    | AND of Input * Input
    | OR of Input * Input
    | NOT of Input
    | LSHIFT of Input * int
    | RSHIFT of Input * int

type Circuit = Map<WireId, Gate>

module Circuit =
    let empty = Map.empty<WireId, Gate>

    let add wire circuit =
        let toInput s =
            match s with
            | Regex @"^\d+$" [] -> s |> uint16 |> InSignal
            | _ -> s |> InWireId

        let wireId, input =
            match wire with
            | Regex @"^(\d+|\w+) -> (\w+)$" [ signal; id ] ->
                id, signal |> toInput |> SIGNAL
            | Regex @"^(\d+|\w+) AND (\d+|\w+) -> (\w+)$" [ in1; in2; id ] ->
                id, (in1 |> toInput, in2 |> toInput) |> AND
            | Regex @"^(\d+|\w+) OR (\d+|\w+) -> (\w+)$" [ in1; in2; id ] ->
                id, (in1 |> toInput, in2 |> toInput) |> OR
            | Regex @"NOT (\d+|\w+) -> (\w+)$" [ input; id ] ->
                id, input |> toInput |> NOT
            | Regex @"^(\d+|\w+) LSHIFT (\d+) -> (\w+)$" [ input; shift; id ] ->
                id, (input |> toInput, shift |> int) |> LSHIFT
            | Regex @"^(\d+|\w+) RSHIFT (\d+) -> (\w+)$" [ input; shift; id ] ->
                id, (input |> toInput, shift |> int) |> RSHIFT
            | _ -> failwithf "Invalid wire: %s" wire

        circuit |> Map.add wireId input

    let rec resolve id circuit =
        let resolveSignal input circuit =
            match input with
            | InSignal signal -> signal, circuit
            | InWireId wireId ->
                let signal, circuit = resolve wireId circuit

                match signal with
                | InSignal signal -> signal, circuit
                | InWireId wireId ->
                    failwithf
                        "Could not fully resolve signal of wire '%s'"
                        wireId

        match circuit |> Map.tryFind id with
        | None -> failwithf "Unknown wire: '%s'" id
        | Some gate ->
            match gate with
            | SIGNAL signal ->
                match signal with
                | InSignal _ -> signal, circuit
                | InWireId id1 ->
                    let signal, circuit = resolve id1 circuit
                    signal, circuit |> Map.add id (signal |> SIGNAL)
            | AND(in1, in2) ->
                let signal1, circuit = resolveSignal in1 circuit
                let signal2, circuit = resolveSignal in2 circuit
                let signal = signal1 &&& signal2

                signal |> InSignal,
                circuit |> Map.add id (signal |> InSignal |> SIGNAL)
            | OR(in1, in2) ->
                let signal1, circuit = resolveSignal in1 circuit
                let signal2, circuit = resolveSignal in2 circuit
                let signal = signal1 ||| signal2

                signal |> InSignal,
                circuit |> Map.add id (signal |> InSignal |> SIGNAL)
            | NOT in1 ->
                let signal, circuit = resolveSignal in1 circuit
                let signal = ~~~signal

                signal |> InSignal,
                circuit |> Map.add id (signal |> InSignal |> SIGNAL)
            | LSHIFT(in1, n) ->
                let signal, circuit = resolveSignal in1 circuit
                let signal = signal <<< n

                signal |> InSignal,
                circuit |> Map.add id (signal |> InSignal |> SIGNAL)
            | RSHIFT(in1, n) ->
                let signal, circuit = resolveSignal in1 circuit
                let signal = signal >>> n

                signal |> InSignal,
                circuit |> Map.add id (signal |> InSignal |> SIGNAL)

    let run circuit =
        circuit
        |> Map.keys
        |> Seq.fold (fun circuit id -> resolve id circuit |> snd) circuit

    let get id circuit =
        match resolve id circuit |> fst with
        | InSignal signal -> signal
        | InWireId wireId ->
            failwithf "Could not fully resolve signal of wire '%s'" wireId

let input = readInputLines "07"

let circuit =
    input
    |> Seq.fold (fun circuit line -> circuit |> Circuit.add line) Circuit.empty


let job1 () = circuit |> Circuit.get "a" |> string

let job2 () =
    let a = circuit |> Circuit.get "a"

    circuit |> Circuit.add (sprintf "%d -> b" a) |> Circuit.get "a" |> string
