module Utils

let notImplemented () = raise (System.NotImplementedException())

let unreachable () =
    raise (System.Exception "Panic: Unreachable code is reached!! 😱")

module Test =
    let run title expected fn =
        try
            let watch = System.Diagnostics.Stopwatch()
            watch.Start()
            let result = fn ()
            watch.Stop()

            if result = expected then
                printfn "✅ %s: %A [%A]" title result watch
            else
                eprintfn
                    "❌ %s: %A (expected: %A) [%A]"
                    title
                    result
                    expected
                    watch
        with
        | :? System.NotImplementedException ->
            eprintfn "🚧 %s: Not implemented" title
        | _ -> reraise ()

module Bool =
    let inline toInt (value: bool) = if value then 1 else 0
    let inline toByte (value: bool) = if value then 1uy else 0uy
    let inline toChar (value: bool) = if value then '1' else '0'
    let inline toLong (value: bool) = if value then 1L else 0L
    let inline toInt64 (value: bool) = if value then 1L else 0L
    let inline toUInt (value: bool) = if value then 1u else 0u
    let inline toUInt64 (value: bool) = if value then 1UL else 0UL
    let inline toFloat (value: bool) = if value then 1.0 else 0.0
    let inline toDouble (value: bool) = if value then 1.0 else 0.0
    let inline toDecimal (value: bool) = if value then 1M else 0M
    let inline toString (value: bool) = if value then "true" else "false"

let inline dump (obj: 'a) =
    printfn "%A" obj
    obj

let inline f_dump fn args =
    // printf "%A -> " args
    let watch = System.Diagnostics.Stopwatch()
    watch.Start()
    let result = fn args
    watch.Stop()
    printfn "%A [%A]" result watch
    args

let inline assert' fn =
    if not (fn ()) then
        failwith "Utils assertion failed"

[<AutoOpen>]
module FancyPatterns =
    open System.Text.RegularExpressions

    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern)

        if m.Success then
            Some(List.tail [ for g in m.Groups -> g.Value ])
        else
            None

    let (|Found|NotFound|) =
        function
        | true, value -> Found value
        | false, _ -> NotFound

    let (|Char|_|) (str: string) =
        match str.Length with
        | 1 -> Some str.[0]
        | _ -> None

    let (|Byte|_|) (str: string) =
        match System.Byte.TryParse(str) with
        | Found value -> Some value
        | NotFound -> None

    let (|Int|_|) (str: string) =
        match System.Int32.TryParse(str) with
        | Found value -> Some value
        | NotFound -> None

    let (|UInt|_|) (str: string) =
        match System.UInt32.TryParse(str) with
        | Found value -> Some value
        | NotFound -> None

    let (|Int64|_|) (str: string) =
        match System.Int64.TryParse(str) with
        | Found value -> Some value
        | NotFound -> None

    let (|UInt64|_|) (str: string) =
        match System.UInt64.TryParse(str) with
        | Found value -> Some value
        | NotFound -> None

    let (|Even|Odd|) number =
        match number % 2 with
        | 0 -> Even
        | _ -> Odd

module Option =
    let ofTry (ok, value) =
        match ok with
        | true -> Some value
        | false -> None

    let orDefault defaultValue =
        function
        | true, value -> value
        | false, _ -> defaultValue

type Dictionary<'key, 'value when 'key: equality> =
    System.Collections.Generic.Dictionary<'key, 'value>

