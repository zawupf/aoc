module Day22.Tests

open Xunit
open Utils
open Day22

let player = {
    Name = "Player"
    HitPoints = 10
    Damage = 0
    Armor = 0
    Mana = 250
    TotalManaSpent = 0
    Effects = []
}

let boss = {
    Name = "Boss"
    HitPoints = 13
    Damage = 8
    Armor = 0
    Mana = 0
    TotalManaSpent = 0
    Effects = []
}

[<Fact>]
let ``Day22 example 1`` () =
    let game = { Players = player, boss }

    let actions = [|
        CastSpell(Spell.Poison)
        Attack
        CastSpell(MagicMissile)
        Attack
    |]

    let turns =
        actions
        |> Array.scan
            (fun game action -> game |> Game.nextTurn Easy action)
            game

    Assert.Equal(5, turns |> Array.length)

    Assert.Equal<string array>(
        [|
            [|
                "Player has 10 hit points, 0 armor, 250 mana"
                "Boss has 13 hit points"
            |]
            [|
                "Player has 10 hit points, 0 armor, 77 mana"
                "Boss has 13 hit points"
            |]
            [|
                "Player has 2 hit points, 0 armor, 77 mana"
                "Boss has 10 hit points"
            |]
            [|
                "Player has 2 hit points, 0 armor, 24 mana"
                "Boss has 3 hit points"
            |]
            [|
                "Player has 2 hit points, 0 armor, 24 mana"
                "Boss has 0 hit points"
            |]
        |],
        turns |> Array.map Game.dump
    )

    Assert.True(turns.[3] |> Game.isRunning)
    Assert.False(turns.[4] |> Game.isRunning)

[<Fact>]
let ``Day22 example 2`` () =
    let game = {
        Players = player, { boss with HitPoints = 14 }
    }

    let actions = [|
        CastSpell(Spell.Recharge)
        Attack
        CastSpell(Spell.Shield)
        Attack
        CastSpell(Drain)
        Attack
        CastSpell(Spell.Poison)
        Attack
        CastSpell(MagicMissile)
        Attack
    |]

    let turns =
        actions
        |> Array.scan
            (fun game action -> game |> Game.nextTurn Easy action)
            game

    Assert.Equal(11, turns |> Array.length)

    Assert.Equal<string array array>(
        [|
            [|
                "Player has 10 hit points, 0 armor, 250 mana"
                "Boss has 14 hit points"
            |]
            [|
                "Player has 10 hit points, 0 armor, 21 mana"
                "Boss has 14 hit points"
            |]
            [|
                "Player has 2 hit points, 0 armor, 122 mana"
                "Boss has 14 hit points"
            |]
            [|
                "Player has 2 hit points, 7 armor, 110 mana"
                "Boss has 14 hit points"
            |]
            [|
                "Player has 1 hit points, 7 armor, 211 mana"
                "Boss has 14 hit points"
            |]
            [|
                "Player has 3 hit points, 7 armor, 239 mana"
                "Boss has 12 hit points"
            |]
            [|
                "Player has 2 hit points, 7 armor, 340 mana"
                "Boss has 12 hit points"
            |]
            [|
                "Player has 2 hit points, 7 armor, 167 mana"
                "Boss has 12 hit points"
            |]
            [|
                "Player has 1 hit points, 7 armor, 167 mana"
                "Boss has 9 hit points"
            |]
            [|
                "Player has 1 hit points, 0 armor, 114 mana"
                "Boss has 2 hit points"
            |]
            [|
                "Player has 1 hit points, 0 armor, 114 mana"
                "Boss has 0 hit points"
            |]
        |],
        turns |> Array.map Game.dump
    )

    Assert.True(turns.[9] |> Game.isRunning)
    Assert.False(turns.[10] |> Game.isRunning)

[<Fact>]
let ``Day22 Stars`` () =
    try
        Assert.Equal("1269", job1 ())
        Assert.Equal("1309", job2 ())
    with :? System.NotImplementedException ->
        ()
