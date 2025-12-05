import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

export const part1: Part = input => () => {
    const [rangesInput, idsInput] = utils.as_lines(utils.split_sections(input))
    const ranges = rangesInput!.map(
        line => line.split('-').map(Number) as [number, number],
    )
    return idsInput!.map(Number).reduce(
        utils.sumBy(id =>
            ranges.some(([start, end]) => id >= start && id <= end) ? 1 : 0,
        ),
        0,
    )
}

export const part2: Part = input => () =>
    utils
        .split_lines(utils.split_sections(input)[0]!)
        .map(line => line.split('-').map(Number) as [number, number])
        .sort(([a0, a1], [b0, b1]) => (a0 !== b0 ? a0 - b0 : a1 - b1))
        .reduce((merged, current) => {
            if (merged.length === 0) return [current]

            const [currentStart, currentEnd] = current
            const [lastStart, lastEnd] = merged[merged.length - 1]!
            if (currentStart <= lastEnd + 1)
                merged[merged.length - 1] = [
                    lastStart,
                    Math.max(lastEnd, currentEnd),
                ]
            else merged.push(current)

            return merged
        }, [] as [number, number][])
        .reduce(
            utils.sumBy(([a, b]) => b - a + 1),
            0,
        )

export const day = import.meta.file.match(/day(\d+)/)![1]!
export const input = await utils.readInputText(day)
part1.solution = 613
part2.solution = 336495597913098

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_text([
        `
        3-5
        10-14
        16-20
        12-18

        1
        5
        8
        11
        17
        32
        `,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 3, part1(testInput[0]!)],
                ['Test part 2', 14, part2(testInput[0]!)],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
