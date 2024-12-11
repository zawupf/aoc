import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Cache = Map<string, string[]>

function parse(line: string) {
    return line.split(' ')
}

function blink(n: number, value: string, cache: Cache): string[] {
    const key = `${n}:${value}`

    let entry = cache.get(key)
    if (entry) {
        return entry
    }

    const values: string[] = n === 1 ? [value] : blink(n - 1, value, cache)
    for (let i = 0; i < values.length; i++) {
        const v = values[i]
        if (v === '0') {
            values[i] = '1'
        } else if (v.length % 2 === 0) {
            const l = v.length / 2
            const vs = [v.substring(0, l), `${utils.parseInt(v.substring(l))}`]
            values.splice(i, 1, ...vs)
            i++
        } else {
            values[i] = `${utils.parseInt(v) * 2024}`
        }
    }
    entry = values
    cache.set(key, values)
    // console.log(n, value, '->', i, values.length)

    return entry
}

function blinkCount(n: number, values: string[]): number {
    let sum = 0
    const cache: Cache = new Map()
    for (const value of values) {
        const entry = blink(n, value, cache)
        console.log(value, entry.length)
        // console.log('blink', n, value, count, [...vals])
        // console.log([...vals], count)
        sum += entry.length
    }
    return sum
    // return values.reduce(
    //     utils.sumBy(value => blink(n, value, {})[1]),
    //     0,
    // )
}

export const part1: Part = input => () => blinkCount(25, parse(input))

export const part2: Part = input => () => blinkCount(75, parse(input))

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputText(day)
part1.solution = 220722
part2.solution = 0

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_text([`125 17`])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test blink 1', 3, () => blinkCount(1, parse(testInput[0]))],
                ['Test blink 2', 4, () => blinkCount(2, parse(testInput[0]))],
                ['Test blink 3', 5, () => blinkCount(3, parse(testInput[0]))],
                ['Test blink 4', 9, () => blinkCount(4, parse(testInput[0]))],
                ['Test blink 5', 13, () => blinkCount(5, parse(testInput[0]))],
                ['Test blink 6', 22, () => blinkCount(6, parse(testInput[0]))],
                ['Test part 1', 55312, part1(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
