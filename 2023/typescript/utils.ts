import * as path from 'path'

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

export function test_run(
    name: string,
    expected: number | string,
    fn: () => number | string,
) {
    const start = Bun.nanoseconds()
    const result = fn()
    const duration = Bun.nanoseconds() - start
    const durationMs = duration / 1_000_000

    if (result === expected) {
        console.log(`✅ ${name}: ${result} [${durationMs}ms]`)
    } else {
        console.log(`❌ ${name}: Expected ${expected}, got ${result}`)
    }
}
