module Utils

[<AutoOpen>]
module FancyPatterns =
    open System.Text.RegularExpressions

    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern)

        if m.Success then
            Some(List.tail [ for g in m.Groups -> g.Value ])
        else
            None

    let (|Int|_|) (str: string) =
        match System.Int32.TryParse(str) with
        | true, intvalue -> Some intvalue
        | _ -> None

module String =
    let join separator (chunks: Collections.seq<_>) =
        System.String.Join(separator, chunks)

    let trim (string: string) = string.Trim()

    let split (sep: char) (string: string) = string.Split(sep)

    let splitNoEmpty (sep: char) (string: string) =
        string.Split(sep, System.StringSplitOptions.RemoveEmptyEntries)

    let toCharArray (string: string) = string.ToCharArray()

    let ofChars chars =
        System.String(chars |> Seq.toArray) |> string

    let substring i (string: string) = string.Substring(i)

    let startsWith (prefix: string) (string: string) = string.StartsWith prefix

    let parseInts sep line =
        line |> splitNoEmpty sep |> List.ofArray |> List.map int

open System.IO

let private _join dir name = Path.Join([| dir; name |])

let private _exists = File.Exists

let rec private _findFile dir path =
    let filepath = _join dir path

    if _exists filepath then
        filepath
    else
        _findFile (Directory.GetParent(dir).FullName) path

let private _findInputFile name =
    let subpath = _join "_inputs" (sprintf "Day%s.txt" name)

    _findFile (Directory.GetCurrentDirectory()) subpath

let useCache getterFn =
    let mutable cache = Map.empty

    fun key ->
        match cache.TryFind(key) with
        | Some value -> value
        | None ->
            let value = getterFn key
            cache <- cache |> Map.add key value
            value

let private _readLines filename = File.ReadLines(filename) |> Seq.toArray

let readInputLines = useCache (_findInputFile >> _readLines)

let private _readAllText filename = File.ReadAllText(filename).Trim()

let readInputText = useCache (_findInputFile >> _readAllText)

module Math =
    let permutations list =
        let rec _permutations list taken =
            seq {
                if Set.count taken = List.length list then
                    yield []
                else
                    for l in list do
                        if not (Set.contains l taken) then
                            for perm in _permutations list (Set.add l taken) do
                                yield l :: perm
            }

        _permutations list Set.empty

    let inline isZero value = value = LanguagePrimitives.GenericZero

    let inline greatestCommonDivisor a b =
        let rec gcd x y =
            if y |> isZero then x else gcd y (x % y)

        match (abs a, abs b) with
        | a, b when a < b -> b, a
        | a, b -> a, b
        ||> gcd

    let inline GCD a b = greatestCommonDivisor a b

    let inline leastCommonMultiple a b = abs a / GCD a b * abs b
    let inline LCM a b = leastCommonMultiple a b

    let factorise n =
        let rec loop n x factors =
            if x = n then x :: factors
            elif n % x = 0 then loop (n / x) x (x :: factors)
            else loop n (x + 1) factors

        loop n 2 []

    let divisors n =
        let m = n |> double |> sqrt |> int

        let realDivisors =
            { 2..m }
            |> Seq.fold
                (fun divisors d ->
                    match n % d with
                    | 0 ->
                        match n / d with
                        | p when p = d -> d :: divisors
                        | p -> d :: p :: divisors
                    | _ -> divisors)
                []

        match n with
        | 1 -> 1 :: realDivisors
        | _ -> 1 :: n :: realDivisors

module List =
    let inline private lcm a b = Math.LCM a b

    let inline private lcmm list =
        let rec _lcmm =
            function
            | [ a; b ] -> lcm a b
            | a :: list -> lcm a (_lcmm list)
            | _ -> failwith "At least 2 values are required"

        _lcmm list

    let inline leastCommonMultiple list = list |> lcmm
    let inline LCM list = leastCommonMultiple list

type Queue<'a> = Queue of 'a list * 'a list

module Queue =
    let empty = Queue([], [])

    let length (Queue(fs, bs)) = fs.Length + bs.Length

    let enqueue item queue =
        match queue with
        | Queue(fs, bs) -> Queue(item :: fs, bs)

    let dequeue queue =
        match queue with
        | Queue([], []) -> failwith "Empty queue!"
        | Queue(fs, b :: bs) -> b, Queue(fs, bs)
        | Queue(fs, []) ->
            let bs = List.rev fs
            bs.Head, Queue([], bs.Tail)

    let tryDequeue queue =
        match queue with
        | Queue([], []) -> None, queue
        | Queue(fs, b :: bs) -> Some b, Queue(fs, bs)
        | Queue(fs, []) ->
            let bs = List.rev fs
            Some bs.Head, Queue([], bs.Tail)

type PriorityQueue<'a> = PriorityQueue of Queue<'a> * Queue<'a> * Queue<'a>

module PriorityQueue =
    let empty = PriorityQueue(Queue.empty, Queue.empty, Queue.empty)

    let length (PriorityQueue(high, normal, low)) =
        Queue.length high + Queue.length normal + Queue.length low

    type Priority =
        | High
        | Normal
        | Low

    let enqueue prio item (PriorityQueue(high, normal, low)) =
        match prio with
        | High -> PriorityQueue(Queue.enqueue item high, normal, low)
        | Normal -> PriorityQueue(high, Queue.enqueue item normal, low)
        | Low -> PriorityQueue(high, normal, Queue.enqueue item low)

    let dequeue (PriorityQueue(high, normal, low)) =
        match Queue.tryDequeue high with
        | Some item, high' -> item, PriorityQueue(high', normal, low)
        | None, _ ->
            match Queue.tryDequeue normal with
            | Some item, normal' -> item, PriorityQueue(high, normal', low)
            | None, _ ->
                match Queue.tryDequeue low with
                | Some item, low' -> item, PriorityQueue(high, normal, low')
                | None, _ -> failwith "Empty queue!"

    let tryDequeue queue =
        let (PriorityQueue(high, normal, low)) = queue

        match Queue.tryDequeue high with
        | Some item, high' -> Some item, PriorityQueue(high', normal, low)
        | None, _ ->
            match Queue.tryDequeue normal with
            | Some item, normal' -> Some item, PriorityQueue(high, normal', low)
            | None, _ ->
                match Queue.tryDequeue low with
                | Some item, low' ->
                    Some item, PriorityQueue(high, normal, low')
                | None, _ -> None, queue

type BBox = { X: int * int; Y: int * int }

module BBox =
    let empty =
        { X = (System.Int32.MaxValue, System.Int32.MinValue)
          Y = (System.Int32.MaxValue, System.Int32.MinValue) }

    let merge { X = (minX, maxX); Y = (minY, maxY) } (x, y) =
        { X = (min x minX, max x maxX)
          Y = (min y minY, max y maxY) }

    let ofList list = list |> List.fold merge empty

    let ofArray array = array |> Array.fold merge empty

    let ofSeq seq = seq |> Seq.fold merge empty

    let ofMap map =
        map |> Map.toSeq |> Seq.map fst |> ofSeq


let render toString map =
    let bbox = map |> BBox.ofMap

    String.join
        ""
        (seq {
            for y in fst bbox.Y .. snd bbox.Y do
                yield "\n"

                for x in fst bbox.X .. snd bbox.X do
                    yield map |> Map.tryFind (x, y) |> toString
        })

module Md5 =
    let ofString (string: string) =
        use md5 = System.Security.Cryptography.MD5.Create()

        string
        |> System.Text.Encoding.ASCII.GetBytes
        |> md5.ComputeHash
        |> System.Convert.ToHexString
