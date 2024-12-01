import * as path from 'path'
import {
    ansi,
    AssertionError,
    NotImplementedError,
    UnreachableError,
    type DayModule,
    type Input,
    type Solution,
    type SolutionFun,
} from './types'

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

export function as_lines(inputs: string[]): string[][] {
    return inputs.map(input =>
        input
            .trim()
            .split('\n')
            .map(line => line.trim()),
    )
}

export function as_text(inputs: string[]): string[] {
    return inputs.map(input => input.trim())
}

export type TestStatus = 'passed' | 'failed' | 'skipped'

export function isStatusOk(status: TestStatus): boolean {
    return status !== 'failed'
}

export async function test_run<T extends Solution>(
    title: string,
    expected: T,
    fn: SolutionFun<T>,
): Promise<TestStatus> {
    try {
        if (Number.isNaN(expected) || expected === '') {
            notImplemented()
        }

        const start = Bun.nanoseconds()
        const result = await Promise.resolve(fn())
        const duration = Bun.nanoseconds() - start
        const durationMs = duration / 1_000_000

        const equal = Bun.deepEquals(result, expected)
        if (equal) {
            console.log(`✅ ${title}: ${result} [${durationMs}ms]`)
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
    return await test_all(
        [title(1), part1.solution, part1(input)],
        [title(2), part2.solution, part2(input)],
    )
}

type TestFactory = () => Promise<boolean>
export async function tests(...tests: TestFactory[]) {
    for (const test of tests) {
        if (!(await test())) {
            break
        }
    }
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
