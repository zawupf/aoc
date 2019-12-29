module Utils

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

let private _readLines filename = File.ReadLines(filename)
let readInputLines name = _readLines (_findInputFile name)

let private _readAllText filename = File.ReadAllText(filename).Trim()
let readInputText name = _readAllText (_findInputFile name)

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

    let (x, y) =
        match (abs a, abs b) with
        | a, b when a < b -> b, a
        | a, b -> a, b

    gcd x y
