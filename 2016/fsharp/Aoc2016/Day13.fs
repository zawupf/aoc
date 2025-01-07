module Day13

open System.Collections.Generic

open Utils

let (|Even|Odd|) n =
    match n % 2u with
    | 0u -> Even
    | _ -> Odd

type Pos = uint * uint

type Space =
    | Wall
    | Open

module Space =
    let toChar =
        function
        | Wall -> '#'
        | Open -> '.'

type Area = { MagicNumber: uint; Area: Dictionary<Pos, Space> }

module Area =
    let empty number = { MagicNumber = number; Area = Dictionary<Pos, Space>() }

    let private countBits (number: uint) =
        let rec loop bits number =
            match number with
            | 0u -> bits
            | _ -> loop (bits + (number &&& 1u)) (number >>> 1)

        loop 0u number

    let space pos { MagicNumber = n; Area = area } =
        match area.TryGetValue(pos) with
        | true, space -> space
        | false, _ ->
            let x, y = pos

            let space =
                match
                    (x * x + 3u * x + 2u * x * y + y + y * y + n) |> countBits
                with
                | Even -> Open
                | Odd -> Wall

            area.Add(pos, space)
            space

    let nextPos (x, y) area =
        seq {
            yield x + 1u, y

            if x > 0u then
                yield x - 1u, y

            yield x, y + 1u

            if y > 0u then
                yield x, y - 1u
        }
        |> Seq.filter (fun pos -> area |> space pos = Open)


let keepWalking n pos (steps: Dictionary<Pos, uint>) =
    match steps.TryGetValue(pos) with
    | false, _ ->
        steps.Add(pos, n)
        true
    | true, n' when n < n' ->
        steps.[pos] <- n
        true
    | true, _ -> false

let minSteps from' to' area =
    let x', y' = to'

    let globalStepCount = Dictionary<Pos, uint>()

    let pushHigh = PriorityList.push PriorityList.High
    let pushLow = PriorityList.push PriorityList.Low

    let dist (x, y) =
        let diff a b = if a < b then b - a else a - b
        diff x x' + diff y y'

    let push n pos previous ways =
        if dist pos > dist previous then
            pushLow (n, pos) ways
        else
            pushHigh (n, pos) ways

    let rec loop result ways =
        match ways |> PriorityList.tryPop with
        | None, _ -> result
        | Some(n, pos), ways ->
            match keepWalking n pos globalStepCount with
            | false -> loop result ways
            | true ->
                match pos = to' with
                | true -> loop n ways
                | false ->
                    let ways' =
                        area
                        |> Area.nextPos pos
                        |> Seq.filter (fun next -> n + 1u + dist next < result)
                        |> Seq.fold
                            (fun ways next -> ways |> push (n + 1u) next pos)
                            ways

                    loop result ways'

    loop System.UInt32.MaxValue (PriorityList.empty |> pushLow (0u, from'))

let maxPlaces maxSteps from' area =
    let globalStepCount = Dictionary<Pos, uint>()

    let rec loop ways =
        match ways with
        | [] -> globalStepCount.Keys |> Seq.length
        | (n, pos) :: ways ->
            match keepWalking n pos globalStepCount && n < maxSteps with
            | false -> loop ways
            | true ->
                let ways' =
                    area
                    |> Area.nextPos pos
                    |> Seq.fold (fun ways next -> (n + 1u, next) :: ways) ways

                loop ways'

    loop [ 0u, from' ]

let input = readInputText "13" |> uint

let job1 () = Area.empty input |> minSteps (1u, 1u) (31u, 39u) |> string

let job2 () = Area.empty input |> maxPlaces 50u (1u, 1u) |> string
