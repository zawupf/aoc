import * as path from 'path'
import {
    ansi,
    AssertionError,
    NotImplementedError,
    UnreachableError,
    type DayModule,
    type Input,
    type Solution,
    type SolutionFactory,
    type SolutionFun,
} from './types'

export type HumanizeOptions = {
    prefix: string
    suffix: string
    colorize: boolean
}

const defaultOptions: HumanizeOptions = {
    prefix: '[ ',
    suffix: ' ]',
    colorize: true,
}
const noDecorationOptions: HumanizeOptions = {
    prefix: '',
    suffix: '',
    colorize: false,
}

export function humanize(
    nanoseconds: number,
    options: Partial<HumanizeOptions> | false = {},
): string {
    const durationMs = nanoseconds / 1_000_000
    const durationSec = Math.floor(durationMs / 1000)
    const hours = Math.floor(durationSec / 3600)
    const minutes = Math.floor((durationSec % 3600) / 60)
    const seconds = durationSec % 60
    const milliseconds = durationMs % 1000

    const parts = []
    if (hours > 0) parts.push(`${hours}h`)
    if (minutes > 0 || parts.length) parts.push(`${minutes}m`)
    if (seconds > 0 || parts.length) parts.push(`${seconds}s`)
    if (milliseconds > 0 || parts.length)
        parts.push(`${milliseconds.toFixed(3)}ms`)

    const result = parts.slice(0, 2).join(':')

    const { prefix, suffix, colorize } =
        options === false
            ? noDecorationOptions
            : { ...defaultOptions, ...options }
    if (!colorize) {
        return `${prefix}${result}${suffix}`
    }

    const color =
        durationMs < 300
            ? ansi.green
            : durationMs < 1000
            ? ansi.yellow
            : ansi.red
    return `${color}${prefix}${result}${suffix}${ansi.clear}`
}

function findInputFile(day: string): string {
    return path.normalize(path.join(__dirname, `../_inputs/Day${day}.txt`))
}

export async function readInputText(day: string): Promise<string> {
    const inputFile = findInputFile(day)
    const content = await Bun.file(inputFile).text()
    return content.trim()
}

export async function readInputLines(day: string): Promise<string[]> {
    const content = await readInputText(day)
    const lines = content.split('\n')
    return lines.map(line => line.trim())
}

export function split_numbers(
    input: string,
    separator: string | RegExp,
    limit?: number,
): number[] {
    return input
        .trim()
        .split(separator, limit)
        .map(chunk => parseInt(chunk.trim()))
}

export function split_lines(input: string): string[] {
    return input
        .trim()
        .split('\n')
        .map(line => line.trim())
}

export function split_sections(input: string): string[] {
    return input
        .trim()
        .split('\n\n')
        .map(section => section.trim())
}

export function as_lines(inputs: string[]): string[][] {
    return inputs.map(split_lines)
}

export function as_text(inputs: string[]): string[] {
    return inputs.map(input => input.trim())
}

export type TestStatus = 'passed' | 'failed' | 'skipped'

export function isStatusOk(status: TestStatus): boolean {
    return status !== 'failed'
}

export function test_run<T extends Solution>(
    title: string,
    expected: T,
    fn: SolutionFun<T>,
): TestStatus {
    try {
        if (Number.isNaN(expected) || expected === '') {
            notImplemented()
        }

        const start = Bun.nanoseconds()
        const result = fn()
        const duration = Bun.nanoseconds() - start

        const equal = Bun.deepEquals(result, expected)
        if (equal) {
            console.log(`✅ ${title}: ${result} ${humanize(duration)}`)
        } else {
            const first = `❌ ${title}: Expected`
            const second = 'but got'.padStart(Bun.stringWidth(first))
            console.log(
                `${first} ${ansi.marked(expected)}\n` +
                    `${second} ${ansi.marked(result)}`,
            )
        }
        return equal ? 'passed' : 'failed'
    } catch (e) {
        const { name, message = 'Unknown error' } = e as Error
        switch (name) {
            case 'NotImplementedError':
                console.log(`${ansi.gray}🚧 ${title}: ${message}${ansi.clear}`)
                return 'skipped'
            default:
                console.log(`💥 ${title}: ${ansi.red}${message}${ansi.clear}`)
                console.error(e)
                return 'failed'
        }
    }
}

type TestArgs<T extends Solution> = [string, T, SolutionFun<T>]
type TestResult = PromiseSettledResult<TestStatus>

function isTestOk(result: TestResult) {
    return result.status === 'fulfilled' && isStatusOk(result.value)
}

export async function test_all<T extends Solution>(
    ...tests: TestArgs<T>[]
): Promise<boolean> {
    const results = await Promise.allSettled(
        tests.map(args => test_run(...args)),
    )

    return results.every(isTestOk)
}

