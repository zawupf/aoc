import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

export const part1: Part = input => () =>
    calculateAll(
        ...parseChunks(input, lines =>
            utils.transpose(
                lines.map(line => line.trim().split(/\s+/).map(utils.parseInt)),
            ),
        ),
    )

export const part2: Part = input => () =>
    calculateAll(
        ...parseChunks(input, lines =>
            utils
                .transpose(lines.map(line => line.split('')))
                .map(line => line.join(''))
                .join('\n')
                .split(/\n\s*\n/)
                .map(section =>
                    section
                        .trim()
                        .split(/\s*\n\s*/)
                        .map(utils.parseInt),
                ),
        ),
    )

type Op = '+' | '*'

const parseChunks = (
    input: string,
    parseNumbers: (lines: string[]) => number[][],
): [Op[], number[][]] => {
    const lines = input.split('\n').filter(line => line.trim().length > 0)
    const numbers = parseNumbers(lines.slice(0, -1))
    const ops = lines[lines.length - 1]!.trim()
        .split(/\s+/)
        .map(op => op.trim()) as Op[]
    utils.assert(
        ops.every(op => op === '+' || op === '*'),
        'Invalid operation found',
    )
    return [ops, numbers]
}

const calculateAll = (ops: Op[], numbers: number[][]): number =>
    utils
        .zip(ops, numbers)
        .reduce((sum, [op, numbers]) => sum + calculate(op, numbers), 0)

const calculate = (op: Op, numbers: number[]): number => {
    switch (op) {
        case '+':
            return numbers.reduce((a, b) => a + b, 0)
        case '*':
            return numbers.reduce((a, b) => a * b, 1)
        default:
            utils.panic(`Unknown operation: ${op}`)
    }
}

export const day = import.meta.file.match(/day(\d+)/)![1]!
export const input = await utils.readInputText(day, { trim: false })
part1.solution = 5171061464548
part2.solution = 10189959087258

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_text([
        [
            '123 328  51 64 ',
            ' 45 64  387 23 ',
            '  6 98  215 314',
            '*   +   *   +  ',
        ].join('\n'),
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 4277556, part1(testInput[0]!)],
                ['Test part 2', 3263827, part2(testInput[0]!)],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
