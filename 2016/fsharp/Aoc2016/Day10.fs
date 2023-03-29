module Day10

open Utils

type Target =
    | Robot of int
    | Output of int

let (|Target|_|) (str: string) =
    match str with
    | Regex @"^(bot|output) (\d+)$" [ kind; Int id ] ->
        match kind with
        | "bot" -> Some(Robot id)
        | "output" -> Some(Output id)
        | _ -> None
    | _ -> None

type Bot = {
    Id: int
    Chips: int list
    Targets: Target list
}

type Factory = {
    Bots: Map<int, Bot>
    Outputs: Map<int, int>
}

module Bot =
    let empty = { Id = 0; Chips = []; Targets = [] }

    let get id factory =
        match factory.Bots |> Map.tryFind id with
        | Some bot -> bot, factory
        | None ->
            let bot = { empty with Id = id }

            bot,
            {
                factory with
                    Bots = factory.Bots |> Map.add id bot
            }

    let set bot factory = {
        factory with
            Bots = factory.Bots |> Map.add bot.Id bot
    }

    let remove id factory = {
        factory with
            Bots = factory.Bots |> Map.remove id
    }

    let isActive bot =
        match bot.Chips, bot.Targets with
        | [ _; _ ], [ _; _ ] -> true
        | _ -> false

    let deliver bot factory =
        let deliverChip chip target factory =
            match target with
            | Output id -> {
                factory with
                    Outputs = factory.Outputs |> Map.add id chip
              }
            | Robot id ->
                let bot = factory.Bots |> Map.find id

                factory
                |> set {
                    bot with
                        Chips = chip :: bot.Chips |> List.sort
                }

        match bot with
        | {
              Chips = [ lowChip; highChip ]
              Targets = [ lowTarget; highTarget ]
          } ->
            assert (lowChip < highChip)

            factory
            |> deliverChip lowChip lowTarget
            |> deliverChip highChip highTarget
        | _ -> failwith $"Invalid deliver bot:%A{bot}"


    let handle instruction factory =
        let pattern =
            @"^bot (\d+) gives low to ((?:bot|output) \d+) and high to ((?:bot|output) \d+)$"

        let bot, factory =
            match instruction with
            | Regex @"^value (\d+) goes to bot (\d+)$" [ Int value; Int id ] ->
                let bot, factory = get id factory

                bot,
                factory
                |> set {
                    bot with
                        Chips = value :: bot.Chips |> List.sort
                }
            | Regex pattern [ Int id; Target lowTarget; Target highTarget ] ->
                let bot, factory = get id factory

                bot,
                factory
                |> set {
                    bot with
                        Targets = [ lowTarget; highTarget ]
                }
            | _ -> failwith $"Invalid instruction: %s{instruction}"

        bot, factory

module Factory =
    let empty = {
        Bots = Map.empty
        Outputs = Map.empty
    }

    let init instructions =
        instructions
        |> Array.fold
            (fun (_bot, factory) instruction -> Bot.handle instruction factory)
            (Bot.empty, empty)
        |> snd

    let findActiveBot factory =
        factory.Bots
        |> Map.tryPick (fun _ bot ->
            if bot |> Bot.isActive then Some bot else None)

    let private _run predicate factory =
        let pred = predicate |> Option.defaultValue (fun _ -> false)

        let rec loop result factory =
            match result with
            | Some bot -> bot, factory
            | None ->
                match factory |> findActiveBot with
                | None ->
                    match predicate with
                    | Some _ -> failwith "No bot found and none are active"
                    | None -> Bot.empty, factory
                | Some bot ->
                    if bot |> pred then
                        loop (Some bot) factory
                    else
                        loop
                            None
                            (factory |> Bot.deliver bot |> Bot.remove bot.Id)

        loop None factory

    let runUntil predicate factory = _run (Some predicate) factory

    let run factory = _run None factory |> snd

    let findBot predicate factory =
        factory |> runUntil predicate |> fst |> (fun bot -> bot.Id)

    let getOutputProduct factory =
        let factory = factory |> run

        factory.Outputs
        |> Map.fold
            (fun product id chip ->
                product
                * match id with
                  | 0
                  | 1
                  | 2 -> chip
                  | _ -> 1)
            1

let input = readInputLines "10"

let job1 () =
    input
    |> Factory.init
    |> Factory.findBot (fun bot -> bot.Chips = [ 17; 61 ])
    |> string

let job2 () =
    input |> Factory.init |> Factory.getOutputProduct |> string
