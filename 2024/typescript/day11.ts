import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Cache = Map<string, number>

function parse(line: string) {
    return utils.split_numbers(line, ' ')
}

function count(stone: number, blinks: number, cache: Cache): number {
    if (blinks === 0) return 1

    const key = `${stone}:${blinks}`

    let result = cache.get(key)
    if (result) {
        return result
    }

    const str = stone.toString()
    if (stone === 0) {
        result = count(1, blinks - 1, cache)
    } else if (str.length % 2 === 0) {
        const m = str.length / 2
        const left = parseInt(str.substring(0, m))
        const right = parseInt(str.substring(m))
        result =
            count(left, blinks - 1, cache) + count(right, blinks - 1, cache)
    } else {
        result = count(stone * 2024, blinks - 1, cache)
    }

    cache.set(key, result)
    return result
}

function sumByCount(stones: number[], blinks: number) {
    const cache: Cache = new Map<string, number>()
    return stones.reduce(
        utils.sumBy(stone => count(stone, blinks, cache)),
        0,
    )
}

export const part1: Part = input => () => sumByCount(parse(input), 25)

export const part2: Part = input => () => sumByCount(parse(input), 75)

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputText(day)
part1.solution = 220722
part2.solution = 261952051690787

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_text([`125 17`])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test blink 1', 3, () => sumByCount(parse(testInput[0]), 1)],
                ['Test blink 2', 4, () => sumByCount(parse(testInput[0]), 2)],
                ['Test blink 3', 5, () => sumByCount(parse(testInput[0]), 3)],
                ['Test blink 4', 9, () => sumByCount(parse(testInput[0]), 4)],
                ['Test blink 5', 13, () => sumByCount(parse(testInput[0]), 5)],
                ['Test blink 6', 22, () => sumByCount(parse(testInput[0]), 6)],
                ['Test part 1', 55312, part1(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
