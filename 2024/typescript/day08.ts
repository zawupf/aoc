import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Pos = [number, number]
type Grid = {
    width: number
    height: number
    antennas: Record<string, Pos[]>
}

function* pairs(ps: Pos[]): Generator<[Pos, Pos]> {
    const [p, ...ps2] = ps
    yield* ps2.map(p2 => [p, p2] as [Pos, Pos])
    if (ps2.length > 1) {
        yield* pairs(ps2)
    }
}

function isInside([x, y]: Pos, grid: Grid) {
    return x >= 0 && x < grid.width && y >= 0 && y < grid.height
}

function deltas([x1, y1]: Pos, [x2, y2]: Pos): Pos {
    return [x2 - x1, y2 - y1]
}

function* antinodesInLine(p: Pos, [dx, dy]: Pos, grid: Grid): Generator<Pos> {
    while (isInside(p, grid)) {
        yield p
        p = [p[0] + dx, p[1] + dy]
    }
}

type FindAntinodesFn = (pair: [Pos, Pos], grid: Grid) => Pos[]

const pureAntinodes: FindAntinodesFn = ([a, b], grid) => {
    const [dx, dy] = deltas(a, b)
    return [
        ...antinodesInLine(a, [-dx, -dy], grid).drop(1).take(1),
        ...antinodesInLine(b, [dx, dy], grid).drop(1).take(1),
    ]
}

const harmonicAntinodes: FindAntinodesFn = ([a, b], grid) => {
    const [dx, dy] = deltas(a, b)
    return [
        ...antinodesInLine(a, [-dx, -dy], grid),
        ...antinodesInLine(b, [dx, dy], grid),
    ]
}

function antinodesCount(grid: Grid, findAntinodes: FindAntinodesFn): number {
    const antinodes: Set<string> = new Set()
    for (const positions of Object.values(grid.antennas)) {
        for (const pair of pairs(positions)) {
            for (const antinode of findAntinodes(pair, grid)) {
                antinodes.add(antinode.join(','))
            }
        }
    }

    return antinodes.size
}

function parse(lines: string[]): Grid {
    const height = lines.length
    const width = lines[0].length
    const antennas: Record<string, Pos[]> = lines.reduce(groupByAntennaName, {})
    return { width, height, antennas }

    function groupByAntennaName(
        antennas: Record<string, Pos[]>,
        line: string,
        y: number,
    ) {
        return line.split('').reduce((antennas, name, x) => {
            if (name !== '.') {
                antennas[name] = antennas[name] ?? []
                antennas[name].push([x, y])
            }
            return antennas
        }, antennas)
    }
}

export const part1: Part = input => () =>
    antinodesCount(parse(input), pureAntinodes)

export const part2: Part = input => () =>
    antinodesCount(parse(input), harmonicAntinodes)

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 318
part2.solution = 1126

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 14, part1(testInput[0])],
                ['Test part 2', 34, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
