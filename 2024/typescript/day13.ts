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

function parse(input: string): Game[] {
    return utils.split_sections(input).map(parseGame)
}

function solve(machine: Game, offset: number): number {
    const [a_dx, a_dy] = machine.buttonA
    const [b_dx, b_dy] = machine.buttonB
    const [px, py] = [machine.prize[0] + offset, machine.prize[1] + offset]
    const det = a_dx * b_dy - a_dy * b_dx
    const a = Math.floor((px * b_dy - py * b_dx) / det)
    const b = Math.floor((a_dx * py - a_dy * px) / det)
    return a_dx * a + b_dx * b === px && a_dy * a + b_dy * b === py
        ? a * 3 + b
        : 0
}

export const part1: Part = input => () =>
    parse(input)
        .map(machine => solve(machine, 0))
        .reduce(utils.sum)

export const part2: Part = input => () =>
    parse(input)
        .map(machine => solve(machine, 10000000000000))
        .reduce(utils.sum)

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputText(day)
part1.solution = 35729
part2.solution = 88584689879723

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
