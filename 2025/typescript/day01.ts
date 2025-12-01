import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

export const part1: Part = input => () => {
    let count = 0
    let pos = 50
    for (const line of input) {
        const direction = line[0] === 'L' ? -1 : 1
        const distance = parseInt(line.slice(1), 10)
        pos += direction * distance
        pos %= 100
        if (pos % 100 === 0) {
            count++
        }
    }
    return count
}

export const part2: Part = input => () => {
    let count = 0
    let pos = 50
    for (const line of input) {
        const direction = line[0] === 'L' ? -1 : 1
        const distance = parseInt(line.slice(1), 10)
        const overflows = Math.floor(distance / 100)
        const dist = distance % 100

        const nextPos = pos + direction * dist
        const inc =
            nextPos <= -100 ||
            nextPos >= 100 ||
            nextPos === 0 ||
            (Math.sign(pos) !== Math.sign(nextPos) &&
                Math.sign(pos) !== 0 &&
                Math.sign(nextPos) !== 0)
        count += overflows + (inc ? 1 : 0)
        pos = nextPos % 100
    }
    return count
}

export const day = import.meta.file.match(/day(\d+)/)![1]!
export const input = await utils.readInputLines(day)
part1.solution = 1120
part2.solution = 6554

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
        L68
        L30
        R48
        L5
        R60
        L55
        L1
        L99
        R14
        L82
        `,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 3, part1(testInput[0]!)],
                ['Test part 2', 6, part2(testInput[0]!)],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
