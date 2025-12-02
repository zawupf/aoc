import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

export const part1: Part = input => () => solve('part1', input)

export const part2: Part = input => () => solve('part2', input)

const countDigits = (n: number): number => Math.floor(Math.log10(n)) + 1

type PartId = 'part1' | 'part2'
const solve = (part: PartId, input: string) => {
    const ranges: [number, number][] = input.split(',').flatMap(range => {
        const [a, b] = range.split('-').map(Number) as [number, number]
        const [da, db] = [countDigits(a), countDigits(b)]
        utils.assert(a <= b, `Invalid range: ${range}`)
        return Array(db - da + 1)
            .fill(0)
            .map(
                (_, i) =>
                    [
                        i === 0 ? a : 10 ** (da + i - 1),
                        i === db - da ? b : 10 ** (da + i) - 1,
                    ] as [number, number],
            )
    })

    let sum = 0
    const ids = new Set<number>()
    for (const [start, end] of ranges) {
        ids.clear()
        const digitCount = countDigits(start)
        const maxSliceCount = part === 'part1' ? 2 : digitCount

        for (let sliceCount = 2; sliceCount <= maxSliceCount; sliceCount++) {
            if (digitCount % sliceCount !== 0) continue

            const sliceSize = digitCount / sliceCount
            const [a, b] = [
                Math.floor(start / 10 ** (sliceSize * (sliceCount - 1))),
                Math.floor(end / 10 ** (sliceSize * (sliceCount - 1))),
            ]

            const f = 10 ** sliceSize
            for (let i = a; i <= b; i += 1) {
                let [n, j] = [0, sliceCount]
                while (j--) n = n * f + i

                if (n >= start && n <= end && !ids.has(n)) {
                    sum += n
                    ids.add(n)
                }
            }
        }
    }

    return sum
}

export const day = import.meta.file.match(/day(\d+)/)![1]!
export const input = await utils.readInputText(day)
part1.solution = 23039913998
part2.solution = 35950619148

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_text([
        '11-22,95-115,998-1012,1188511880-1188511890,222220-222224, 1698522-1698528,446443-446449,38593856-38593862,565653-565659, 824824821-824824827,2121212118-2121212124',
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 1227775554, part1(testInput[0]!)],
                ['Test part 2', 4174379265, part2(testInput[0]!)],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
