module Day15

open Utils

type Ingredient =
    { Name: string
      Capacity: int
      Durability: int
      Flavor: int
      Texture: int
      Calories: int }

module Ingredient =
    let parse input =
        let pattern =
            @"(\w+): capacity (-?\d+), durability (-?\d+), flavor (-?\d+), texture (-?\d+), calories (-?\d+)"

        match input with
        | Regex pattern [ name
                          Int capacity
                          Int durability
                          Int flavor
                          Int texture
                          Int calories ] ->
            { Name = name
              Capacity = capacity
              Durability = durability
              Flavor = flavor
              Texture = texture
              Calories = calories }
        | _ -> failwith $"Invalid input: %s{input}"

    let properties (amount, ingredient) =
        let { Capacity = capacity
              Durability = durability
              Flavor = flavor
              Texture = texture } =
            ingredient

        [| capacity; durability; flavor; texture |]
        |> Array.map (fun value -> amount * value)

    let calories (amount, ingredient) =
        let { Calories = calories } = ingredient
        amount * calories

let totalScore recipe =
    recipe
    |> List.map (fun amount -> amount |> Ingredient.properties)
    |> List.fold
        (fun sum properties ->
            Array.zip sum properties |> Array.map (fun (a, b) -> a + b))
        [| 0; 0; 0; 0 |]
    |> Array.fold (fun product value -> product * (max 0L value)) 1L

let totalCalories recipe =
    recipe |> List.map (fun amount -> amount |> Ingredient.calories) |> List.sum

let amounts n =
    let rec loop n max result =
        match n, result with
        | 0, _ -> failwith "No ingredients, no cookies!"
        | 1, [] -> [ max, [ max ] ]
        | 1, _ ->
            result
            |> List.map (fun (current, amounts) ->
                let missing = max - current
                max, missing :: amounts)
        | _, [] -> loop (n - 1) max [ for i in 0..max -> i, [ i ] ]
        | _, _ ->
            loop
                (n - 1)
                max
                (result
                 |> List.fold
                     (fun result (current, amounts) ->
                         [ 0 .. max - current ]
                         |> List.fold
                             (fun result i ->
                                 (i + current, i :: amounts) :: result)
                             result)
                     [])

    loop n 100 [] |> List.map snd

let maxTotalScore ingredients =
    let n = ingredients |> List.length

    amounts n
    |> List.map (fun amounts -> ingredients |> List.zip amounts |> totalScore)
    |> List.max

let maxTotalScore500 ingredients =
    let n = ingredients |> List.length

    amounts n
    |> List.filter (fun amounts ->
        ingredients |> List.zip amounts |> totalCalories = 500)
    |> List.map (fun amounts -> ingredients |> List.zip amounts |> totalScore)
    |> List.max

let input = readInputLines "15" |> Seq.map Ingredient.parse |> Seq.toList

let job1 () = input |> maxTotalScore |> string

let job2 () = input |> maxTotalScore500 |> string
