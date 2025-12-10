import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

export const part1: Part = input => () =>
    input.map(parseMachine).reduce(utils.sumBy(solve1), 0)

export const part2: Part = input => () =>
    input.map(parseMachine).reduce(utils.sumBy(solve2), 0)

type Machine = {
    lights: number
    buttons: number[]
    joltages: number[]
}

function solve1(machine: Machine): number {
    const stack: Machine[] = [machine]
    let current: Machine | undefined
    while ((current = stack.shift())) {
        const { lights, buttons, joltages } = current
        if (lights === 0) return machine.buttons.length - buttons.length

        buttons.forEach((button, idx) => {
            const nextLights = lights ^ button
            const nextButtons = buttons.filter((_, i) => i !== idx)
            stack.push({ lights: nextLights, buttons: nextButtons, joltages })
        })
    }
    utils.unreachable()
}

function solve2(machine: Machine): number {
    utils.notImplemented()
}

function parseMachine(line: string): Machine {
    const [, lights_, buttons_, joltages_] = line.match(
        /^\[(.+)\] (.+) {(.*)}$/,
    )!

    const lights = lights_!
        .split('')
        .reduce((acc, light, idx) => (light === '#' ? acc + 2 ** idx : acc), 0)
    const buttons = [...buttons_?.matchAll(/\((.*?)\)/g)!].map(m =>
        m[1]!.split(',').reduce((acc, bit) => acc + 2 ** Number(bit), 0),
    )
    let joltages: number[] = joltages_!.split(',').map(Number)

    return { lights, buttons, joltages }
}

export const day = import.meta.file.match(/day(\d+)/)![1]!
export const input = await utils.readInputLines(day)
part1.solution = 481
part2.solution = NaN

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
        [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
        [...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
        [.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
        `,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 7, part1(testInput[0]!)],
                ['Test part 2', 33, part2(testInput[0]!)],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
