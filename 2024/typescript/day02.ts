import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Report = number[]

function parseReport(line: string): Report {
    return line.split(' ').map(utils.parseInt)
}

function isGraduallyChanging(report: Report): boolean {
    const sign = Math.sign(report[1] - report[0])
    return (
        sign !== 0 &&
        report.slice(1).every((v, i) => {
            const diff = v - report[i]
            const delta = Math.abs(diff)
            return sign === Math.sign(diff) && delta !== 0 && delta <= 3
        })
    )
}

function* generateSubsets(report: Report): Generator<Report> {
    for (let i = 0; i < report.length; i++) {
        const r = [...report]
        r.splice(i, 1)
        yield r
    }
}

function isGraduallyChangingWithProblemDampener(report: Report): boolean {
    return (
        isGraduallyChanging(report) ||
        generateSubsets(report).some(isGraduallyChanging)
    )
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
