module Utils

open System.IO

let private _join dir name = Path.Join([| dir; name |])

let private _exists = File.Exists

let rec private _findFile dir path =
    let filepath = _join dir path
    if _exists filepath then filepath
    else _findFile (Directory.GetParent(dir).FullName) path

let private _findInputFile name =
    let subpath = _join "inputs" (sprintf "Day%s.txt" name)
    _findFile (Directory.GetCurrentDirectory()) subpath

let private _readLines filename = File.ReadLines(filename)
let readInputLines name = _readLines (_findInputFile name)

let private _readAllText filename = File.ReadAllText(filename)
let readInputText name = _readAllText (_findInputFile name)
