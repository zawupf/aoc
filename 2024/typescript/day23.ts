import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Node = string
type NodeSet = Set<Node>
type NodeMap = Map<Node, NodeSet>

type SubnetKey = string
type SubnetMap = Map<SubnetKey, NodeSet>

class Lan {
    map: NodeMap

    constructor(map: NodeMap) {
        this.map = map
    }

    static from(lines: string[]): Lan {
        return new Lan(
            lines.reduce((map, line) => {
                const [a, b] = line.split('-')
                if (!map.has(a)) {
                    map.set(a, new Set())
                }
                map.get(a)!.add(b)
                if (!map.has(b)) {
                    map.set(b, new Set())
                }
                map.get(b)!.add(a)
                return map
            }, new Map()),
        )
    }

    countGroupsOfThreeWithT(): number {
        return new Set(
            new Map(this.map.entries().filter(([node]) => node.startsWith('t')))
                .entries()
                .flatMap(([node, nodes]) =>
                    nodes.values().flatMap(n =>
                        this.commonNodes(new Set([node, n]))
                            .values()
                            .map(c => this.subnetKey(new Set([node, n, c])))
                            .toArray(),
                    ),
                ),
        ).size
    }

    findPassword(): string {
        return this.resolveSubnets()
            .keys()
            .map(key => [key.length, key] as [number, string])
            .reduce(
                ([len, key], [l, k]) => (l > len ? [l, k] : [len, key]),
                [0, ''],
            )[1]
    }

    private resolveSubnets(): SubnetMap {
        return this.resolveSubnetsRec(
            new Map(
                Array.from(this.map.keys()).map(node => [
                    node as SubnetKey,
                    new Set([node]) as NodeSet,
                ]),
            ),
        )
    }

    private resolveSubnetsRec(
        subnets: SubnetMap,
        final: SubnetMap = new Map(),
    ): SubnetMap {
        const map: SubnetMap = new Map()
        for (const [key, nodes] of subnets) {
            const common = this.commonNodes(nodes)
            if (common.size === 0) {
                final.set(key, nodes)
            } else {
                common.forEach(c => {
                    const subnet = new Set(nodes).add(c)
                    map.set(this.subnetKey(subnet), subnet)
                })
            }
        }

        return map.size === 0 ? final : this.resolveSubnetsRec(map, final)
    }

    private commonNodes(nodes: NodeSet): NodeSet {
        return nodes
            .values()
            .map(node => this.map.get(node)!)
            .reduce((common, connected) => {
                return common.intersection(connected)
            })
    }

    private subnetKey(nodes: NodeSet): string {
        return Array.from(nodes).sort().join(',')
    }
}

export const part1: Part = input => () =>
    Lan.from(input).countGroupsOfThreeWithT().toString()

export const part2: Part = input => () => Lan.from(input).findPassword()

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = '1043'
part2.solution = 'ai,bk,dc,dx,fo,gx,hk,kd,os,uz,xn,yk,zs'

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
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
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', '7', part1(testInput[0])],
                ['Test part 2', 'co,de,ka,ta', part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = string
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
