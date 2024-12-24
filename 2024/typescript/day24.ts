import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Wires = Map<string, number | null>
type Operation = 'AND' | 'OR' | 'XOR'
type Gate = {
    op: Operation
    inputs: [string, string]
    output: string
}

type Circuit = {
    wires: Wires
    gates: Gate[]
}

function parse(input: string): Circuit {
    const [wiresChunk, gatesChunk] = utils
        .split_sections(input)
        .map(utils.split_lines)
    const wires = new Map(
        wiresChunk.map(line => {
            const [name, value] = line.split(': ')
            return [name, parseInt(value)]
        }),
    )
    const gates = gatesChunk.map(line => {
        const [in1, op, in2, _, output] = line.split(' ')
        ;[in1, in2, output].forEach(wire => {
            if (!wires.has(wire)) {
                wires.set(wire, null as unknown as number)
            }
        })
        return { op, inputs: [in1, in2], output } as Gate
    })
    return { wires, gates }
}

function evaluate(wire: string, circuit: Circuit): number {
    const { wires, gates } = circuit
    const value = wires.get(wire)
    if (value != null) {
        return value
    }

    const gate = gates.find(gate => gate.output === wire)!
    const [value1, value2] = gate.inputs.map(w => evaluate(w, circuit))
    const result = {
        AND: value1 & value2,
        OR: value1 | value2,
        XOR: value1 ^ value2,
    }[gate.op]
    wires.set(wire, result)
    return result
}

export const part1: Part = input => () => {
    const circuit = parse(input)
    const zs = circuit.wires
        .keys()
        .filter(wire => wire.startsWith('z'))
        .toArray()
        .sort()
    const result = zs
        .map(wire => evaluate(wire, circuit))
        .reduce((sum, bit, i) => sum + bit * 2 ** i, 0)
    return result.toString()
}

export const part2: Part = input => () => {
    const circuit = parse(input)
    const bits =
        circuit.wires
            .keys()
            .filter(wire => wire.startsWith('z'))
            .toArray().length - 1
    return 'WIP'
}

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputText(day)
part1.solution = '49430469426918'
part2.solution = 'TODO'

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_text([
        `
x00: 1
x01: 1
x02: 1
y00: 0
y01: 1
y02: 0

x00 AND y00 -> z00
x01 XOR y01 -> z01
x02 OR y02 -> z02
`,
        `
x00: 1
x01: 0
x02: 1
x03: 1
x04: 0
y00: 1
y01: 1
y02: 1
y03: 1
y04: 1

ntg XOR fgs -> mjb
y02 OR x01 -> tnw
kwq OR kpj -> z05
x00 OR x03 -> fst
tgd XOR rvg -> z01
vdt OR tnw -> bfw
bfw AND frj -> z10
ffh OR nrd -> bqk
y00 AND y03 -> djm
y03 OR y00 -> psh
bqk OR frj -> z08
tnw OR fst -> frj
gnj AND tgd -> z11
bfw XOR mjb -> z00
x03 OR x00 -> vdt
gnj AND wpb -> z02
x04 AND y00 -> kjc
djm OR pbm -> qhw
nrd AND vdt -> hwm
kjc AND fst -> rvg
y04 OR y02 -> fgs
y01 AND x02 -> pbm
ntg OR kjc -> kwq
psh XOR fgs -> tgd
qhw XOR tgd -> z09
pbm OR djm -> kpj
x03 XOR y03 -> ffh
x00 XOR y04 -> ntg
bfw OR bqk -> z06
nrd XOR fgs -> wpb
frj XOR qhw -> z04
bqk OR frj -> z07
y03 OR x01 -> nrd
hwm AND bqk -> z03
tgd XOR rvg -> z12
tnw OR pbm -> gnj
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', '4', part1(testInput[0])],
                ['Test part 1', '2024', part1(testInput[1])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = string
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
