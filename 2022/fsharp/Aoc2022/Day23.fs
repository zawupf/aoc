module Day23

open Utils

type Direction =
    | North
    | South
    | West
    | East

type Elf = int * int

let private allEmpty elves positions =
    positions |> Seq.exists (fun pos -> elves |> Set.contains pos) |> not

let hasEnoughSpace elves elf =
    let x, y = elf

    seq {
        for dx in -1 .. 1 do
            for dy in -1 .. 1 do
                if dx <> 0 || dy <> 0 then
                    yield x + dx, y + dy
    }
    |> allEmpty elves

let hasSpaceAt elves elf direction =
    let x, y = elf

    match direction with
    | North -> seq { for x in x - 1 .. x + 1 -> x, y - 1 }
    | South -> seq { for x in x - 1 .. x + 1 -> x, y + 1 }
    | West -> seq { for y in y - 1 .. y + 1 -> x - 1, y }
    | East -> seq { for y in y - 1 .. y + 1 -> x + 1, y }
    |> allEmpty elves

let proposeDirection proposals elves elf =
    let direction = proposals |> List.tryFind (hasSpaceAt elves elf)
    direction, elf

let nextRound proposals allElves activeElves =
    activeElves

    // consider new positions
    |> Seq.map (fun elf ->
        match proposeDirection proposals allElves elf with
        | Some direction, elf ->
            let x, y = elf

            match direction with
            | North -> (x, y - 1)
            | South -> (x, y + 1)
            | West -> (x - 1, y)
            | East -> (x + 1, y)
            , elf
        | None, elf -> elf, elf)

    // find collisions
    |> Seq.groupBy fst

    // set new positions when unique
    |> Seq.collect (fun (_, posMap) ->
        match posMap |> Seq.tryExactlyOne with
        | Some(newPos, _) -> Seq.singleton newPos
        | None -> posMap |> Seq.map snd)
    |> Set.ofSeq

let rounds elves =
    ([ North; South; West; East ], elves)
    |> Seq.unfold (fun (proposals, elves) ->
        match
            Set.count elves, elves |> Set.partition (hasEnoughSpace elves)
        with
        | 0, _ -> None
        | _, (_, active) when active.IsEmpty -> Some(elves, ([], Set.empty)) // All elves have enough space
        | _, (inactive, active) ->
            let elves' =
                active |> nextRound proposals elves |> Set.union inactive

            let proposals' = proposals.Tail @ [ proposals.Head ]

            Some(elves, (proposals', elves')))

let minMax elves =
    let xmin, xmax =
        elves
        |> Set.map fst // x
        |> (fun xs -> Set.minElement xs, Set.maxElement xs)

    let ymin, ymax =
        elves
        |> Set.map snd // y
        |> (fun ys -> Set.minElement ys, Set.maxElement ys)

    (xmin, xmax), (ymin, ymax)

let countEmptyTiles elves =
    let (xmin, xmax), (ymin, ymax) = minMax elves
    (xmax - xmin + 1) * (ymax - ymin + 1) - Set.count elves

let parse lines =
    lines
    |> Seq.indexed
    |> Seq.collect (fun (y, line) ->
        line
        |> Seq.indexed
        |> Seq.choose (fun (x, c) ->
            match c with
            | '#' -> Some(x, y)
            | _ -> None))
    |> Set.ofSeq

let dump elves =
    let (xmin, xmax), (ymin, ymax) = minMax elves

    [ for y in ymin..ymax do
          for x in xmin..xmax -> x, y ]
    |> List.iter (fun (x, y) ->
        if x = xmin then
            printfn ""

        if elves |> Set.contains (x, y) then '#' else '.'
        |> printf "%c")

    printfn ""

let input = readInputLines "23"

let job1 () =
    input |> parse |> rounds |> Seq.item 10 |> countEmptyTiles |> string

let job2 () =
    input |> parse |> rounds |> Seq.length |> string
