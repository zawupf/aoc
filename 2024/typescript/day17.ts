import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

const Opcodes = ['adv', 'bxl', 'bst', 'jnz', 'bxc', 'out', 'bdv', 'cdv']
type Instruction = (typeof Opcodes)[number]

type Registers = {
    a: number
    b: number
    c: number
}

type Computer = {
    program: number[]
    pc: number
    r: Registers
    out: number[]
}

function readInstruction(program: number[], pc: number): [Instruction, number] {
    const opcode = program[pc]
    const comboOperand = program[pc + 1]
    return [Opcodes[opcode], comboOperand]
}

function literalValue(literalOperand: number): number {
    return literalOperand
}

function comboValue(comboOperand: number, r: Registers): number {
    if (comboOperand <= 3) {
        return comboOperand
    }

    const index = comboOperand - 4
    const register = Object.keys(r)[index] as keyof Registers
    const value = r[register]
    return value
}

function execute(c: Computer): Computer {
    const [instruction, operand] = readInstruction(c.program, c.pc)
    let next_pc = c.pc + 2
    switch (instruction) {
        case 'adv':
            c.r.a = c.r.a >>> comboValue(operand, c.r)
            break
        case 'bxl':
            c.r.b = c.r.b ^ literalValue(operand)
            break
        case 'bst':
            c.r.b = comboValue(operand, c.r) & 0b111
            break
        case 'jnz':
            if (c.r.a) {
                next_pc = literalValue(operand)
            }
            break
        case 'bxc':
            c.r.b = c.r.b ^ c.r.c
            break
        case 'out':
            c.out.push(comboValue(operand, c.r) & 0b111)
            break
        case 'bdv':
            c.r.b = c.r.a >>> comboValue(operand, c.r)
            break
        case 'cdv':
            c.r.c = c.r.a >>> comboValue(operand, c.r)
            break
        default:
            throw new Error(`Unknown instruction: ${c.program[c.pc]}`)
    }
    c.pc = next_pc
    return c
}

function run(c: Computer): Computer {
    while (c.pc < c.program.length) {
        execute(c)
    }
    return c
}

function findRegisterValue(computer: Computer): number {
    utils.notImplemented()
}

function parse(lines: string[]): Computer {
    const [a, b, c] = lines
        .slice(0, 3)
        .map(line => utils.parseInt(line.match(/\d+/g)![0]))
    const program = lines[4].match(/(\d)/g)!.map(utils.parseInt)
    return { program, pc: 0, r: { a, b, c }, out: [] }
}

export const part1: Part = input => () => run(parse(input)).out.join(',')

export const part2: Part = input => () =>
    findRegisterValue(parse(input)).toString()

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = '4,1,5,3,1,5,3,5,7'
part2.solution = ''

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
Register A: 729
Register B: 0
Register C: 0

Program: 0,1,5,4,3,0
`,
        `
Register A: 2024
Register B: 0
Register C: 0

Program: 0,3,5,4,3,0
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                [
                    'Test 1',
                    '1',
                    () =>
                        run({
                            pc: 0,
                            r: { a: 0, b: 0, c: 9 },
                            program: [2, 6],
                            out: [],
                        }).r.b.toString(),
                ],
                [
                    'Test 2',
                    '0,1,2',
                    () =>
                        run({
                            pc: 0,
                            r: { a: 10, b: 0, c: 0 },
                            program: [5, 0, 5, 1, 5, 4],
                            out: [],
                        }).out.join(','),
                ],
                [
                    'Test 3.1',
                    '4,2,5,6,7,7,7,7,3,1,0',
                    () =>
                        run({
                            pc: 0,
                            r: { a: 2024, b: 0, c: 0 },
                            program: [0, 1, 5, 4, 3, 0],
                            out: [],
                        }).out.join(','),
                ],
                [
                    'Test 3.2',
                    '0',
                    () =>
                        run({
                            pc: 0,
                            r: { a: 2024, b: 0, c: 0 },
                            program: [0, 1, 5, 4, 3, 0],
                            out: [],
                        }).r.a.toString(),
                ],
                [
                    'Test 4',
                    '26',
                    () =>
                        run({
                            pc: 0,
                            r: { a: 0, b: 29, c: 0 },
                            program: [1, 7],
                            out: [],
                        }).r.b.toString(),
                ],
                [
                    'Test 5',
                    '44354',
                    () =>
                        run({
                            pc: 0,
                            r: { a: 0, b: 2024, c: 43690 },
                            program: [4, 0],
                            out: [],
                        }).r.b.toString(),
                ],
            ),
        () =>
            utils.test_all(
                ['Test part 1', '4,6,3,5,6,3,5,2,1,0', part1(testInput[0])],
                ['Test part 2', '117440', part2(testInput[1])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = string
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
