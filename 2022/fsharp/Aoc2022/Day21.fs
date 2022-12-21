module Day21

open Utils

type Operand =
    | Add
    | Sub
    | Mul
    | Div
    | Match

let (|Op|_|) (str: string) =
    match str with
    | "+" -> Some Add
    | "-" -> Some Sub
    | "*" -> Some Mul
    | "/" -> Some Div
    | "=" -> Some Match
    | _ -> None

type Expr =
    | Value of int64
    | Operation of Operand * string * string
    | MagicNumber

let parseLine line =
    match line with
    | Regex @"^(\w+): (-?\d+)$" [ name; Int64 value ] -> name, Value(value)
    | Regex @"^(\w+): (\w+) ([*/+-]) (\w+)$" [ name; left; Op op; right ] ->
        name, Operation(op, left, right)
    | _ -> failwithf "Invalid line: %s" line

let parse lines = lines |> Seq.map parseLine |> Map.ofSeq

let calc op =
    match op with
    | Add -> (+)
    | Sub -> (-)
    | Mul -> (*)
    | Div -> (/)
    | Match -> (fun left right -> (left - right) |> sign |> int64)

let calcLeft op right result =
    match op with
    | Add -> result - right
    | Sub -> result + right
    | Mul -> result / right
    | Div -> result * right
    | Match -> right

let calcRight op left result =
    match op with
    | Add -> result - left
    | Sub -> left - result
    | Mul -> result / left
    | Div -> left / result
    | Match -> left

let eval name exprMap =
    let resolveMagicNumbers name value map magicMap =
        let rec loop name value map magicMap =
            let expr = magicMap |> Map.find name

            let map = map |> Map.add name (Value value)
            let magicMap = magicMap |> Map.remove name

            let next =
                match expr with
                | Operation(op, left, right) when
                    magicMap |> Map.containsKey left
                    ->
                    match map |> Map.find right with
                    | Value r -> Some(left, calcLeft op r value)
                    | _ -> failwith "Logic error 🤯"
                | Operation(op, left, right) when
                    magicMap |> Map.containsKey right
                    ->
                    match map |> Map.find left with
                    | Value l -> Some(right, calcRight op l value)
                    | _ -> failwith "Logic error 🤯"
                | MagicNumber -> None
                | _ -> failwith "Logic error 🤯"

            match next with
            | Some(name, value) -> loop name value map magicMap
            | None -> map, magicMap

        loop name value map magicMap

    let rec loop names map magicMap =
        match names with
        | [] ->
            match map |> Map.find name with
            | Value v -> v, map
            | MagicNumber
            | Operation _ -> failwithf "Could not resolve value of: %s" name
        | name :: names ->
            match map |> Map.find name with
            | Value _
            | MagicNumber -> loop names map magicMap
            | Operation(op, left, right) as operation ->
                match Map.find left map, Map.find right map with
                | Value l, Value r ->
                    loop
                        names
                        (map |> Map.add name (Value(calc op l r)))
                        magicMap
                | Value _, Operation _ ->
                    loop (right :: name :: names) map magicMap
                | Operation _, Value _ ->
                    loop (left :: name :: names) map magicMap
                | Operation _, Operation _ ->
                    loop (left :: right :: name :: names) map magicMap
                | MagicNumber, Value value when op = Match ->
                    let map, magicMap =
                        resolveMagicNumbers left value map magicMap

                    loop
                        names
                        (map |> Map.add name (Value(calc op value value)))
                        magicMap
                | Value value, MagicNumber when op = Match ->
                    let map, magicMap =
                        resolveMagicNumbers right value map magicMap

                    loop
                        names
                        (map |> Map.add name (Value(calc op value value)))
                        magicMap
                | MagicNumber, Value _
                | MagicNumber, Operation _ ->
                    let magicMap = magicMap |> Map.add name operation

                    let map = map |> Map.add name MagicNumber
                    loop (right :: name :: names) map magicMap
                | Value _, MagicNumber
                | Operation _, MagicNumber ->
                    let magicMap = magicMap |> Map.add name operation

                    let map = map |> Map.add name MagicNumber
                    loop (right :: name :: names) map magicMap
                | MagicNumber, MagicNumber -> failwithf "I'm screwed 🤪"

    let magicMap =
        exprMap
        |> Map.filter (fun _ expr ->
            match expr with
            | MagicNumber -> true
            | _ -> false)

    loop [ name ] exprMap magicMap

let guessNumber exprMap =
    let map =
        match exprMap |> Map.find "root" with
        | Operation(_, l, r) ->
            exprMap |> Map.add "root" (Operation(Match, l, r))
        | _ -> failwith "Invlalid input"
        |> Map.add "humn" MagicNumber

    match eval "root" map |> snd |> Map.find "humn" with
    | Value v -> v
    | _ -> failwith "Eval magic number failed"

let input = readInputLines "21"

let job1 () =
    input |> parse |> eval "root" |> fst |> string

let job2 () = input |> parse |> guessNumber |> string
