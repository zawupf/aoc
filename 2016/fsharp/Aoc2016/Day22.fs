module Day22

open Utils

type Node = {
    X: int
    Y: int
    Size: int
    Used: int
} with

    member this.Id = this.X, this.Y
    member this.Avail = this.Size - this.Used

    member this.UsePercent = this.Used * 100 / this.Size

module Node =
    let isEmpty node = node.Used = 0

    let parse line =
        let rx =
            @"^/dev/grid/node-x(\d+)-y(\d+)\s+(\d+)T\s+(\d+)T\s+(\d+)T\s+(\d+)\%$"

        match line with
        | Regex rx [ Int x; Int y; Int size; Int used; Int avail; Int usep ] ->
            let node = {
                X = x
                Y = y
                Size = size
                Used = used
            }

            assert (node.Id = (x, y))
            assert (node.Avail = avail)
            assert (node.UsePercent = usep)
            node
        | _ -> failwith $"Invalid node: %s{line}"

    let parseInput lines =
        lines |> Seq.skip 2 |> Seq.map parse |> Seq.toList

let viablePairsCount nodes =
    let isViable n1 (n2: Node) =
        n1 |> (Node.isEmpty >> not) && n1.Id <> n2.Id && n1.Used <= n2.Avail

    let rec loop count nodes =
        match nodes with
        | [] -> count
        | node :: nodes ->
            let count' =
                nodes
                |> List.fold
                    (fun count node2 ->
                        count
                        + (if isViable node node2 then 1 else 0)
                        + (if isViable node2 node then 1 else 0))
                    count

            loop count' nodes

    loop 0 nodes

let input = readInputLines "22"

let job1 () =
    input |> Node.parseInput |> viablePairsCount |> string

let job2 () =
    // input |> String.join "" |> string
    raise (System.NotImplementedException())
