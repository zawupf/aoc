module Day12

open Utils

type Pos = int * int

type Area = {
    current: Pos
    destination: Pos
    grid: int[,]
}

module Area =
    let parse input =
        let mutable current = 0, 0
        let mutable destination = 0, 0

        let grid =
            input
            |> Seq.mapi (fun x line ->
                line
                |> String.toCharArray
                |> Seq.mapi (fun y ->
                    function
                    | 'S' ->
                        current <- (x, y)
                        0
                    | 'E' ->
                        destination <- (x, y)
                        int 'z' - int 'a'
                    | c when c >= 'a' && c <= 'z' -> int c - int 'a'
                    | c -> failwith $"Invalid input: %c{c}"))
            |> array2D

        {
            current = current
            destination = destination
            grid = grid
        }

    let isValidPos (x, y) area =
        x >= 0
        && x < Array2D.length1 area.grid
        && y >= 0
        && y < Array2D.length2 area.grid

    let isReachable (x, y) area =
        let cx, cy = area.current
        isValidPos (x, y) area && area.grid[x, y] - area.grid[cx, cy] <= 1

    let distance (a, b) (x, y) = abs (a - x) + abs (b - y)

    let private _findShortestPath (stepCount: _[,]) queue =
        let rec loop result paths =
            match paths |> PriorityQueue.tryDequeue with
            | None, _ -> result
            | Some(area, count), paths ->
                let cx, cy = area.current

                match stepCount[cx, cy] with
                | _ when count >= result -> loop result paths
                | n when count >= n -> loop result paths
                | _ ->
                    stepCount[cx, cy] <- count

                    match area.current = area.destination with
                    | true -> loop count paths
                    | false ->
                        let (x, y) = area.current

                        let paths' =
                            [ (x - 1, y); (x + 1, y); (x, y - 1); (x, y + 1) ]
                            |> List.filter (fun p -> isReachable p area)
                            |> List.fold
                                (fun paths pos ->
                                    PriorityQueue.enqueue
                                        PriorityQueue.High
                                        ({ area with current = pos }, count + 1)
                                        paths)
                                paths

                        loop result paths'

        loop System.Int32.MaxValue queue

    let emptyStepCount area =
        area.grid |> Array2D.map (fun _ -> System.Int32.MaxValue)

    let findShortestPath area =
        let queue =
            (PriorityQueue.enqueue
                PriorityQueue.High
                (area, 0)
                PriorityQueue.empty)

        _findShortestPath (emptyStepCount area) queue

    let findShortestGlobalPath area =
        let queue =
            [
                for x in 0 .. Array2D.length1 area.grid - 1 do
                    for y in 0 .. Array2D.length2 area.grid - 1 do
                        if area.grid[x, y] = 0 then
                            yield x, y
            ]
            |> List.fold
                (fun queue start ->
                    queue
                    |> PriorityQueue.enqueue
                        PriorityQueue.High
                        ({ area with current = start }, 0))
                PriorityQueue.empty

        _findShortestPath (emptyStepCount area) queue

let input = readInputLines "12"

let job1 () =
    input |> Area.parse |> Area.findShortestPath |> string


let job2 () =
    input |> Area.parse |> Area.findShortestGlobalPath |> string
