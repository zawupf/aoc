import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

function mul(a: string, b: string): number {
    return utils.parseInt(a) * utils.parseInt(b)
}

export const part1: Part = input => async () =>
    input.matchAll(/mul\((\d{1,3}),(\d{1,3})\)/g).reduce(
        utils.sumBy(([_, a, b]) => mul(a, b)),
        0,
    )

export const part2: Part = input => async () =>
    input.matchAll(/mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)/g).reduce(
        ([sum, enabled]: [number, boolean], [m, a, b]): [number, boolean] => {
            switch (m) {
                case 'do()':
                    return [sum, true]
                case "don't()":
                    return [sum, false]
                default:
                    return [sum + (enabled ? mul(a, b) : 0), enabled]
            }
        },
        [0, true],
    )[0]

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputText(day)
part1.solution = 173529487
part2.solution = 99532691

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_text([
        `xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))`,
        `xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 161, part1(testInput[0])],
                ['Test part 2', 48, part2(testInput[1])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
