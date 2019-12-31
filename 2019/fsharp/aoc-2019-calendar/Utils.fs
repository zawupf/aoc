module Utils

module String =
    let join separator (chunks: Collections.seq<_>) = System.String.Join(separator, chunks)

open System.IO

let private _join dir name = Path.Join([| dir; name |])

let private _exists = File.Exists

let rec private _findFile dir path =
    let filepath = _join dir path
    if _exists filepath
    then filepath
    else _findFile (Directory.GetParent(dir).FullName) path

let private _findInputFile name =
    let subpath = _join "inputs" (sprintf "Day%s.txt" name)
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

let private _readLines filename = File.ReadLines(filename)

let readInputLines = useCache (_findInputFile >> _readLines)

let private _readAllText filename = File.ReadAllText(filename).Trim()

let readInputText = useCache (_findInputFile >> _readAllText)

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

let greatestCommonDivisor a b =
    let rec gcd x y =
        if y = 0 then x else gcd y (x % y)

    match (abs a, abs b) with
    | a, b when a < b -> b, a
    | a, b -> a, b
    ||> gcd

type BBox =
    { x: int * int
      y: int * int }

module BBox =
    let empty =
        { x = (System.Int32.MaxValue, System.Int32.MinValue)
          y = (System.Int32.MaxValue, System.Int32.MinValue) }

    let merge { x = (minX, maxX); y = (minY, maxY) } (x, y) =
        { x = (min x minX, max x maxX)
          y = (min y minY, max y maxY) }

    let ofList list = list |> List.fold merge empty

    let ofArray array = array |> Array.fold merge empty

    let ofSeq seq = seq |> Seq.fold merge empty

    let ofMap map =
        map
        |> Map.toSeq
        |> Seq.map fst
        |> ofSeq


let render toString map =
    let bbox = map |> BBox.ofMap
    String.join ""
        (seq {
            for y in fst bbox.y .. snd bbox.y do
                yield "\n"
                for x in fst bbox.x .. snd bbox.x do
                    yield map
                          |> Map.tryFind (x, y)
                          |> toString
         })
