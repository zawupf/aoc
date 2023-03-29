module Day21

open Utils

type Item = {
    Name: string
    Cost: int
    Damage: int
    Armor: int
}

type Weapon = Item
type Armor = Item
type Ring = Item

type Shop = {
    Weapons: Weapon list
    Armor: Armor list
    Rings: Ring list
}

module Shop =
    let equipments shop =
        let weaponsList = shop.Weapons
        let armorList = [] :: (shop.Armor |> List.map List.singleton)

        let ringsList =
            shop.Rings
            |> (fun rings ->
                let rec loop acc rings =
                    match rings with
                    | [] -> acc
                    | r :: rs ->
                        let acc' =
                            [ r ]
                            :: (rs
                                |> List.fold
                                    (fun acc'' r' -> [ r; r' ] :: acc'')
                                    acc)

                        loop acc' rs

                loop [ [] ] rings)

        seq {
            for weapon in weaponsList do
                for armor in armorList do
                    for rings in ringsList do
                        yield weapon :: (armor @ rings)
        }

    let parse lines =
        let itemPattern = @"^(.+?)\s+(\d+)\s+(\d+)\s+(\d+)$"

        lines
        |> Seq.fold
            (fun (category, shop) line ->
                match line with
                | Regex @"^(Weapons|Armor|Rings):" [ category ] ->
                    category, shop
                | Regex itemPattern [ name; Int cost; Int damage; Int armor ] ->
                    match category with
                    | "Weapons" ->
                        category,
                        {
                            shop with
                                Weapons =
                                    {
                                        Name = name
                                        Cost = cost
                                        Damage = damage
                                        Armor = armor
                                    }
                                    :: shop.Weapons
                        }
                    | "Armor" ->
                        category,
                        {
                            shop with
                                Armor =
                                    {
                                        Name = name
                                        Cost = cost |> int
                                        Damage = damage |> int
                                        Armor = armor |> int
                                    }
                                    :: shop.Armor
                        }
                    | "Rings" ->
                        category,
                        {
                            shop with
                                Rings =
                                    {
                                        Name = name
                                        Cost = cost |> int
                                        Damage = damage |> int
                                        Armor = armor |> int
                                    }
                                    :: shop.Rings
                        }
                    | _ -> category, shop
                | _ -> category, shop)
            ("", { Weapons = []; Armor = []; Rings = [] })
        |> snd

type Player = {
    Name: string
    Items: Item list
    HitPoints: int
} with

    member this.Damage =
        this.Items |> List.fold (fun damage item -> damage + item.Damage) 0

    member this.Armor =
        this.Items |> List.fold (fun armor item -> armor + item.Armor) 0

    member this.Cost =
        this.Items |> List.fold (fun cost item -> cost + item.Cost) 0

module Player =
    let takeDamageFrom (enemy: Player) (player: Player) =
        let damage = max 1 (enemy.Damage - player.Armor)

        {
            player with
                HitPoints = player.HitPoints - damage
        }

    let isWinningAgainst (enemy: Player) (player: Player) =
        let hitCount (attacker: Player) (defender: Player) =
            let damage = max 1 (attacker.Damage - defender.Armor)

            defender.HitPoints / damage
            + if defender.HitPoints % damage = 0 then 0 else 1

        let enemyHitCount = hitCount enemy player
        let playerHitCount = hitCount player enemy
        playerHitCount <= enemyHitCount

let cheapestWinningEquipmentCost boss shop =
    shop
    |> Shop.equipments
    |> Seq.map (fun items -> {
        Name = "Player"
        HitPoints = 100
        Items = items
    })
    |> Seq.filter (fun player -> player |> Player.isWinningAgainst boss)
    |> Seq.map (fun player -> player.Cost)
    |> Seq.min

let mostExpensiveLoosingEquipmentCost boss shop =
    shop
    |> Shop.equipments
    |> Seq.map (fun items -> {
        Name = "Player"
        HitPoints = 100
        Items = items
    })
    |> Seq.filter (fun player -> player |> Player.isWinningAgainst boss |> not)
    |> Seq.map (fun player -> player.Cost)
    |> Seq.max

let shop =
    """
Weapons:    Cost  Damage  Armor
Dagger        8     4       0
Shortsword   10     5       0
Warhammer    25     6       0
Longsword    40     7       0
Greataxe     74     8       0

Armor:      Cost  Damage  Armor
Leather      13     0       1
Chainmail    31     0       2
Splintmail   53     0       3
Bandedmail   75     0       4
Platemail   102     0       5

Rings:      Cost  Damage  Armor
Damage +1    25     1       0
Damage +2    50     2       0
Damage +3   100     3       0
Defense +1   20     0       1
Defense +2   40     0       2
Defense +3   80     0       3"""
    |> String.split '\n'
    |> Shop.parse

let parseBoss (lines: string[]) =
    let parseInt line =
        match line with
        | Regex @" (\d+)$" [ Int n ] -> n
        | _ -> failwith "Invalid input"

    {
        Name = "Boss"
        HitPoints = parseInt lines.[0]
        Items = [
            {
                Name = "Gear"
                Damage = parseInt lines.[1]
                Armor = parseInt lines.[2]
                Cost = 0
            }
        ]
    }

let boss = readInputLines "21" |> Seq.toArray |> parseBoss

let job1 () =
    shop |> cheapestWinningEquipmentCost boss |> string

let job2 () =
    shop |> mostExpensiveLoosingEquipmentCost boss |> string
