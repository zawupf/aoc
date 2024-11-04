import type { DayModule, SolutionFactory } from './types'
import * as utils from './utils'

const numbersMap: Record<string, number> = {
    one: 1,
    two: 2,
    three: 3,
    four: 4,
    five: 5,
    six: 6,
    seven: 7,
    eight: 8,
    nine: 9,
}
function toNumber(str: string): number {
    return numbersMap[str] ?? parseInt(str)
}

function getCalibrationSum(digitRegex: RegExp, input: string[]): number {
    const rxA = new RegExp(`.*?(${digitRegex.source})`)
    const rxB = new RegExp(`.*(${digitRegex.source})`)
    return input
        .map(line => {
            const a = toNumber(line.match(rxA)![1])
            const b = toNumber(line.match(rxB)![1])
            return +`${a}${b}`
        })
        .reduce(utils.sum, 0)
}

export const part1: Part = input => async () => getCalibrationSum(/\d/, input)

export const part2: Part = input => async () =>
    getCalibrationSum(/\d|one|two|three|four|five|six|seven|eight|nine/, input)

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 54990
part2.solution = 54473

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([])

    await utils.tests(
        () => utils.test_all(),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
