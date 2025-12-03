import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

export const part1: Part = input => () =>
    input.reduce(utils.sumBy(maxJoltage(2)), 0)

export const part2: Part = input => () =>
    input.reduce(utils.sumBy(maxJoltage(12)), 0)

const maxJoltage =
    (len: number) =>
    (bank: string): number => {
        let result = 0
        let batteries = bank.split('').map(Number)
        let n = len
        while (n--) {
            const maxValue = batteries
                .slice(0, batteries.length - n)
                .reduce(utils.max, 0)
            result = result * 10 + maxValue
            const i = batteries.indexOf(maxValue)
            batteries = batteries.slice(i + 1)
        }
        return result
    }

export const day = import.meta.file.match(/day(\d+)/)![1]!
export const input = await utils.readInputLines(day)
part1.solution = 17613
part2.solution = 175304218462560

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
        987654321111111
        811111111111119
        234234234234278
        818181911112111
        `,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 357, part1(testInput[0]!)],
                ['Test part 2', 3121910778619, part2(testInput[0]!)],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
