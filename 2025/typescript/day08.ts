import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

export const part1: Part = input => () => solve('part1', input, 1000)

export const part2: Part = input => () => solve('part2', input)

type Pos = [number, number, number]
type Junction = { pos: Pos; circuit?: Set<Junction> }

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
    const junctions = input.map(
        line => ({ pos: line.split(',').map(Number) as Pos } as Junction),
    )

    const distances: [number, Junction, Junction][] = []
    for (let i1 = 0; i1 < junctions.length; i1++) {
        const j1 = junctions[i1]!
        for (let i2 = i1 + 1; i2 < junctions.length; i2++) {
            const j2 = junctions[i2]!
            const dist =
                (j1.pos[0] - j2.pos[0]) ** 2 +
                (j1.pos[1] - j2.pos[1]) ** 2 +
                (j1.pos[2] - j2.pos[2]) ** 2
            distances.push([dist, j1, j2])
        }
    }
    distances.sort((a, b) => a[0] - b[0])

    const circuits: Set<Junction>[] = []
    const done = () => {
        switch (partId) {
            case 'part1':
                return connectionsCount!-- === 0
            case 'part2':
                return (
                    circuits.length === 1 &&
                    circuits[0]!.size === junctions.length
                )
            default:
                utils.unreachable(`Unknown part id: ${partId}`)
        }
    }

    let i = 0
    while (!done()) {
        const [_, j1, j2] = distances[i++]!
        const c1 = j1.circuit
        const c2 = j2.circuit

        if (c1 && c2) {
            if (c1 === c2) continue
            c2.forEach(j => {
                c1.add(j)
                j.circuit = c1
            })
            circuits.splice(circuits.indexOf(c2), 1)
        } else if (c1) {
            c1.add(j2)
            j2.circuit = c1
        } else if (c2) {
            c2.add(j1)
            j1.circuit = c2
        } else {
            const c3 = new Set([j1, j2])
            j1.circuit = c3
            j2.circuit = c3
            circuits.push(c3)
        }
    }

    switch (partId) {
        case 'part1':
            return circuits
                .sort((a, b) => b.size - a.size)
                .slice(0, 3)
                .reduce(
                    utils.multiplyBy(c => c.size),
                    1,
                )
        case 'part2':
            const [, j1, j2] = distances[i - 1]!
            return j1.pos[0] * j2.pos[0]
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
