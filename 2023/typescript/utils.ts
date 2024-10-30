import * as path from 'path'
import { ansi, type Solution, type SolutionFun } from './types'

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
    title: string,
    expected: T,
    fn: SolutionFun<T>,
): Promise<string | null | undefined> {
    try {
        const start = Bun.nanoseconds()
        const result = await Promise.resolve(fn())
        const duration = Bun.nanoseconds() - start
        const durationMs = duration / 1_000_000

        if (result === expected) {
            console.log(`✅ ${title}: ${result} [${durationMs}ms]`)
        } else {
            const first = `❌ ${title}: Expected`
            const second = 'but got'.padStart(Bun.stringWidth(first))
            console.log(
                `${first} ${ansi.marked(expected)}\n` +
                    `${second} ${ansi.marked(result)}`,
            )
        }
    } catch (e) {
        const { name, message = 'Unknown error' } = e as Error
        switch (name) {
            case 'NotImplementedError':
                console.log(`${ansi.gray}🚧 ${title}: ${message}${ansi.clear}`)
                break
            default:
                console.log(`💥 ${title}: ${ansi.red}${message}${ansi.clear}`)
                break
        }

        return message
    }
}
