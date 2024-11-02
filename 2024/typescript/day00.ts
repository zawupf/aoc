import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

export const part1: Part = input => async () => utils.notImplemented()

export const part2: Part = input => async () => utils.notImplemented()

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = NaN
part2.solution = NaN

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', NaN, part1(testInput[0])],
                ['Test part 2', NaN, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
