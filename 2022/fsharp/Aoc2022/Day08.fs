module Day08

open Utils

type Grid = int[,]

module Grid =
    let parse lines =
        lines
        |> Array.map (
            String.toByteArray >> Array.map (fun b -> int b - int '0')
        )
        |> array2D

    let rowCount (grid: Grid) = grid |> Array2D.length1

    let columnCount (grid: Grid) = grid |> Array2D.length2

let countVisibleTrees (grid: Grid) =
    let w, h = Grid.columnCount grid, Grid.rowCount grid

    let rec lookAt index line height result =
        match line |> Array.length = index with
        | true -> result
        | false ->
            match line[index] with
            | h when h <= height -> lookAt (index + 1) line height result
            | h -> lookAt (index + 1) line h (index :: result)

    let handle line mapIndex set =
        lookAt 1 line line[0] []
        |> List.fold (fun set i -> set |> Set.add (mapIndex i)) set

    let handleRow set rowIndex =
        let row = grid[rowIndex, *]

        set
        |> handle row (fun i -> rowIndex, i)
        |> handle (row |> Array.rev) (fun i -> rowIndex, w - 1 - i)

    let handleRows set =
        [ 1 .. h - 2 ] |> List.fold handleRow set

    let handleCol set colIndex =
        let col = grid[*, colIndex]

        set
        |> handle col (fun i -> i, colIndex)
        |> handle (col |> Array.rev) (fun i -> h - 1 - i, colIndex)

    let handleCols set =
        [ 1 .. w - 2 ] |> List.fold handleCol set

    seq {
        for col in 0 .. w - 1 -> (0, col)
        for col in 0 .. w - 1 -> (h - 1, col)
        for row in 0 .. h - 1 -> (row, 0)
        for row in 0 .. h - 1 -> (row, w - 1)
    }
    |> Set.ofSeq
    |> handleRows
    |> handleCols
    |> Set.count

let maxScenicScore (grid: Grid) =
    let w, h = Grid.columnCount grid, Grid.rowCount grid

    let scenicScore (irow, icol) =
        let height = grid[irow, icol]

        [ grid[irow, icol + 1 ..]
          grid[irow, .. icol - 1] |> Array.rev
          grid[irow + 1 .., icol]
          grid[.. irow - 1, icol] |> Array.rev ]
        |> List.map (fun line ->
            match line |> Array.tryFindIndex (fun h -> h >= height) with
            | Some i -> i + 1
            | None -> line |> Array.length)
        |> List.fold (fun score dist -> score * dist) 1

    seq {
        for row in 1 .. h - 2 do
            for col in 1 .. w - 2 -> row, col
    }
    |> Seq.map scenicScore
    |> Seq.max

let input = readInputLines "08" |> Grid.parse

let job1 () = input |> countVisibleTrees |> string

let job2 () = input |> maxScenicScore |> string
