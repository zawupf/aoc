import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Report = number[]

function parseReport(line: string): Report {
    return line.split(' ').map(Number)
}

function isGraduallyChanging(report: Report): boolean {
    let sign = Math.sign(report[1] - report[0])
    if (sign === 0) {
        return false
    }
    for (let i = 1; i < report.length; i++) {
        const diff = report[i] - report[i - 1]
        if (sign !== Math.sign(diff)) {
            return false
        }
        const delta = Math.abs(diff)
        if (delta === 0 || delta > 3) {
            return false
        }
    }
    return true
}

function isGraduallyChangingWithProblemDampener(report: Report): boolean {
    if (isGraduallyChanging(report)) {
        return true
    }
    for (let i = 0; i < report.length; i++) {
        const r = [...report]
        r.splice(i, 1)
        if (isGraduallyChanging(r)) {
            return true
        }
    }
    return false
}

export const part1: Part = input => async () =>
    input.map(parseReport).reduce(
        utils.sumBy(r => (isGraduallyChanging(r) ? 1 : 0)),
        0,
    )

export const part2: Part = input => async () =>
    input.map(parseReport).reduce(
        utils.sumBy(r => (isGraduallyChangingWithProblemDampener(r) ? 1 : 0)),
        0,
    )

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 490
part2.solution = 536

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 2, part1(testInput[0])],
                ['Test part 2', 4, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
