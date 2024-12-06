import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

const XMAS = 'XMAS'

type CounterFn = (lines: string[], x: number, y: number) => number

function count_XMAS(lines: string[], x: number, y: number): number {
    if (lines[y][x] !== XMAS[0]) {
        return 0
    }

    let directions: [number, number][] = [
        [1, 0],
        [-1, 0],
        [0, 1],
        [0, -1],
        [1, 1],
        [-1, -1],
        [1, -1],
        [-1, 1],
    ]

    return Array.from(XMAS.slice(1), (c, i) => [c, i + 1] as const).reduce(
        (directions, [c, i]) =>
            directions.filter(
                ([dx, dy]) => lines[y + dy * i]?.[x + dx * i] === c,
            ),
        directions,
    ).length
}

function count_MAS_cross(lines: string[], x: number, y: number): number {
    const diag1 = () => lines[y + 1]?.[x + 1] + lines[y - 1]?.[x - 1]
    const diag2 = () => lines[y + 1]?.[x - 1] + lines[y - 1]?.[x + 1]
    const isMS = (s: string) => s === 'MS' || s === 'SM'

    return lines[y][x] === 'A' && isMS(diag1()) && isMS(diag2()) ? 1 : 0
}

function count(lines: string[], counter: CounterFn): number {
    return lines.reduce(
        (sum, _, y) =>
            _.split('').reduce((sum, _, x) => sum + counter(lines, x, y), sum),
        0,
    )
}

export const part1: Part = input => () => count(input, count_XMAS)

export const part2: Part = input => () => count(input, count_MAS_cross)

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 2517
part2.solution = 1960

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 18, part1(testInput[0])],
                ['Test part 2', 9, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