export async function test_day<T extends Solution, In extends Input>({
    day,
    part1,
    part2,
    input,
    main,
}: DayModule<T, In>): Promise<boolean> {
    const title = (n: number) => (main ? `Part ${n}` : `Day ${day} (part ${n})`)
    const tests: [SolutionFactory<T, In>, TestArgs<T>][] = [
        [part1, [title(1), part1.solution, part1(input)]],
        [part2, [title(2), part2.solution, part2(input)]],
    ]
    const activeTests = tests
        .filter(([f, _]) => !f.skip)
        .map(([_, args]) => args)
    const result = await test_all(...activeTests)

    tests
        .filter(([f, _]) => f.skip)
        .map(([_, [title, ...__]]) => title)
        .forEach(title => {
            console.log(
                `${ansi.gray}🚧 ${title}: Explicitly skipped due to duration${ansi.clear}`,
            )
        })

    return result
}

type TestFactory = () => Promise<boolean>
export async function tests(...tests: TestFactory[]) {
    const start = Bun.nanoseconds()
    for (const test of tests) {
        if (!(await test())) {
            break
        }
    }
    const duration = Bun.nanoseconds() - start
    console.log(`🏁 Tests completed in ${humanize(duration)}`)
}

export function notImplemented(): never {
    throw new NotImplementedError()
}

export function unreachable(message?: string): never {
    throw new UnreachableError(message)
}

export function assert(condition: boolean, message: string): void | never {
    if (!condition) {
        throw new AssertionError(message)
    }
}

export function dump<T>(value: T, fn?: any): T {
    console.log(fn instanceof Function ? fn(value) : value)
    return value
}

export function dump_map<T, U>(fn: (value: T) => U): (value: T) => T {
    return value => {
        console.log(fn(value))
        return value
    }
}

export function parseInt(value: string): number {
    return globalThis.parseInt(value, 10)
}

export function sum(sum: number, value: number): number {
    return sum + value
}

export function sumBy<T>(
    fn: (value: T) => number,
): (sum: number, value: T) => number {
    return (sum, value) => sum + fn(value)
}

export function multiply(sum: number, value: number): number {
    return sum * value
}

export function multiplyBy<T>(
    fn: (value: T) => number,
): (sum: number, value: T) => number {
    return (sum, value) => sum * fn(value)
}

export function min(a: number, b: number): number {
    return Math.min(a, b)
}

export function minBy<T>(
    fn: (value: T) => number,
): (a: number, b: T) => number {
    return (a, b) => Math.min(a, fn(b))
}

export function max(a: number, b: number): number {
    return Math.max(a, b)
}

export function maxBy<T>(
    fn: (value: T) => number,
): (a: number, b: T) => number {
    return (a, b) => Math.max(a, fn(b))
}

export function groupBy<K extends keyof any, V>(
    fn: (value: V) => K,
): (groups: Record<K, V[]>, value: V) => Record<K, V[]> {
    return (groups, value) => {
        const key = fn(value)
        if (key in groups) {
            groups[key].push(value)
        } else {
            groups[key] = [value]
        }
        return groups
    }
}

export function count<T extends keyof any>(
    counts: Record<T, number>,
    value: T,
): Record<T, number> {
    if (value in counts) {
        counts[value]++
    } else {
        counts[value] = 1
    }
    return counts
}

export function countBy<K extends keyof any, V>(
    fn: (value: V) => K,
): (counts: Record<K, number>, value: V) => Record<K, number> {
    return (counts, value) => {
        const key = fn(value)
        if (key in counts) {
            counts[key]++
        } else {
            counts[key] = 1
        }
        return counts
    }
}

export function sortNumbers(numbers: number[]): number[] {
    return numbers.sort((a, b) => a - b)
}

export function zip<T, U>(a: T[], b: U[]): [T, U][] {
    return a.map((value, index) => [value, b[index]])
}

export function unzip<T, U>(pairs: [T, U][]): [T[], U[]] {
    return pairs.reduce(
        (acc, [x, y]) => {
            acc[0].push(x)
            acc[1].push(y)
            return acc
        },
        [[], []] as [T[], U[]],
    )
}

export function transpose<T>(matrix: T[][]): T[][] {
    return matrix[0].map((_, i) => matrix.map(row => row[i]))
}

export function greatesCommonDivisor(a: number, b: number): number {
    return b === 0 ? a : greatesCommonDivisor(b, a % b)
}

export const gcd = greatesCommonDivisor

export function leastCommonMultiple(a: number, b: number): number {
    return (a * b) / greatesCommonDivisor(a, b)
}

export const lcm = leastCommonMultiple
