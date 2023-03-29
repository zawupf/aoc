module Day22

open Utils

[<AutoOpen>]
module Types =
    type Difficulty =
        | Easy
        | Hard

    type Spell =
        | MagicMissile
        | Drain
        | Shield
        | Poison
        | Recharge

    type Effect =
        | Shield of int
        | Poison of int
        | Recharge of int

    module Effect =
        let duration =
            function
            | Shield d
            | Poison d
            | Recharge d -> d

        let tick =
            function
            | Shield d -> Shield(d - 1)
            | Poison d -> Poison(d - 1)
            | Recharge d -> Recharge(d - 1)

    type Action =
        | CastSpell of Spell
        | Attack

    type Player = {
        Name: string
        HitPoints: int
        Damage: int
        Armor: int
        Mana: int
        TotalManaSpent: int
        Effects: Effect list
    }

    module Player =
        let dump player =
            let {
                    Name = name
                    HitPoints = hp
                    Armor = armor
                    Mana = mana
                } =
                player

            match name with
            | "Boss" -> $"%s{name} has %d{hp} hit points"
            | _ ->
                $"%s{name} has %d{hp} hit points, %d{armor} armor, %d{mana} mana"

        let isAlive player = player.HitPoints > 0

        let handleDifficulty difficulty player =
            match difficulty with
            | Easy -> player
            | Hard ->
                match player.Name with
                | "Boss" -> player
                | _ -> {
                    player with
                        HitPoints = player.HitPoints - 1
                  }

        let handleEffects player =
            let remove effect player =
                match effect with
                | Shield _ -> { player with Armor = player.Armor - 7 }
                | Poison _
                | Recharge _ -> player

            let apply effect player =
                let player' =
                    match effect with
                    | Shield _ -> player
                    | Poison _ -> {
                        player with
                            HitPoints = max 0 (player.HitPoints - 3)
                      }
                    | Recharge _ -> { player with Mana = player.Mana + 101 }

                match (effect |> Effect.tick) with
                | effect when effect |> Effect.duration > 0 -> {
                    player' with
                        Effects = effect :: player'.Effects
                  }
                | _ -> remove effect player'

            player.Effects
            |> List.fold
                (fun player effect ->
                    if player |> isAlive then
                        player |> apply effect
                    else
                        player)
                { player with Effects = [] }

        let hasEnoughMana spell player =
            match spell with
            | MagicMissile -> player.Mana >= 53
            | Drain -> player.Mana >= 73
            | Spell.Shield -> player.Mana >= 113
            | Spell.Poison -> player.Mana >= 173
            | Spell.Recharge -> player.Mana >= 229

        let hasBlockingEffect spell player boss =
            match spell with
            | MagicMissile
            | Drain -> false
            | Spell.Shield ->
                player.Effects
                |> List.exists (function
                    | Shield _ -> true
                    | _ -> false)
            | Spell.Recharge ->
                player.Effects
                |> List.exists (function
                    | Recharge _ -> true
                    | _ -> false)
            | Spell.Poison ->
                boss.Effects
                |> List.exists (function
                    | Poison _ -> true
                    | _ -> false)

        let canCastSpell spell player boss =
            hasEnoughMana spell player
            && hasBlockingEffect spell player boss |> not

        let handleAction action players =
            let player, enemy = players

            match action with
            | CastSpell spell ->
                if canCastSpell spell player enemy then
                    match spell with
                    | MagicMissile ->
                        {
                            player with
                                Mana = player.Mana - 53
                                TotalManaSpent = player.TotalManaSpent + 53
                        },
                        {
                            enemy with
                                HitPoints = max 0 (enemy.HitPoints - 4)
                        }
                    | Drain ->
                        {
                            player with
                                Mana = player.Mana - 73
                                TotalManaSpent = player.TotalManaSpent + 73
                                HitPoints = player.HitPoints + 2
                        },
                        {
                            enemy with
                                HitPoints = max 0 (enemy.HitPoints - 2)
                        }
                    | Spell.Shield ->
                        {
                            player with
                                Mana = player.Mana - 113
                                TotalManaSpent = player.TotalManaSpent + 113
                                Armor = player.Armor + 7
                                Effects = Shield(6) :: player.Effects
                        },
                        enemy
                    | Spell.Poison ->
                        {
                            player with
                                Mana = player.Mana - 173
                                TotalManaSpent = player.TotalManaSpent + 173
                        },
                        {
                            enemy with
                                Effects = Poison(6) :: enemy.Effects
                        }
                    | Spell.Recharge ->
                        {
                            player with
                                Mana = player.Mana - 229
                                TotalManaSpent = player.TotalManaSpent + 229
                                Effects = Recharge(5) :: player.Effects
                        },
                        enemy
                else
                    { player with HitPoints = 0 }, enemy
            // else if
            //     player.Mana < 53
            //     && hasBlockingEffect Spell.Recharge player enemy |> not
            //     && hasBlockingEffect Spell.Poison enemy player |> not
            // then
            //     { player with HitPoints = 0 }, enemy
            // else
            //     players
            | Attack ->
                let damage = max 1 (player.Damage - enemy.Armor)

                player,
                {
                    enemy with
                        HitPoints = max 0 (enemy.HitPoints - damage)
                }

    type Game = { Players: Player * Player }

    module Game =
        let dump game =
            let p1, p2 = game.Players

            match p1.Name with
            | "Boss" -> [| p2; p1 |]
            | _ -> [| p1; p2 |]
            |> Array.map Player.dump

        let isRunning game =
            let p1, p2 = game.Players
            p1 |> Player.isAlive && p2 |> Player.isAlive

        let winner game =
            let p1, p2 = game.Players

            if game |> isRunning |> not then
                Some(if p1 |> Player.isAlive then p1 else p2)
            else
                None

        let (>=>) game reducer =
            if game |> isRunning then game |> reducer else game

        let handleEffects game = {
            game with
                Players =
                    (game.Players |> fst |> Player.handleEffects,
                     game.Players |> snd |> Player.handleEffects)
        }

        let handleAction action game = {
            game with
                Players = Player.handleAction action game.Players
        }

        let handleDifficulty difficulty game = {
            game with
                Players =
                    (game.Players |> fst |> Player.handleDifficulty difficulty,
                     game.Players |> snd |> Player.handleDifficulty difficulty)
        }


        let swapPlayers game =
            let p1, p2 = game.Players
            { game with Players = p2, p1 }

        let nextTurn difficulty action game =
            game
            >=> handleDifficulty difficulty
            >=> handleEffects
            >=> handleAction action
            >=> swapPlayers

        let nextRound difficulty action game =
            game >=> nextTurn difficulty action >=> nextTurn difficulty Attack

