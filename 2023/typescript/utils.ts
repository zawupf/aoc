import * as path from 'path'
import type { Solution, SolutionFun } from './types'

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

export function toLines(input: string): string[] {
    return input
        .trim()
        .split('\n')
        .map(line => line.trim())
}

export async function test_run<T extends Solution>(
    name: string,
    expected: T,
    fn: SolutionFun<T>,
) {
    const start = Bun.nanoseconds()
    const result = await Promise.resolve(fn())
    const duration = Bun.nanoseconds() - start
    const durationMs = duration / 1_000_000

    if (result === expected) {
        console.log(`✅ ${name}: ${result} [${durationMs}ms]`)
    } else {
        console.log(`❌ ${name}: Expected ${expected}, got ${result}`)
    }
}
