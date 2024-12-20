#load "Utils.fsx"

type Pos = int * int
type Dist2EndMap = System.Collections.Generic.Dictionary<Pos, int>
type Jump = Pos * Pos

type Track = {
    Start: Pos
    End: Pos
    Dist2End: Dist2EndMap
    Jumps: Jump seq
}

let parse maxDuration lines =
    let grid = lines |> Array.map Utils.String.toCharArray

    let isWall (x, y) =
        grid
        |> Array.tryItem y
        |> Option.bind (fun row -> row |> Array.tryItem x)
        |> Option.map ((=) '#')
        |> Option.defaultValue true

    let fields =
        grid
        |> Array.mapi (fun y row -> row |> Array.mapi (fun x c -> c, (x, y)))
        |> Array.concat

    let start = fields |> Array.find (fun (c, _) -> c = 'S') |> snd
    let end' = fields |> Array.find (fun (c, _) -> c = 'E') |> snd

    let dist2End = Dist2EndMap()

    let next (x, y) =
        [ x, y - 1; x, y + 1; x - 1, y; x + 1, y ]
        |> Seq.filter (fun p ->
            not <| isWall p && not <| dist2End.ContainsKey p)
        |> Seq.tryExactlyOne

    let track =
        Seq.unfold
            (fun (current, steps) ->
                current
                |> Option.map (fun pos ->
                    dist2End.Add(pos, steps)
                    pos, (next pos, steps + 1)))
            (Some end', 0)
        |> Seq.toArray

    let cheats (x, y) =
        seq {
            for duration in 2..maxDuration do
                for d in 0..duration do
                    let dx, dy = d, duration - d

                    match dx, dy with
                    | 0, _ ->
                        yield x, y + dy
                        yield x, y - dy
                    | _, 0 ->
                        yield x + dx, y
                        yield x - dx, y
                    | _ ->
                        yield x + dx, y + dy
                        yield x + dx, y - dy
                        yield x - dx, y + dy
                        yield x - dx, y - dy
        }
        |> Seq.filter (fun p -> not <| isWall p)
        |> Seq.map (fun p -> (x, y), p)

    {
        Start = start
        End = end'
        Dist2End = dist2End
        Jumps = track |> Seq.collect cheats
    }

let distance ((x, y), (x', y')) = abs (x - x') + abs (y - y')

let countCheatsSaving pred track =
    let saving (from, to') =
        track.Dist2End[from] - track.Dist2End[to'] - distance (from, to')

    track.Jumps |> Seq.map saving |> Seq.filter pred |> Seq.length

let part1 input =
    input |> parse 2 |> countCheatsSaving (fun s -> s >= 100)

let part2 input =
    input |> parse 20 |> countCheatsSaving (fun s -> s >= 100)

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 1415 solution1
Utils.Test.run "Part 2" 1022577 solution2

#load "_benchmark.fsx"
