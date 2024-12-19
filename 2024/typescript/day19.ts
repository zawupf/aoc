import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Towels = { patterns: string[]; designs: string[] }

function parse(lines: string[]): Towels {
    const patterns = lines[0].split(', ')
    const designs = lines.slice(2)
    return { patterns, designs }
}

function countPossibleDesigns({ designs, patterns }: Towels): number {
    function isPossibleDesign(design: string): boolean {
        if (design.length === 0) {
            return true
        }

        return patterns
            .filter(pattern => design.startsWith(pattern))
            .some(pattern => isPossibleDesign(design.slice(pattern.length)))
    }

    return designs.filter(isPossibleDesign).length
}

function countAllPatternCombinations({ designs, patterns }: Towels): number {
    const cache = new Map<string, number>()

    function countCombinations(design: string): number {
        if (design.length === 0) {
            return 1
        }

        let result = cache.get(design)
        if (result !== undefined) {
            return result
        }

        result = patterns.reduce(
            utils.sumBy(pattern =>
                design.startsWith(pattern)
                    ? countCombinations(design.slice(pattern.length))
                    : 0,
            ),
            0,
        )
        cache.set(design, result)
        return result
    }

    return designs.reduce(utils.sumBy(countCombinations), 0)
}

export const part1: Part = input => () => countPossibleDesigns(parse(input))

export const part2: Part = input => () =>
    countAllPatternCombinations(parse(input))

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 333
part2.solution = 678536865274732

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
r, wr, b, g, bwu, rb, gb, br

brwrr
bggr
gbbr
rrbgbr
ubwu
bwurrg
brgr
bbrgwb
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 6, part1(testInput[0])],
                ['Test part 2', 16, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