let play difficulty game =
    let totalManaSpent game =
        let p1, p2 = game.Players

        let player = if p1.Name <> "Boss" then p1 else p2
        player.TotalManaSpent

    let rec loop games result =
        match games with
        | [] -> result
        | game :: games' ->
            let m = game |> totalManaSpent

            match game |> Game.winner with
            | None ->
                if m < result - 53 then
                    loop
                        ([
                            MagicMissile
                            Drain
                            Spell.Shield
                            Spell.Poison
                            Spell.Recharge
                         ]
                         |> List.fold
                             (fun games'' spell ->
                                 (game
                                  |> Game.nextRound
                                      difficulty
                                      (CastSpell(spell)))
                                 :: games'')
                             games')
                        result
                else
                    loop games' result
            | Some winner when winner.Name = "Boss" -> loop games' result
            | Some _ -> loop games' (min m result)

    loop [ game ] System.Int32.MaxValue

let parseBoss (lines: string[]) =
    let parseInt line =
        match line with
        | Regex @" (\d+)$" [ Int n ] -> n
        | _ -> failwith "Invalid input"

    {
        Name = "Boss"
        HitPoints = parseInt lines.[0]
        Damage = parseInt lines.[1]
        Armor = 0
        Mana = 0
        TotalManaSpent = 0
        Effects = []
    }

let boss = readInputLines "22" |> Seq.toArray |> parseBoss

let player = {
    Name = "Player"
    HitPoints = 50
    Damage = 0
    Armor = 0
    Mana = 500
    TotalManaSpent = 0
    Effects = []
}

let job1 () =
    { Players = player, boss } |> play Easy |> string

let job2 () =
    { Players = player, boss } |> play Hard |> string
