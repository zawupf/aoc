import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Name = string
type Pos = [number, number]
type Region = {
    name: Name
    fields: Map<string, Pos>
}
type Grid = Name[][]

function parseGrid(input: string[]): Grid {
    return input.map(row => row.split(''))
}

function scanRegions(_grid: Grid): Region[] {
    const grid = _grid.map(row => [...row])
    const regions: Region[] = []
    for (let y = 0; y < grid.length; y++) {
        for (let x = 0; x < grid[y].length; x++) {
            if (grid[y][x] === '.') {
                continue
            }

            const region = scanRegion([x, y], grid)
            regions.push(region)
        }
    }
    return regions
}

function scanRegion([x, y]: Pos, grid: Grid): Region {
    const name = grid[y][x]
    const fields: Map<string, Pos> = new Map()

    const queue: Pos[] = [[x, y]]
    while (queue.length > 0) {
        const p = queue.pop()!
        const [x, y] = p
        fields.set(posKey(p), p)
        grid[y][x] = '.'
        queue.push(
            ...neighbors([x, y]).filter(([x, y]) => grid[y]?.[x] === name),
        )
    }

    return { name, fields }
}

function posKey([x, y]: Pos): string {
    return `${x},${y}`
}

function* neighbors([x, y]: Pos): Generator<Pos> {
    yield [x - 1, y]
    yield [x + 1, y]
    yield [x, y - 1]
    yield [x, y + 1]
}

function area(region: Region): number {
    return region.fields.size
}

function perimeter(region: Region): number {
    let result = 0
    for (const [x, y] of region.fields.values()) {
        const outsideCount = neighbors([x, y])
            .filter(n => !region.fields.has(posKey(n)))
            .reduce((a, _) => a + 1, 0)
        result += outsideCount
    }
    return result
}

function sideCount(region: Region): number {
    const edges: [string, number][] = []
    for (const [x, y] of region.fields.values()) {
        edges.push(
            ...neighbors([x, y])
                .filter(n => !region.fields.has(posKey(n)))
                .map(([x2, y2]) => {
                    if (x2 === x) {
                        return [`h|y=${y}|d=${y2 - y}`, x] as [string, number]
                    }
                    console.assert(y2 === y, 'Invalid edge')
                    return [`v|x=${x}|d=${x2 - x}`, y] as [string, number]
                }),
        )
    }

    const edgesBySameRowOrColumn = edges.reduce((groups, [key, value]) => {
        groups[key] = groups[key] || []
        groups[key].push(value)
        return groups
    }, {} as Record<string, number[]>)

    const sidesCountBySameRowOrColumn = Object.entries(
        edgesBySameRowOrColumn,
    ).reduce((groupCounts, [key, values]) => {
        const counts = values
            .sort((a, b) => a - b)
            .reduce((count, value, i, values) => {
                if (i === 0 || values[i - 1] + 1 === value) {
                    return count
                }
                return count + 1
            }, 1)
        groupCounts[key] = counts
        return groupCounts
    }, {} as Record<string, number>)
    const result = Object.values(sidesCountBySameRowOrColumn).reduce(
        utils.sum,
        0,
    )

    return result
}

export const part1: Part = input => () =>
    scanRegions(parseGrid(input))
        .map(r => area(r) * perimeter(r))
        .reduce(utils.sum, 0)

export const part2: Part = input => () =>
    scanRegions(parseGrid(input))
        .map(r => area(r) * sideCount(r))
        .reduce(utils.sum, 0)

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 1381056
part2.solution = 834828

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
AAAA
BBCD
BBCC
EEEC
`,
        `
OOOOO
OXOXO
OOOOO
OXOXO
OOOOO
`,
        `
RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE
`,
        `
EEEEE
EXXXX
EEEEE
EXXXX
EEEEE
`,
        `
AAAAAA
AAABBA
AAABBA
ABBAAA
ABBAAA
AAAAAA
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 140, part1(testInput[0])],
                ['Test part 1', 772, part1(testInput[1])],
                ['Test part 1', 1930, part1(testInput[2])],
                ['Test part 2', 80, part2(testInput[0])],
                ['Test part 2', 436, part2(testInput[1])],
                ['Test part 2', 236, part2(testInput[3])],
                ['Test part 2', 368, part2(testInput[4])],
                ['Test part 2', 1206, part2(testInput[2])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
