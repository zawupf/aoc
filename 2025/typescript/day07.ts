import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

export const part1: Part = input => () => solve('part1', input)

export const part2: Part = input => () => solve('part2', input)

type PartId = 'part1' | 'part2'
const solve = (partId: PartId, input: string[]) => {
    const [beams, ...rows] = input.map(line => line.split(''))
    if (beams === undefined) return 0

    const len = beams.length
    const beamsCount: number[] = beams.map(c => (c === '.' ? 0 : 1))

    let splitCount = 0
    for (let row of rows) {
        for (let i = 0; i < len; i++) {
            if (row[i] === '.') continue
            const n = beamsCount[i]!
            if (n === 0) continue

            splitCount++
            beamsCount[i] = 0
            beamsCount[i - 1]! += n
            beamsCount[i + 1]! += n
        }
    }
    return partId === 'part1' ? splitCount : beamsCount.reduce(utils.sum, 0)
}

export const day = import.meta.file.match(/day(\d+)/)![1]!
export const input = await utils.readInputLines(day)
part1.solution = 1672
part2.solution = 231229866702355

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
        .......S.......
        ...............
        .......^.......
        ...............
        ......^.^......
        ...............
        .....^.^.^.....
        ...............
        ....^.^...^....
        ...............
        ...^.^...^.^...
        ...............
        ..^...^.....^..
        ...............
        .^.^.^.^.^...^.
        ...............
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 21, part1(testInput[0]!)],
                ['Test part 2', 40, part2(testInput[0]!)],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
