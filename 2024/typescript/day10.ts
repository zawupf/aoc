import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Pos = [number, number]
type Grid = number[][]

function parseGrid(lines: string[]): Grid {
    return lines.map(line => line.split('').map(utils.parseInt))
}

function* nextSteps([x, y]: Pos): Generator<Pos> {
    yield [x - 1, y]
    yield [x + 1, y]
    yield [x, y - 1]
    yield [x, y + 1]
}

function isValidStep(a: Pos, b: Pos, grid: Grid): boolean {
    const [[x1, y1], [x2, y2]] = [a, b]
    const [v1, v2] = [grid[y1]?.[x1], grid[y2]?.[x2]]
    const isInside = () => v1 !== undefined && v2 !== undefined
    const length = () => Math.abs(x1 - x2) + Math.abs(y1 - y2)
    const height = () => v2 - v1
    return isInside() && length() === 1 && height() === 1
}

function* walk(start: Pos, grid: Grid): Generator<Pos> {
    if (grid[start[1]][start[0]] === 9) {
        yield start
        return
    }

    const isValid = (p: Pos) => isValidStep(start, p, grid)
    for (const next of nextSteps(start).filter(isValid)) {
        yield* walk(next, grid)
    }
}

function* starts(grid: Grid): Generator<Pos> {
    for (let y = 0; y < grid.length; y++) {
        for (let x = 0; x < grid[y].length; x++) {
            if (grid[y][x] === 0) {
                yield [x, y] as Pos
            }
        }
    }
}

function pathCount(start: Pos, grid: Grid): number {
    return [...walk(start, grid)].length
}

function endCount(start: Pos, grid: Grid): number {
    return new Set(walk(start, grid).map(([x, y]) => `${x},${y}`)).size
}

export const part1: Part = input => () => {
    const grid = parseGrid(input)
    return starts(grid).reduce(
        utils.sumBy(start => endCount(start, grid)),
        0,
    )
}

export const part2: Part = input => () => {
    const grid = parseGrid(input)
    return starts(grid).reduce(
        utils.sumBy(start => pathCount(start, grid)),
        0,
    )
}

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 789
part2.solution = 1735

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 36, part1(testInput[0])],
                ['Test part 2', 81, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
