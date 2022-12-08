module Day12

open System
open Utils
open System.Text.RegularExpressions

[<AutoOpen>]
module MoonTypes =
    type Position = Pos of int * int * int

    type Velocity = Vel of int * int * int

    type Moon = { pos: Position; vel: Velocity }

module Moon =
    let parse string =
        let m = Regex.Match(string, @"^\<x=(-?\d+), y=(-?\d+), z=(-?\d+)\>$")

        let coords =
            m.Groups
            |> Seq.skip 1
            |> Seq.take 3
            |> Seq.map (fun group -> Int32.Parse(group.Value))
            |> Seq.toArray

        { pos = Pos(coords.[0], coords.[1], coords.[2])
          vel = Vel(0, 0, 0) }

    let toString moon = sprintf "Moon %A" moon

    let private updateVelocity moons moon =
        let { pos = Pos(x, y, z)
              vel = Vel(vx, vy, vz) } =
            moon

        let (Vel(dx, dy, dz)) =
            moons
            |> Seq.fold
                (fun (Vel(vx, vy, vz)) { pos = Pos(mx, my, mz) } ->
                    Vel(
                        vx + compare mx x,
                        vy + compare my y,
                        vz + compare mz z
                    ))
                (Vel(0, 0, 0))

        { moon with
            vel = Vel(vx + dx, vy + dy, vz + dz) }

    let private updatePosition moon =
        let { pos = Pos(x, y, z)
              vel = Vel(vx, vy, vz) } =
            moon

        { moon with
            pos = Pos(x + vx, y + vy, z + vz) }

    let rec applyGravity moons =
        seq {
            yield moons

            yield!
                moons
                |> List.map ((updateVelocity moons) >> updatePosition)
                |> applyGravity
        }

    let potentialEnergy { pos = Pos(x, y, z) } = (abs x) + (abs y) + (abs z)

    let kineticEnergy { vel = Vel(vx, vy, vz) } = (abs vx) + (abs vy) + (abs vz)

    let totalEnergy moon =
        potentialEnergy moon * kineticEnergy moon

    let only axis moon =
        let { pos = Pos(x, y, z) } = moon

        { moon with
            pos =
                match axis with
                | 'x' -> Pos(x, 0, 0)
                | 'y' -> Pos(0, y, 0)
                | 'z' -> Pos(0, 0, z)
                | _ -> failwith "Invalid axis" }

    let cycleCount moons =
        (moons |> applyGravity |> Seq.skip 1 |> Seq.findIndex ((=) moons)) + 1

    let totalCycleCount x y z =
        [ x; y; z ] |> List.map int64 |> List.LCM

let job1 () =
    readInputLines "12"
    |> List.ofSeq
    |> List.map Moon.parse
    |> Moon.applyGravity
    |> Seq.item 1000
    |> List.sumBy Moon.totalEnergy
    |> string

let job2 () =
    let moons = readInputLines "12" |> List.ofSeq |> List.map Moon.parse

    let x = moons |> List.map (Moon.only 'x') |> Moon.cycleCount

    let y = moons |> List.map (Moon.only 'y') |> Moon.cycleCount

    let z = moons |> List.map (Moon.only 'z') |> Moon.cycleCount

    // sprintf "%A" (x, y, z)
    Moon.totalCycleCount x y z |> string
