import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Value = string
type NewValues = Value[]
type TotalCount = number
type CacheEntry = [NewValues, TotalCount]

type Cache = Record<Value, CacheEntry[]>

function parse(line: string) {
    return line.split(' ')
}

function blink(n: number, value: Value, cache: Cache): CacheEntry {
    console.assert(n > 0)

    let entry = cache[value]?.[n - 1]
    if (entry) {
        return entry
    }

    if (n > 1) {
        const [vs, _] = blink(n - 1, value, cache)
        const newValues: NewValues[] = []
        let count = 0
        for (const v of vs) {
            const [subVs, subCount] = blink(1, v, cache)
            newValues.push(subVs)
            count += subCount
        }

        entry = [newValues.flat(), count]
        cache[value].push(entry)
        console.assert(cache[value].length === n)
        // console.log('blink', n, value, [...entry[0]])
        return entry
    }

    if (value === '0') {
        entry = [['1'], 1]
    } else if (value.length % 2 === 0) {
        const l = value.length / 2
        const [a, b] = [
            value.substring(0, l),
            `${utils.parseInt(value.substring(l))}`,
        ]
        entry = [[a, b], 2]
    } else {
        entry = [[`${utils.parseInt(value) * 2024}`], 1]
    }

    cache[value] = [entry]
    // console.log('blink', n, value, entry)
    return entry
}

function blinkCount(n: number, values: Value[]): TotalCount {
    let sum = 0
    const cache: Cache = {}
    for (const value of values) {
        const [vals, count] = blink(n, value, cache)
        console.log(value, count)
        // console.log('blink', n, value, count, [...vals])
        // console.log([...vals], count)
        sum += count
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
part2.solution = NaN

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
