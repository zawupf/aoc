import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Equation = [number, number[]]
type Operator = '+' | '*' | '||'

function apply(operator: Operator, a: number, b: number): number {
    switch (operator) {
        case '+':
            return a + b
        case '*':
            return a * b
        case '||':
            return parseInt(`${a}${b}`)
    }
}

function solveWith(operators: Operator[]): (e: Equation) => Generator<number> {
    function* solve(equation: Equation): Generator<number> {
        const [result, [a, b, ...rest]] = equation
        if (b === undefined) {
            if (a === result) {
                yield result
            }
            return
        }

        for (const operator of operators) {
            const value = apply(operator, a, b)
            if (value <= result) {
                yield* solve([result, [value, ...rest]])
            }
        }
    }
    return solve
}

function parseEquation(line: string): Equation {
    const [resultChunk, numbersChunk] = line.split(': ')
    return [utils.parseInt(resultChunk), utils.split_numbers(numbersChunk, ' ')]
}

export const part1: Part = input => () =>
    input.map(parseEquation).reduce(
        utils.sumBy(e => (solveWith(['+', '*'])(e).next().done ? 0 : e[0])),
        0,
    )

export const part2: Part = input => () =>
    input.map(parseEquation).reduce(
        utils.sumBy(e =>
            solveWith(['+', '*', '||'])(e).next().done ? 0 : e[0],
        ),
        0,
    )

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 1260333054159
part2.solution = 162042343638683

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 3749, part1(testInput[0])],
                ['Test part 2', 11387, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
