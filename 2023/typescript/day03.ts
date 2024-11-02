import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

export const day = import.meta.file.match(/day(\d+)/)![1]

type Pos = { x: number; y: number }
type Symbol = { value: string; pos: Pos }
type Number = { value: number; pos: Pos; length: number; symbol: Symbol }

function findSymbols(pos: Pos, length: number, lines: string[]): Symbol[] {
    return [-1, 0, 1].flatMap(
        dy =>
            Array.from({ length: length + 2 }, (_, i) => i - 1)
                .map(dx => {
                    const [x, y] = [pos.x + dx, pos.y + dy]
                    const value = lines[y]?.[x]
                    return value && !'0123456789.'.includes(value)
                        ? { value, pos: { x, y } }
                        : null
                })
                .filter(Boolean) as Symbol[],
    )
}

function makeNumber(value: string, pos: Pos, lines: string[]): Number {
    const length = value.length
    const symbols = findSymbols(pos, length, lines)
    console.assert(symbols.length < 2, 'More than 1 symbol', value)
    return { value: parseInt(value), pos, length, symbol: symbols[0] }
}

function findNumbers(lines: string[]): Number[] {
    return lines.flatMap((line, y) =>
        line
            .matchAll(/\d+/g)
            .map(match => makeNumber(match[0], { x: match.index, y }, lines))
            .toArray(),
    )
}

export const part1: Part = function (input) {
    return async function () {
        return findNumbers(input)
            .filter(n => n.symbol)
            .reduce((sum, n) => sum + n.value, 0)
    }
}

export const part2: Part = function (input) {
    return async function () {
        return findNumbers(input)
            .reduce((gears, number) => {
                if (number.symbol?.value === '*') {
                    const key = `${number.symbol.pos.x},${number.symbol.pos.y}`
                    const numbers = gears.get(key) || []
                    numbers.push(number.value)
                    gears.set(key, numbers)
                }
                return gears
            }, new Map<string, number[]>())
            .values()
            .map(numbers =>
                numbers.length === 2 ? numbers.reduce((a, b) => a * b, 1) : 0,
            )
            .reduce((a, b) => a + b, 0)
    }
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
export const input = await utils.readInputLines(day)
part1.solution = 557705
part2.solution = 84266818

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 4361, part1(testInput[0])],
                ['Test part 2', 467835, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}
