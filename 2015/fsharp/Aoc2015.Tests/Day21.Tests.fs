module Day21.Tests

open Xunit
open Utils
open Day21

[<Fact>]
let ``Day21 parse shop works`` () =
    Assert.Equal<Weapon list>(
        [ { Name = "Greataxe"
            Cost = 74
            Damage = 8
            Armor = 0 }
          { Name = "Longsword"
            Cost = 40
            Damage = 7
            Armor = 0 }
          { Name = "Warhammer"
            Cost = 25
            Damage = 6
            Armor = 0 }
          { Name = "Shortsword"
            Cost = 10
            Damage = 5
            Armor = 0 }
          { Name = "Dagger"
            Cost = 8
            Damage = 4
            Armor = 0 } ],
        shop.Weapons
    )

    Assert.Equal<Armor list>(
        [ { Name = "Platemail"
            Cost = 102
            Damage = 0
            Armor = 5 }
          { Name = "Bandedmail"
            Cost = 75
            Damage = 0
            Armor = 4 }
          { Name = "Splintmail"
            Cost = 53
            Damage = 0
            Armor = 3 }
          { Name = "Chainmail"
            Cost = 31
            Damage = 0
            Armor = 2 }
          { Name = "Leather"
            Cost = 13
            Damage = 0
            Armor = 1 } ],
        shop.Armor
    )

    Assert.Equal<Ring list>(
        [ { Name = "Defense +3"
            Cost = 80
            Damage = 0
            Armor = 3 }
          { Name = "Defense +2"
            Cost = 40
            Damage = 0
            Armor = 2 }
          { Name = "Defense +1"
            Cost = 20
            Damage = 0
            Armor = 1 }
          { Name = "Damage +3"
            Cost = 100
            Damage = 3
            Armor = 0 }
          { Name = "Damage +2"
            Cost = 50
            Damage = 2
            Armor = 0 }
          { Name = "Damage +1"
            Cost = 25
            Damage = 1
            Armor = 0 } ],
        shop.Rings
    )

[<Fact>]
let ``Day21 isWinningAgainst works`` () =
    let player =
        { Name = "Player"
          HitPoints = 8
          Items =
            [ { Name = "Gear"
                Damage = 5
                Armor = 5
                Cost = 0 } ] }

    let boss =
        { Name = "Boss"
          HitPoints = 12
          Items =
            [ { Name = "Gear"
                Damage = 7
                Armor = 2
                Cost = 0 } ] }

    Assert.True(player |> Player.isWinningAgainst boss)

[<Fact>]
let ``Day21 Stars`` () =
    try
        Assert.Equal("78", job1 ())
        Assert.Equal("148", job2 ())
    with :? System.NotImplementedException ->
        ()