module Dictionary =
    // let ofSeq
    //     (seq: Collections.seq<System.Collections.Generic.KeyValuePair<_, _>>)
    //     =
    //     Dictionary<_, _> seq
    // let ofSeq (seq: Collections.seq<_>) = Dictionary<_, _> seq
    // let ofSeq<'k, 'v when 'k: equality> (seq: ('k * 'v) seq) =
    //     seq |> Seq.fold (fun d (k, v) -> add k v d) (Dictionary<'k, 'v>())
    let ofSeq seq =
        seq
        |> Seq.fold
            (fun (d: Dictionary<_, _>) (key, value) ->
                d.Add(key, value) |> ignore
                d)
            (Dictionary<_, _>())

    let copy (d: Dictionary<_, _>) = Dictionary<_, _> d

    let isEmpty (d: Dictionary<_, _>) = d.Count = 0

    let keys (d: Dictionary<_, _>) = d.Keys |> Seq.toArray

    let values (d: Dictionary<_, _>) = d.Values |> Seq.toArray

    let tryGetValue key (d: Dictionary<_, _>) =
        match d.TryGetValue key with
        | true, value -> Some value
        | false, _ -> None

    let get key (d: Dictionary<_, _>) = d[key]

    let set key value (d: Dictionary<_, _>) =
        d[key] <- value
        d

    let tryAdd key value (d: Dictionary<_, _>) =
        d.TryAdd(key, value) |> ignore
        d

    let update key fn (d: Dictionary<_, _>) =
        d[key] <- fn (d.TryGetValue key |> Option.ofTry)
        d

    let change key fn (d: Dictionary<_, _>) =
        match d.TryGetValue key |> Option.ofTry |> fn with
        | Some value -> d[key] <- value
        | None -> d.Remove key |> ignore

        d

    let remove key (d: Dictionary<_, _>) =
        d.Remove key |> ignore
        d

    let getOrInsertWith (d: Dictionary<_, _>) key fn =
        match tryGetValue key d with
        | Some value -> value
        | None ->
            let value = fn d
            d[key] <- value
            value

let inline useCacheWith (initialEntries) =
    let cache = initialEntries |> Dictionary.ofSeq
    fun key buildValue -> Dictionary.getOrInsertWith cache key buildValue

let inline useCache<'k, 'v when 'k: equality> () =
    useCacheWith Seq.empty<'k * 'v>

type HashSet<'a> = System.Collections.Generic.HashSet<'a>

module HashSet =
    let empty<'a> = HashSet<_>()

    let ofSeq (seq: Collections.seq<_>) = HashSet<_> seq

    let singleton (item: 'a) = HashSet<_> [ item ]

    let copy (set: HashSet<_>) = new HashSet<_>(set)

    let isEmpty (set: HashSet<_>) = set.Count = 0

    let add item (set: HashSet<_>) =
        set.Add item |> ignore
        set

    let remove item (set: HashSet<_>) =
        set.Remove item |> ignore
        set

    let contains item (set: HashSet<_>) = set.Contains item

    let tryContains item (set: HashSet<_>) =
        if set.Contains item then Some set else None

    let unionWith (other: HashSet<_>) (set: HashSet<_>) =
        set.UnionWith other
        set

    let intersectWith (other: HashSet<_>) (set: HashSet<_>) =
        set.IntersectWith other
        set

module String =
    let join separator (chunks: Collections.seq<_>) =
        System.String.Join(separator, chunks)

    let trim (string: string) = string.Trim()

    let inline split<'a> (sep: 'a) (string: string) =
        match box sep with
        | :? char as sep -> string.Split(sep)
        | :? string as sep -> string.Split(sep)
        | _ -> failwith "Utils.String.split separator must be char or string"

    let inline splitNoEmpty<'a> (sep: 'a) (string: string) =
        match box sep with
        | :? char as sep ->
            string.Split(sep, System.StringSplitOptions.RemoveEmptyEntries)
        | :? string as sep ->
            string.Split(sep, System.StringSplitOptions.RemoveEmptyEntries)
        | _ ->
            failwith
                "Utils.String.splitNoEmpty separator must be char or string"

    let toCharArray (string: string) = string.ToCharArray()

    let toByteArray (string: string) =
        string |> System.Text.Encoding.ASCII.GetBytes

    let toSections = trim >> split "\n\n" >> Array.map trim

    let toLines = trim >> split '\n' >> Array.map trim

    let inline ofChars<'a> chars = System.String.Concat<'a> chars

    let substring i (string: string) = string.Substring(i)

    let startsWith (prefix: string) (string: string) = string.StartsWith prefix

    let inline parseInts<'a> (sep: 'a) line =
        line |> splitNoEmpty<'a> sep |> Array.map int

    let inline parseInt64s<'a> (sep: 'a) line =
        line |> splitNoEmpty<'a> sep |> Array.map int64

open System.IO

let private _join dir name = Path.Join([| dir; name |])

let private _exists = File.Exists

[<TailCall>]
let rec private _findFile dir path =
    let filepath = _join dir path

    if _exists filepath then
        filepath
    else
        _findFile (Directory.GetParent(dir).FullName) path

let private _findInputFile name =
    let subpath = _join "_inputs" (sprintf "Day%s.txt" name)

    _findFile (Directory.GetCurrentDirectory()) subpath

