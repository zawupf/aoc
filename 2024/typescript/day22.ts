import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

function parse(lines: string[]): number[] {
    return lines.map(utils.parseInt)
}

function step1(secret: number): number {
    return (secret ^ (secret << 6)) & 0xffffff
}

function step2(secret: number): number {
    return (secret ^ (secret >>> 5)) & 0xffffff
}

function step3(secret: number): number {
    return (secret ^ (secret << 11)) & 0xffffff
}

function* generateSecrets(secret: number): Generator<number> {
    while (true) {
        yield (secret = step3(step2(step1(secret))))
    }
}

function getNthSecret(secret: number, n: number): number {
    for (const next of generateSecrets(secret)) {
        if (--n === 0) {
            return next
        }
    }
    return NaN
}

type Stats = Map<string, number>
function collectStats(secret: number, n: number): Stats {
    const stats: Stats = new Map()

    let previous = secret % 10
    const diffs: number[] = []
    for (const next of generateSecrets(secret)) {
        const current = next % 10
        const diff = current - previous
        diffs.push(diff)
        if (diffs.length > 4) {
            diffs.shift()
        }

        if (diffs.length === 4) {
            const key = diffs.join(',')
            if (!stats.has(key)) {
                stats.set(key, current)
            }
        }

        if (--n === 0) {
            return stats
        }
        previous = current
    }
    return stats
}

export const part1: Part = input => () =>
    parse(input)
        .map(secret => getNthSecret(secret, 2000))
        .reduce(utils.sum)

export const part2: Part = input => () => {
    const secrets = parse(input)
    const stats = secrets.map(secret => collectStats(secret, 2000))
    const diffs = stats.reduce(
        (diffs, stat) => diffs.union(new Set(stat.keys())),
        new Set<string>(),
    )
    return diffs
        .values()
        .map(diff => stats.map(stat => stat.get(diff) ?? 0).reduce(utils.sum))
        .reduce(utils.max)
}

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 18317943467
part2.solution = 2018

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
1
10
100
2024
`,
        `
1
2
3
2024
`,
    ])

    await utils.tests(
        // () =>
        //     utils.test_all(
        //         ['Test 1', 15887950, () => getNthSecret(123, 1)],
        //         ['Test 1', 16495136, () => getNthSecret(123, 2)],
        //         ['Test 1', 527345, () => getNthSecret(123, 3)],
        //         ['Test 1', 704524, () => getNthSecret(123, 4)],
        //         ['Test 1', 1553684, () => getNthSecret(123, 5)],
        //         ['Test 1', 12683156, () => getNthSecret(123, 6)],
        //         ['Test 1', 11100544, () => getNthSecret(123, 7)],
        //         ['Test 1', 12249484, () => getNthSecret(123, 8)],
        //         ['Test 1', 7753432, () => getNthSecret(123, 9)],
        //         ['Test 1', 5908254, () => getNthSecret(123, 10)],
        //         ['Test 1', 8685429, () => getNthSecret(1, 2000)],
        //     ),
        () =>
            utils.test_all(
                ['Test part 1', 37327623, part1(testInput[0])],
                ['Test part 2', 23, part2(testInput[1])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
