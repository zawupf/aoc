import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

export const part1: Part = input => () => solve('part1', input, 1000)

export const part2: Part = input => () => solve('part2', input)

type Pos = [number, number, number]

type PartId = 'part1' | 'part2'
function solve(
    partId: 'part1',
    input: string[],
    connectionsCount: number,
): number
function solve(partId: 'part2', input: string[]): number
function solve(
    partId: PartId,
    input: string[],
    connectionsCount?: number,
): number {
    const positions: Pos[] = input.map(
        line => line.split(',').map(Number) as Pos,
    )

    const n = positions.length
    const edges: [number, number, number][] = []
    for (let i1 = 0; i1 < n; i1++) {
        const p1 = positions[i1]!
        for (let i2 = i1 + 1; i2 < n; i2++) {
            const p2 = positions[i2]!
            const dx = p1[0] - p2[0]
            const dy = p1[1] - p2[1]
            const dz = p1[2] - p2[2]
            const dist = dx * dx + dy * dy + dz * dz
            edges.push([dist, i1, i2])
        }
    }
    const distances = utils.PriorityQueue.from(edges, (a, b) => a[0] - b[0])

    const uf = new utils.UnionFind(n)
    let stepsLeft = partId === 'part1' ? connectionsCount! : 0
    let lastEdge: [number, number, number] | undefined

    while (true) {
        if (
            (partId === 'part1' && stepsLeft === 0) ||
            (partId === 'part2' && uf.componentCount === 1)
        )
            break

        const currentEdge = distances.pop()
        if (!currentEdge) break
        lastEdge = currentEdge
        const [, i, j] = currentEdge
        if (partId === 'part1') stepsLeft -= 1
        uf.unite(i, j)
    }

    switch (partId) {
        case 'part1': {
            const sizes = uf.sizes()
            const pq = utils.PriorityQueue.from(sizes, (a, b) => b - a)
            return pq.pop()! * pq.pop()! * pq.pop()!
        }
        case 'part2': {
            const [, i, j] = lastEdge!
            return positions[i]![0] * positions[j]![0]
        }
        default:
            utils.unreachable(`Unknown part id: ${partId}`)
    }
}

export const day = import.meta.file.match(/day(\d+)/)![1]!
export const input = await utils.readInputLines(day)
part1.solution = 97384
part2.solution = 9003685096

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
        162,817,812
        57,618,57
        906,360,560
        592,479,940
        352,342,300
        466,668,158
        542,29,236
        431,825,988
        739,650,466
        52,470,668
        216,146,977
        819,987,18
        117,168,530
        805,96,715
        346,949,466
        970,615,88
        941,993,340
        862,61,35
        984,92,344
        425,690,689
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 40, () => solve('part1', testInput[0]!, 10)],
                ['Test part 2', 25272, part2(testInput[0]!)],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
