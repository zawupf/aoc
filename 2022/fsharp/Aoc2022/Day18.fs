module Day18

open Utils

type Pt = int * int * int
and Edge = Pt * Pt
and Face = Pt * Pt
and Cube = Pt

module Face =
    let surfaces (faces: Face list) =
        faces
        |> List.countBy id
        |> List.choose (fun (face, count) ->
            if count = 1 then Some face else None)

module Cube =
    let create x y z : Cube = x, y, z

    let parse line =
        match line |> String.parseInts ',' with
        | [ x; y; z ] -> create x y z
        | _ -> failwithf "Invalid line: %s" line

    let faces (cube: Cube) : Face list =
        let (x, y, z) = cube

        [
            ((x, y, z), (x + 1, y + 1, z))
            ((x, y, z + 1), (x + 1, y + 1, z + 1))
            ((x, y, z), (x, y + 1, z + 1))
            ((x + 1, y, z), (x + 1, y + 1, z + 1))
            ((x, y, z), (x + 1, y, z + 1))
            ((x, y + 1, z), (x + 1, y + 1, z + 1))
        ]

    let grid (cubes: Cube list) =
        let imin = System.Int32.MinValue
        let imax = System.Int32.MaxValue

        let set, (min1, min2, min3), (max1, max2, max3) =
            cubes
            |> List.fold
                (fun (set, (x, y, z), (x', y', z')) (a, b, c) ->
                    (set |> Set.add (a, b, c)),
                    (min x a, min y b, min z c),
                    (max x' a, max y' b, max z' c))
                (Set.empty, (imax, imax, imax), (imin, imin, imin))

        let (min1, min2, min3), (max1, max2, max3) =
            (min1 - 1, min2 - 1, min3 - 1), (max1 + 1, max2 + 1, max3 + 1)

        set, (min1, min2, min3), (max1, max2, max3)

    let fillPockets cubes =
        let grid, (min1, min2, min3), (max1, max2, max3) = grid cubes

        let isValidAndEmpty (x, y, z) =
            x >= min1
            && x <= max1
            && y >= min2
            && y <= max2
            && z >= min3
            && z <= max3
            && (grid |> Set.contains (x, y, z) |> not)

        let rec loop coords visited =
            match coords with
            | [] -> [
                for x in min1..max1 do
                    for y in min2..max2 do
                        for z in min3..max3 do
                            if visited |> Set.contains (x, y, z) |> not then
                                yield (x, y, z)
              ]
            | _ ->
                let coords' =
                    coords
                    |> List.collect (fun (x, y, z) ->
                        [
                            x + 1, y, z
                            x - 1, y, z
                            x, y + 1, z
                            x, y - 1, z
                            x, y, z + 1
                            x, y, z - 1
                        ]
                        |> List.filter (fun pt ->
                            isValidAndEmpty pt
                            && (Set.contains pt visited |> not)))
                    |> List.distinct

                loop coords' (Set.union visited (Set coords'))

        let corners = [ min1, min2, min3 ]

        assert
            (corners
             |> List.forall (fun cube -> grid |> Set.contains cube |> not))

        loop corners (corners |> Set.ofList)


let cubes lines = lines |> List.map Cube.parse

let faces cubes = cubes |> List.collect Cube.faces

let totalSurface lines =
    lines |> cubes |> faces |> Face.surfaces |> List.length

let outerSurface lines =
    lines |> cubes |> Cube.fillPockets |> faces |> Face.surfaces |> List.length

let input = readInputLines "18" |> Array.toList

let job1 () = input |> totalSurface |> string

let job2 () = input |> outerSurface |> string