let useFileCache getterFn =
    let mutable cache = Map.empty

    fun key ->
        match cache.TryFind(key) with
        | Some value -> value
        | None ->
            let value = getterFn key
            cache <- cache |> Map.add key value
            value

let private _readLines filename = File.ReadLines(filename) |> Seq.toArray

let readInputLines = useFileCache (_findInputFile >> _readLines)

let private _readAllText filename = File.ReadAllText(filename).Trim()

let readInputText = useFileCache (_findInputFile >> _readAllText)

let readInputExact =
    useFileCache (
        _findInputFile >> (fun filename -> File.ReadAllText(filename))
    )

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
        let rec gcd x y = if y |> isZero then x else gcd y (x % y)

        match (abs a, abs b) with
        | a, b when a < b -> b, a
        | a, b -> a, b
        ||> gcd

    let inline GCD a b = greatestCommonDivisor a b

    let inline leastCommonMultiple a b = abs a / GCD a b * abs b
    let inline LCM a b = leastCommonMultiple a b

    let factorize n =
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
    let inline private lcm_pair a b = Math.LCM a b

    let inline private lcm_many list =
        let rec loop =
            function
            | [ a; b ] -> lcm_pair a b
            | a :: list -> lcm_pair a (loop list)
            | _ -> failwith "At least 2 values are required"

        loop list

    let inline leastCommonMultiple list = list |> lcm_many
    let inline LCM list = leastCommonMultiple list

type PriorityList<'a> = PriorityList of 'a list * 'a list

module PriorityList =
    let empty = PriorityList(List.empty, List.empty)

    let length (PriorityList(high, low)) = List.length high + List.length low

    type Priority =
        | High
        | Low

    let push priority item (PriorityList(high, low)) =
        match priority with
        | High -> PriorityList(item :: high, low)
        | Low -> PriorityList(high, item :: low)

    let pop list =
        match list with
        | PriorityList(item :: high, low) -> item, PriorityList(high, low)
        | PriorityList([], item :: low) -> item, PriorityList([], low)
        | PriorityList([], []) -> failwith "Empty list!"

    let tryPop list =
        match list with
        | PriorityList(item :: high, low) -> Some item, PriorityList(high, low)
        | PriorityList([], item :: low) ->
            Some item, PriorityList(List.empty, low)
        | PriorityList([], []) -> None, list

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

    let enqueue priority item (PriorityQueue(high, normal, low)) =
        match priority with
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
    let empty = {
        X = (System.Int32.MaxValue, System.Int32.MinValue)
        Y = (System.Int32.MaxValue, System.Int32.MinValue)
    }

    let merge { X = (minX, maxX); Y = (minY, maxY) } (x, y) = {
        X = (min x minX, max x maxX)
        Y = (min y minY, max y maxY)
    }

    let ofList list = list |> List.fold merge empty

    let ofArray array = array |> Array.fold merge empty

    let ofSeq seq = seq |> Seq.fold merge empty

    let ofMap map = map |> Map.toSeq |> Seq.map fst |> ofSeq


let render toString map =
    let box = map |> BBox.ofMap

    String.join
        ""
        (seq {
            for y in fst box.Y .. snd box.Y do
                yield "\n"

                for x in fst box.X .. snd box.X do
                    yield map |> Map.tryFind (x, y) |> toString
        })

module Md5 =
    let ofString (string: string) =
        use md5 = System.Security.Cryptography.MD5.Create()

        string
        |> String.toByteArray
        |> md5.ComputeHash
        |> System.Convert.ToHexString

module Range =
    let init (a: int) (b: int) =
        assert (a <= b)
        if a < b then a, b else b, a

    let longLength (a, b) = int64 b - int64 a + 1L

    let contains value (a: int, b: int) = value >= a && value <= b

    let tryMerge (a, b) (c, d) =
        let range = init (min a c) (max b d)
        let newLength = longLength range
        let oldLength = longLength (a, b) + longLength (c, d)
        if newLength <= oldLength then Some range else None

    let combine ranges =
        let rec loop result ranges =
            match ranges with
            | [] -> result
            | range :: ranges ->
                let range, ranges =
                    ranges
                    |> loop []
                    |> List.fold
                        (fun (range, ranges) r ->
                            match tryMerge range r with
                            | Some range -> range, ranges
                            | None -> range, (r :: ranges))
                        (range, [])

                loop (range :: result) ranges

        loop [] ranges
