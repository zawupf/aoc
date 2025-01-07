module Day11

open System.Collections.Generic
open Utils

module Bits =
    let inline value i = 1uy <<< i
    let inline isZero i bits = (bits >>> i) &&& 1uy = 0uy
    let inline isOne i bits = (bits >>> i) &&& 1uy <> 0uy

    let inline countOnes bits =
        { 0..7 } |> Seq.sumBy (fun i -> (bits >>> i) &&& 1uy)

    let map (mapping: string seq) keys =
        keys
        |> Seq.fold
            (fun bits key ->
                let i = mapping |> Seq.findIndex ((=) key)
                bits ||| (value i))
            0uy

    let join (gens: uint8) (mics: uint8) = uint16 gens + (uint16 mics <<< 8)

    let split (bits: uint16) = bits |> uint8, (bits >>> 8) |> uint8

    let makeFloor mapping (generators, microchips) =
        join (generators |> map mapping) (microchips |> map mapping)

    let makeFloors mapping floorsData =
        floorsData
        |> Seq.map (makeFloor mapping)
        |> Seq.fold
            (fun (floors, i) floor ->
                floors ||| (uint64 floor <<< (i * 16)), i + 1)
            (0UL, 0)
        |> fst

    let floor i (floors: uint64) = (floors >>> (i * 16)) |> uint16

    let allCargos i target floors =
        let with_gens g = join g 0uy
        let with_mics m = join 0uy m

        let bitValues toCargo bits = [|
            for i in 0..7 do
                if isOne i bits then
                    yield i |> value |> toCargo
        |]

        let bitValuePairs toCargo bits = [|
            for i in 0..6 do
                if isOne i bits then
                    for j in i + 1 .. 7 do
                        if isOne j bits then
                            yield (value i + value j) |> toCargo
        |]

        let bitValueMixedPairs gens mics = [|
            for i in 0..7 do
                if isOne i gens then
                    for j in 0..7 do
                        if isOne j mics then
                            yield join (value i) (value j)
        |]

        let gens, mics = floor i floors |> split

        if target > i then
            Array.concat [
                bitValueMixedPairs gens mics
                bitValuePairs with_gens gens
                bitValuePairs with_mics mics
                bitValues with_gens gens
                bitValues with_mics mics
            ]
        else
            // This assumption might be wrong 🤔
            Array.concat [ bitValues with_gens gens; bitValues with_mics mics ]

    let move (cargo: uint16) from' to' floors =
        let addMask = uint64 cargo <<< (to' * 16)
        let removeMask = ~~~(uint64 cargo <<< (from' * 16))
        (floors ||| addMask) &&& removeMask

    let isStable floors =
        let gens = floors &&& 0x00FF_00FF_00FF_00FFUL
        let mics = (floors &&& 0xFF00_FF00_FF00_FF00UL) >>> 8
        let both = gens &&& mics

        { 0..3 }
        |> Seq.forall (fun i ->
            let b = floor i both
            b = floor i gens || b = floor i mics)

    let hash floors =
        let gens = floors &&& 0x00FF_00FF_00FF_00FFUL
        let mics = (floors &&& 0xFF00_FF00_FF00_FF00UL) >>> 8
        let both = gens &&& mics
        let single = gens ^^^ mics

        { 0..3 }
        |> Seq.fold
            (fun hash i ->
                let b = both |> floor i |> uint8 |> countOnes
                let s = single |> floor i |> uint8 |> countOnes
                let h = uint64 (join b s) <<< (i * 16)
                hash + h)
            0x0UL

    let isFinished floors = floors &&& 0x0000_FFFF_FFFF_FFFFUL = 0UL

let adjacentFloors elevator =
    match elevator with
    | 0 -> [ 1 ]
    | 1 -> [ 0; 2 ]
    | 2 -> [ 1; 3 ]
    | 3 -> [ 2 ]
    | fi -> failwith $"Floor index %d{fi} out of range"

type State = int * uint64

let minMove (initialState: State) =
    let globalHash = Dictionary<int * uint64, int>()
    globalHash.Add((initialState |> fst, initialState |> snd |> Bits.hash), 0)

    let testGlobalHash hash steps =
        match globalHash.TryGetValue(hash) with
        | false, _ ->
            globalHash.[hash] <- steps
            true
        | true, n when steps < n ->
            globalHash.[hash] <- steps
            true
        | true, _ -> false

    let rec loop result moves =
        match moves with
        | [] -> result
        | ((elevator, floors), historyLength) :: moves ->
            let resultLength =
                result |> Option.defaultValue System.Int32.MaxValue

            match floors |> Bits.isFinished with
            | true ->
                match historyLength with
                | l when resultLength < l -> loop result moves
                | _ -> loop (Some historyLength) moves
            | false ->
                let moves' =
                    (adjacentFloors elevator
                     |> List.fold
                         (fun moves target ->
                             Bits.allCargos elevator target floors
                             |> Seq.map (fun cargo ->
                                 floors |> Bits.move cargo elevator target)
                             |> Seq.filter (fun floors ->
                                 let hash = target, floors |> Bits.hash

                                 Bits.isStable floors
                                 && historyLength + 1 < resultLength
                                 && testGlobalHash hash (historyLength + 1))
                             |> Seq.fold
                                 (fun moves floors ->
                                     let state' = target, floors
                                     let history' = historyLength + 1
                                     (state', history') :: moves)
                                 moves)
                         moves)

                loop result moves'

    loop None [ initialState, 0 ]

let minStepCount floors = minMove (0, floors) |> Option.get

let mapping = [|
    "Thulium"
    "Plutonium"
    "Strontium"
    "Promethium"
    "Ruthenium"
|]

let floors = [|
    [ "Thulium"; "Plutonium"; "Strontium" ], [ "Thulium" ]
    [], [ "Plutonium"; "Strontium" ]
    [ "Promethium"; "Ruthenium" ], [ "Promethium"; "Ruthenium" ]
    [], []
|]

let mapping2 = [|
    "Thulium"
    "Plutonium"
    "Strontium"
    "Promethium"
    "Ruthenium"
    "Elerium"
    "Dilithium"
|]

let floors2 = [|
    [ "Thulium"; "Plutonium"; "Strontium"; "Elerium"; "Dilithium" ],
    [ "Thulium"; "Elerium"; "Dilithium" ]
    [], [ "Plutonium"; "Strontium" ]
    [ "Promethium"; "Ruthenium" ], [ "Promethium"; "Ruthenium" ]
    [], []
|]

let job1 () = floors |> Bits.makeFloors mapping |> minStepCount |> string

let job2 () = floors2 |> Bits.makeFloors mapping2 |> minStepCount |> string
