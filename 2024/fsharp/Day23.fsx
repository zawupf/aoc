#load "Utils.fsx"

type Node = string
type NodeSet = Utils.HashSet<Node>
type NodeMap = Utils.Dictionary<Node, NodeSet>

type SubnetKey = string
type SubnetMap = Utils.Dictionary<SubnetKey, NodeSet>

let parseMap lines : NodeMap =
    lines
    |> Array.map (Utils.String.split "-")
    |> Array.fold
        (fun map nodePair ->
            let add node =
                function
                | Some set -> set |> Utils.HashSet.add node
                | None -> Utils.HashSet.singleton node

            match nodePair with
            | [| a; b |] ->
                map
                |> Utils.Dictionary.update a (add b)
                |> Utils.Dictionary.update b (add a)
            | _ -> failwith "Invalid node pair")
        (NodeMap())

let subnetKey nodes = nodes |> Seq.sort |> String.concat ","

let commonNodes map nodes =
    nodes
    |> Seq.map (fun node -> map |> Utils.Dictionary.get node)
    |> Seq.toList
    |> function
        | [ head ] -> head
        | head :: tail ->
            tail
            |> List.fold
                (fun common connected ->
                    common |> Utils.HashSet.intersectWith connected)
                (head |> Utils.HashSet.copy)
        | _ -> failwith "No nodes"

let countGroupsOfThreeWithT (map: NodeMap) =
    map
    |> Seq.choose (fun (KeyValue(node, nodes)) ->
        match node |> Utils.String.startsWith "t" with
        | true -> Some(node, nodes)
        | false -> None)
    |> Seq.collect (fun (node, nodes) ->
        nodes
        |> Seq.collect (fun n ->
            [ node; n ]
            |> Utils.HashSet.ofSeq
            |> commonNodes map
            |> Seq.map (fun c -> [ node; n; c ] |> subnetKey)))
    |> Seq.distinct
    |> Seq.length

let resolveSubnets (map: NodeMap) =
    let rec loop (final: SubnetMap) (subnets: SubnetMap) =
        match subnets |> Utils.Dictionary.isEmpty with
        | true -> final
        | false ->
            let final, subnets =
                subnets
                |> Seq.fold
                    (fun
                        (final: SubnetMap, subnets: SubnetMap)
                        (KeyValue(key, nodes)) ->
                        let common = commonNodes map nodes

                        match common |> Utils.HashSet.isEmpty with
                        | true ->
                            final |> Utils.Dictionary.set key nodes, subnets
                        | false ->
                            final,
                            common
                            |> Seq.fold
                                (fun subnets c ->
                                    let subnet =
                                        nodes
                                        |> Utils.HashSet.copy
                                        |> Utils.HashSet.add c

                                    subnets
                                    |> Utils.Dictionary.set
                                        (subnet |> subnetKey)
                                        subnet)
                                subnets)
                    (final, SubnetMap())

            loop final subnets

    let subnets: SubnetMap =
        map
        |> Utils.Dictionary.keys
        |> Seq.map (fun node -> node, Utils.HashSet.singleton node)
        |> Utils.Dictionary.ofSeq

    loop (SubnetMap()) subnets

let findPassword (map: NodeMap) =
    map
    |> resolveSubnets
    |> Utils.Dictionary.keys
    |> Array.maxBy (fun key -> key.Length)

let part1 input =
    input |> parseMap |> countGroupsOfThreeWithT

let part2 input = input |> parseMap |> findPassword

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    """
    kh-tc
    qp-kh
    de-cg
    ka-co
    yn-aq
    qp-ub
    cg-tb
    vc-aq
    tb-ka
    wh-tc
    yn-cg
    kh-ub
    ta-co
    de-co
    tc-td
    tb-wq
    wh-td
    ta-ka
    td-qp
    aq-cg
    wq-ub
    ub-vc
    de-ta
    wq-aq
    wq-vc
    wh-yn
    ka-de
    kh-ta
    co-tc
    wh-qp
    tb-vc
    td-yn
    """
    |> Utils.String.toLines

Utils.Test.run "Test 2" 7 (fun () -> part1 testInput)
Utils.Test.run "Test 2" "co,de,ka,ta" (fun () -> part2 testInput)

Utils.Test.run "Part 1" 1043 solution1
Utils.Test.run "Part 2" "ai,bk,dc,dx,fo,gx,hk,kd,os,uz,xn,yk,zs" solution2

#load "_benchmark.fsx"
