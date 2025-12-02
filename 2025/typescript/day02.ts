import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

export const part1: Part = input => () => {
    const ranges: [number, number][] = input.split(',').map(range => {
        const [start, end] = range.split('-').map(Number) as [number, number]
        return [start, end]
    })

    let sum = 0
    for (const [start, end] of ranges) {
        for (let n = start; n <= end; n++) {
            const s = n.toString()
            const l = s.length
            if (l % 2 !== 0) {
                n = 10 ** l - 1
                continue
            }

            const m = l / 2
            const left = s.slice(0, m)
            const right = s.slice(m)

            if (left === right) {
                sum += n
                n += 10 ** m - 1
            }
        }
    }

    return sum
}

export const part2: Part = input => () => {
    const ranges: [number, number][] = input.split(',').map(range => {
        const [start, end] = range.split('-').map(Number) as [number, number]
        return [start, end]
    })

    let sum = 0
    for (const [start, end] of ranges) {
        for (let n = start; n <= end; n++) {
            const s = n.toString()
            const l = s.length

            for (let d = 2; d <= l; d++) {
                if (l % d !== 0) {
                    continue
                }

                const m = l / d
                const slices: string[] = []
                for (let i = 0; i < l; i += m) {
                    slices.push(s.slice(i, i + m))
                }

                if (slices.every(slice => slice === slices[0])) {
                    sum += n
                    break
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
