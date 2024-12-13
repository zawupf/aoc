import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Game = {
    buttonA: [number, number]
    buttonB: [number, number]
    prize: [number, number]
}

function extractNumberPair(rx: RegExp, input: string): [number, number] {
    return input.match(rx)!.slice(1, 3).map(utils.parseInt) as [number, number]
}

function parseGame(input: string): Game {
    const lines = utils.split_lines(input)

    const buttonA = extractNumberPair(/X\+(\d+), Y\+(\d+)/, lines[0])
    const buttonB = extractNumberPair(/X\+(\d+), Y\+(\d+)/, lines[1])
    const prize = extractNumberPair(/X=(\d+), Y=(\d+)/, lines[2])

    return { buttonA, buttonB, prize }
}

function fixPrize(game: Game): Game {
    const [px, py] = game.prize
    return { ...game, prize: [px + 10000000000000, py + 10000000000000] }
}

function parse(input: string): Game[] {
    return utils.split_sections(input).map(parseGame)
}

function* solutions(game: Game): Generator<number> {
    const [px, py] = game.prize

    const [a_dx, a_dy] = game.buttonA
    const a_i_max = Math.min(Math.floor(px / a_dx), Math.floor(py / a_dy))

    const [b_dx, b_dy] = game.buttonB
    const b_i_max = Math.min(Math.floor(px / b_dx), Math.floor(py / b_dy))

    for (let i = 0; i <= a_i_max; i++) {
        const [ax, ay] = [a_dx * i, a_dy * i]
        for (let j = 0; j <= b_i_max; j++) {
            const [bx, by] = [b_dx * j, b_dy * j]
            if (ax + bx === px && ay + by === py) {
                yield i * 3 + j
            }
        }
    }
}

function solveGame(game: Game): number {
    const result = Math.min(...solutions(game))
    return result === Infinity ? 0 : result
}

export const part1: Part = input => () =>
    parse(input).reduce(utils.sumBy(solveGame), 0)

export const part2: Part = input => () =>
    parse(input).reduce(
        utils.sumBy(g => solveGame(fixPrize(g))),
        0,
    )

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputText(day)
part1.solution = 35729
part2.solution = NaN

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_text([
        `
Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279
`,
    ])

    await utils.tests(
        () => utils.test_all(['Test part 1', 480, part1(testInput[0])]),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
