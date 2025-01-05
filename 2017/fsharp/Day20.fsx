#load "Utils.fsx"
open Utils

type Particle = {
    Position: int[]
    Velocity: int[]
    Acceleration: int[]
}

let parseParticle =
    let pattern = @"p=<(.+?)>, v=<(.+?)>, a=<(.+?)>"

    function
    | Regex pattern [ position; velocity; acceleration ] -> {
        Position = position |> String.parseInts ","
        Velocity = velocity |> String.parseInts ","
        Acceleration = acceleration |> String.parseInts ","
      }
    | line -> failwithf "Invalid input: %s" line

let parse input = input |> Array.map parseParticle

let positionAtTime t (particle: Particle) =
    // p_tn = p_t0 + n * v_t0 + n * (n + 1) / 2 * a
    let {
            Position = p
            Velocity = v
            Acceleration = a
        } =
        particle

    Array.map3 (fun p v a -> p + t * v + t * (t + 1) / 2 * a) p v a

let solveCollision i1 i2 particles =
    let p1 = particles |> Array.item i1
    let p2 = particles |> Array.item i2

    let a = p2.Acceleration[0] - p1.Acceleration[0]
    let b = 2 * (p2.Velocity[0] - p1.Velocity[0]) + a
    let c = 2 * (p2.Position[0] - p1.Position[0])
    let d = b * b - 4 * a * c

    match a, b, c, sign d with
    | 0, 0, _, _ -> Array.empty
    | 0, _, _, _ ->
        let t = -c / b
        let p = positionAtTime t p1

        [|
            if t >= 0 && positionAtTime t p1 = positionAtTime t p2 then
                yield (t, p), i1
                yield (t, p), i2
        |]
    | _, _, _, -1 -> Array.empty
    | _, _, _, _ ->
        let sqr_d = d |> float |> sqrt |> round |> int

        if sqr_d * sqr_d <> d then
            Array.empty
        else
            let t1 = (-b + sqr_d) / (2 * a)
            let t2 = (-b - sqr_d) / (2 * a)
            let p' = positionAtTime t1 p1
            let p'' = positionAtTime t2 p1

            [|
                if t1 >= 0 && p' = positionAtTime t1 p2 then
                    yield (t1, p'), i1
                    yield (t1, p'), i2
                if t2 >= 0 && p'' = positionAtTime t2 p2 then
                    yield (t2, p''), i1
                    yield (t2, p''), i2
            |]

let part1 input =
    input
    |> parse
    |> Array.indexed
    |> Array.minBy (snd >> _.Acceleration >> Array.sumBy abs)
    |> fst

let part2 input =
    let particles = input |> parse

    [|
        for i = 0 to particles.Length - 1 do
            for j = i + 1 to particles.Length - 1 do
                yield! solveCollision i j particles
    |]
    |> Array.groupBy fst
    |> Array.map (fun (key, particles) ->
        key, particles |> Array.map snd |> Array.distinct)
    |> Array.sortBy fst
    |> Array.fold
        (fun (destroyed) (_, particles) ->
            let particles = particles |> Array.except destroyed

            match particles.Length with
            | 0
            | 1 -> destroyed
            | _ -> destroyed |> Array.append particles)
        Array.empty
    |> _.Length
    |> (-) particles.Length

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>
p=< 4,0,0>, v=< 0,0,0>, a=<-2,0,0>
"""
        """
p=<-6,0,0>, v=< 3,0,0>, a=< 0,0,0>
p=<-4,0,0>, v=< 2,0,0>, a=< 0,0,0>
p=<-2,0,0>, v=< 1,0,0>, a=< 0,0,0>
p=< 3,0,0>, v=<-1,0,0>, a=< 0,0,0>
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" 0 (fun () -> part1 testInput[0])
Test.run "Test 2" 1 (fun () -> part2 testInput[1])

Test.run "Part 1" 300 solution1
Test.run "Part 2" 502 solution2

#load "_benchmark.fsx"
