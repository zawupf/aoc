import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Pair = [number, number]
type Lists = [number[], number[]]

function parseInput(input: string[]): Lists {
    return utils.unzip(
        input.map(line => line.split(/\s+/).map(utils.parseInt)) as Pair[],
    )
}

export const part1: Part = input => () =>
    utils.zip(...(parseInput(input).map(utils.sortNumbers) as Lists)).reduce(
        utils.sumBy(([l, r]) => Math.abs(l - r)),
        0,
    )

export const part2: Part = input => () => {
    const [leftCounts, rightCounts] = parseInput(input).map(list =>
        list.reduce(utils.count, {} as Record<number, number>),
    )
    return Object.entries(leftCounts).reduce(
        utils.sumBy(([k, v]) => {
            const n = utils.parseInt(k)
            return v * n * (rightCounts[n] ?? 0)
        }),
        0,
    )
}

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 1651298
part2.solution = 21306195

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `3   4
4   3
2   5
1   3
3   9
3   3
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 11, part1(testInput[0])],
                ['Test part 2', 31, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
