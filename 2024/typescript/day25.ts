import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Lock = number[]
type Key = number[]
type Data = {
    locks: Lock[]
    keys: Key[]
}

function parse(input: string): Data {
    return utils.as_lines(utils.split_sections(input)).reduce(
        (data, lines) => {
            const isLock = lines[0] == '#####'
            const heights = utils
                .transpose(lines.slice(1, 6).map(line => line.split('')))
                .map(row =>
                    row.reduce((height, c) => height + (c == '#' ? 1 : 0), 0),
                )
            if (isLock) {
                data.locks.push(heights)
            } else {
                data.keys.push(heights)
            }
            return data
        },
        { locks: [], keys: [] } as Data,
    )
}

function isFitting(lock: Lock, key: Key): boolean {
    return utils.zip(lock, key).every(([l, k]) => l + k <= 5)
}

export const part1: Part = input => () => {
    const { locks, keys } = parse(input)
    return locks
        .map(lock => keys.filter(key => isFitting(lock, key)).length)
        .reduce(utils.sum)
}

export const part2: Part = input => () => utils.notImplemented()

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputText(day)
part1.solution = 3077
part2.solution = NaN

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_text([
        `
#####
.####
.####
.####
.#.#.
.#...
.....

#####
##.##
.#.##
...##
...#.
...#.
.....

.....
#....
#....
#...#
#.#.#
#.###
#####

.....
.....
#.#..
###..
###.#
###.#
#####

.....
.....
.....
#....
#.#..
#.#.#
#####
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 3, part1(testInput[0])],
                ['Test part 2', NaN, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
