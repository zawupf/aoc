import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Rule = [number, number]
type Update = number[]

function parse(input: string): [Rule[], Update[]] {
    const [ruleLines, updateLines] = utils
        .split_sections(input)
        .map(utils.split_lines)
    return [
        ruleLines.map(line => utils.split_numbers(line, '|') as Rule),
        updateLines.map(line => utils.split_numbers(line, ',') as Update),
    ]
}

function isUpdateValid(update: Update, rules: Rule[]): boolean {
    return rules.every(rule => {
        const [a, b] = rule
        const [i, j] = [update.indexOf(a), update.indexOf(b)]
        return i === -1 || j === -1 || i < j
    })
}

function fixUpdate(update: Update, rules: Rule[]): Update {
    rules = rules.filter(([a, b]) => update.includes(a) && update.includes(b))
    const predecessorCounts = rules.reduce(
        utils.countBy(([_, b]) => b),
        {},
    )
    return update
        .map(page => [page, predecessorCounts[page] ?? 0])
        .sort((a, b) => a[1] - b[1])
        .map(([page, _]) => page)
}

function middlePage(update: Update): number {
    return update[update.length >> 1]
}

export const part1: Part = input => async () => {
    const [rules, updates] = parse(input)
    return updates
        .filter(update => isUpdateValid(update, rules))
        .reduce(utils.sumBy(middlePage), 0)
}

export const part2: Part = input => async () => {
    const [rules, updates] = parse(input)
    return updates
        .filter(update => !isUpdateValid(update, rules))
        .map(update => fixUpdate(update, rules))
        .reduce(utils.sumBy(middlePage), 0)
}

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputText(day)
part1.solution = 6612
part2.solution = 4944

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_text([
        `
47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 143, part1(testInput[0])],
                ['Test part 2', 123, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
