import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Wires = Map<string, number>
type Gates = Map<string, Gate>
type Operation = 'AND' | 'OR' | 'XOR'
type Gate = {
    op: Operation
    inputs: [string, string]
    output: string
}

type Circuit = {
    wires: Wires
    gates: Gates
    bits: number
}

function parse(input: string): Circuit {
    const [wiresChunk, gatesChunk] = utils
        .split_sections(input)
        .map(utils.split_lines)

    const wires = new Map(
        wiresChunk.map(line => {
            const [name, value] = line.split(': ')
            return [name, utils.parseInt(value)]
        }),
    )

    const gates = new Map(
        gatesChunk.map(line => {
            const [in1, op, in2, _, output] = line.split(' ')
            return [output, { op, inputs: [in1, in2], output } as Gate]
        }),
    )

    const bits = gates
        .keys()
        .filter(wire => /^z\d\d$/.test(wire))
        .reduce((count, _) => count + 1, 0)

    return { wires, gates, bits }
}

const calculate = {
    AND: (a: number, b: number) => a & b,
    OR: (a: number, b: number) => a | b,
    XOR: (a: number, b: number) => a ^ b,
}

function evaluate(wire: string, circuit: Circuit): number {
    const { wires, gates } = circuit
    const value = wires.get(wire)
    if (value !== undefined) {
        return value
    }

    const gate = gates.get(wire)!
    const [value1, value2] = gate.inputs.map(w => evaluate(w, circuit))

    const result = calculate[gate.op](value1, value2)
    wires.set(wire, result)
    return result
}

type Prefix = 'x' | 'y' | 'z'

function bitsOf(name: Prefix, circuit: Circuit): number {
    return name === 'z' ? circuit.bits : circuit.bits - 1
}

function wiresOf(name: Prefix, circuit: Circuit): string[] {
    return Array.from({ length: bitsOf(name, circuit) }).map(
        (_, i) => name + i.toString().padStart(2, '0'),
    )
}

function get(name: Prefix, circuit: Circuit): number | null {
    return wiresOf(name, circuit)
        .map(wire => evaluate(wire, circuit))
        .reduce((sum, bit, i) => sum + bit * 2 ** i, 0)
}

function swapOutput(gate1: Gate, gate2: Gate, circuit: Circuit) {
    ;[gate1.output, gate2.output] = [gate2.output, gate1.output]
    circuit.gates.set(gate1.output, gate1)
    circuit.gates.set(gate2.output, gate2)
}

function isInvalidGate(gate: Gate, circuit: Circuit): boolean {
    const isZOutput = gate.output.startsWith('z')
    const isLastZBit =
        isZOutput && parseInt(gate.output.slice(1), 10) === circuit.bits - 1
    const isXYInput = gate.inputs.every(
        wire => wire.startsWith('x') || wire.startsWith('y'),
    )

    return (
        (isZOutput && !isLastZBit && gate.op !== 'XOR') ||
        (!isZOutput && !isXYInput && gate.op === 'XOR')
    )
}

function resolveOutputs(gate: Gate, circuit: Circuit): Gate[] {
    if (gate.output.startsWith('z')) {
        return [gate]
    }

    const nextGates = circuit.gates
        .values()
        .filter(g => g.inputs.includes(gate.output))
        .toArray()
    return nextGates.flatMap(g => resolveOutputs(g, circuit))
}

export const part1: Part = input => () => {
    const circuit = parse(input)
    const result = get('z', circuit)!
    return result.toString()
}

export const part2: Part = input => () => {
    const circuit = parse(input)
    const initialWires = new Map(circuit.wires)

    const invalidGates = circuit.gates
        .values()
        .filter(g => isInvalidGate(g, circuit))
        .toArray()
    const invalidOutputGates = invalidGates.filter(gate =>
        gate.output.startsWith('z'),
    )
    const invalidInnerGates = invalidGates.filter(
        gate => !gate.output.startsWith('z'),
    )
    const resolvedGates = invalidGates
        .filter(gate => !gate.output.startsWith('z'))
        .map(g => resolveOutputs(g, circuit))

    const mapToInvalidOutputs = (
        gates: Gate[],
        i: number,
    ): [number, Gate[]] => {
        const gatesOutputId = gates.map(gate => parseInt(gate.output.slice(1)))
        return [
            i,
            invalidOutputGates.filter(gate =>
                gatesOutputId.includes(parseInt(gate.output.slice(1)) + 1),
            ),
        ]
    }

    const gatesMapping = resolvedGates
        .map(mapToInvalidOutputs)
        .sort(([_i, a], [_j, b]) => a.length - b.length)
        .reduce(
            (
                [map, used]: [[Gate, Gate][], Set<string>],
                [i, gates]: [number, Gate[]],
            ) => {
                const outputs = new Set(
                    gates.map(gate => gate.output),
                ).difference(used)
                console.assert(
                    outputs.size === 1,
                    `Invalid outputs: ${outputs}`,
                )
                const output = outputs.values().next().value!
                used.add(output)
                map.push([invalidInnerGates[i], circuit.gates.get(output)!])
                return [map, used]
            },
            [[], new Set()] as [[Gate, Gate][], Set<string>],
        )[0]

    gatesMapping.forEach(([gate1, gate2]) => swapOutput(gate1, gate2, circuit))

    const xor = (a: number, b: number) =>
        parseInt(
            utils
                .zip(
                    a.toString(2).padStart(circuit.bits, '0').split(''),
                    b.toString(2).padStart(circuit.bits, '0').split(''),
                )
                .map(([x, y]) => (x === y ? '0' : '1'))
                .join(''),
            2,
        )

    const z = get('z', circuit)!
    const expected = get('x', circuit)! + get('y', circuit)!
    const okBits = xor(z, expected).toString(2).replace(/1/g, '').length
    const xFail = 'x' + okBits.toString().padStart(2, '0')
    const yFail = 'y' + okBits.toString().padStart(2, '0')
    const gatesFail = circuit.gates
        .values()
        .filter(g => g.inputs.includes(xFail) && g.inputs.includes(yFail))
        .toArray()
    console.assert(gatesFail.length === 2, `Invalid gates: ${gatesFail}`)

    swapOutput(gatesFail[0], gatesFail[1], circuit)
    circuit.wires = new Map(initialWires)
    const z2 = get('z', circuit)!
    console.assert(z2 === expected, `Invalid output: ${z2} !== ${expected}`)

    const result = [gatesFail, ...gatesMapping]
        .flatMap(gs => gs.map(g => g.output))
        .sort()
        .join(',')

    return result
}

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputText(day)
part1.solution = '49430469426918'
part2.solution = 'fbq,pbv,qff,qnw,qqp,z16,z23,z36'

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
