#load "Utils.fsx"

open System.Text.RegularExpressions

type Pos = Pos of int * int

type Symbol = Symbol of char * Pos

module Symbol =
    [<Literal>]
    let nonSymbols = "01234567."

    let tryCreate lines pos =
        let getChar (Pos(x, y)) =
            lines
            |> Array.tryItem y
            |> Option.bind (fun line ->
                if x >= 0 && x < (line |> String.length) then
                    Some(line[x])
                else
                    None)

        match getChar pos with
        | Some c when nonSymbols.Contains(c) |> not -> Symbol(c, pos) |> Some
        | _ -> None

type Number = {
    Value: int
    Pos: Pos
    Length: int
    Symbols: Symbol[]
}

module Number =
    let withSymbols lines number =
        let { Pos = Pos(nx, ny); Length = l } = number

        {
            number with
                Symbols =
                    seq {
                        let y = ny - 1
                        for x in nx - 1 .. nx + l -> Pos(x, y)

                        let y = ny + 1
                        for x in nx - 1 .. nx + l -> Pos(x, y)

                        yield Pos(nx - 1, ny)
                        yield Pos(nx + l, ny)
                    }
                    |> Seq.choose (Symbol.tryCreate lines)
                    |> Seq.toArray
        }

    let gearPos n =
        n.Symbols
        |> Array.tryPick (function
            | Symbol('*', pos) -> Some pos
            | _ -> None)

let findNumbers lines =
    lines
    |> Array.indexed
    |> Array.collect (fun (y, line) ->
        let ms = Regex.Matches(line, @"\d+")

        ms
        |> Seq.map (fun m ->
            {
                Value = m.Value |> int
                Length = m.Length
                Pos = Pos(m.Index, y)
                Symbols = Array.empty
            }
            |> Number.withSymbols lines)
        |> Seq.toArray)

let part1 input = //
    input
    |> findNumbers
    |> Seq.filter (fun n -> n.Symbols |> Array.isEmpty |> not)
    |> Seq.sumBy _.Value

let part2 input = //
    input
    |> findNumbers
    |> Array.choose (fun number ->
        match Number.gearPos number with
        | Some pos -> Some(pos, number.Value)
        | _ -> None)
    |> Array.groupBy fst
    |> Array.sumBy (fun (_, values) ->
        match values |> Array.map snd with
        | [| v1; v2 |] -> v1 * v2
        | [| _ |] -> 0
        | array -> failwithf "Invalid gear values: %A" array)



let testInput = [|
    """
467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..
"""
    |> Utils.String.toLines
|]

Utils.Test.run "Test part 1" 4361 (fun () -> part1 testInput[0])
Utils.Test.run "Test part 2" 467835 (fun () -> part2 testInput[0])



let input = Utils.readInputLines "03"

let getDay03_1 () = part1 input

let getDay03_2 () = part2 input

Utils.Test.run "Part 1" 557705 getDay03_1
Utils.Test.run "Part 2" 84266818 getDay03_2
