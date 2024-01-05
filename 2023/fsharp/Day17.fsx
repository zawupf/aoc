#load "Utils.fsx"

type Grid = {
    HeatLoss: int array2d
    Width: int
    Height: int
}

type Cursor = {
    X: int
    Y: int
    DX: int
    DY: int
    HeatLoss: int
} with

    member this.Next g =
        let x', y' = this.X + this.DX, this.Y + this.DY

        if x' >= 0 && x' < g.Width && y' >= 0 && y' < g.Height then
            Some {
                this with
                    X = x'
                    Y = y'
                    HeatLoss = this.HeatLoss + g.HeatLoss[y', x']
            }
        else
            None

    member this.WithDX dx = { this with DX = dx; DY = 0 }
    member this.WithDY dy = { this with DX = 0; DY = dy }

let stalkAround lines noStopLimit wobbleLimit =
    let grid =
        lines
        |> Array.map (Utils.String.toCharArray >> Array.map (string >> int))
        |> array2D

    let grid = {
        HeatLoss = grid
        Height = grid |> Array2D.length1
        Width = grid |> Array2D.length2
    }

    let heatLossCache =
        Array3D.create grid.Height grid.Width 2 System.Int32.MaxValue

    let resultHeatLoss i =
        heatLossCache[grid.Height - 1, grid.Width - 1, i]

    let rec walk h cs i (c: Cursor option) =
        let c' = c |> Option.bind _.Next(grid)

        match c' with
        | _ when i = wobbleLimit -> cs
        | None -> cs
        | Some _ when i < noStopLimit -> walk h cs (i + 1) c'
        | Some c when c.HeatLoss >= resultHeatLoss h -> cs
        | Some c when c.HeatLoss >= heatLossCache[c.Y, c.X, h] ->
            walk h cs (i + 1) c'
        | Some c ->
            heatLossCache[c.Y, c.X, h] <- c.HeatLoss

            if c.X = grid.Width - 1 && c.Y = grid.Height - 1 then
                cs
            else if c.DX <> 0 then
                walk h (c.WithDY(1) :: c.WithDY(-1) :: cs) (i + 1) c'
            else
                walk h (c.WithDX(1) :: c.WithDX(-1) :: cs) (i + 1) c'

    let rec loop =
        function
        | [] -> min (resultHeatLoss 0) (resultHeatLoss 1)
        | c :: cs -> loop (walk (if c.DX <> 0 then 0 else 1) cs 0 (Some c))

    loop [
        {
            X = 0
            Y = 0
            DX = 1
            DY = 0
            HeatLoss = 0
        }
        {
            X = 0
            Y = 0
            DX = 0
            DY = 1
            HeatLoss = 0
        }
    ]

let part1 lines = stalkAround lines 0 3

let part2 lines = stalkAround lines 3 10



let testInput1 =
    """
2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533
"""
    |> Utils.String.toLines

let testInput2 =
    """
111111111111
999999999991
999999999991
999999999991
999999999991
"""
    |> Utils.String.toLines

Utils.Test.run "Test part 1" 102 (fun () -> part1 testInput1)
Utils.Test.run "Test part 2 (1/2)" 94 (fun () -> part2 testInput1)
Utils.Test.run "Test part 2 (2/2)" 71 (fun () -> part2 testInput2)


let input = Utils.readInputLines "17"

let getDay17_1 () = part1 input

let getDay17_2 () = part2 input

Utils.Test.run "Part 1" 1128 getDay17_1
Utils.Test.run "Part 2" 1268 getDay17_2
