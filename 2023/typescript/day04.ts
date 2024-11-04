import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Card = { id: number; wins: number[]; deck: number[] }
function parseCard(input: string): Card {
    const [id, cards] = input.split(': ').map(s => s.trim())
    const [wins, deck] = cards.split(' | ').map(s => s.trim())
    return {
        id: utils.parseInt(id.substring(5)),
        wins: wins.replaceAll(/\s+/g, ' ').split(' ').map(utils.parseInt),
        deck: deck.replaceAll(/\s+/g, ' ').split(' ').map(utils.parseInt),
    }
}

function findWinsInDeck({ wins, deck }: Card): number[] {
    return wins.filter(win => deck.includes(win))
}

export const part1: Part = input => async () =>
    input
        .map(parseCard)
        .map(findWinsInDeck)
        .reduce(
            utils.sumBy(wins => (wins.length ? 1 << --wins.length : 0)),
            0,
        )

export const part2: Part = input => async () =>
    input
        .map(parseCard)
        .map(card => ({ card, count: 1 }))
        .map(({ card, count }, index, cards) => {
            const winsCount = findWinsInDeck(card).length
            for (let i = index + 1, n = index + 1 + winsCount; i < n; i++) {
                cards[i].count += count
            }
            return count
        })
        .reduce(utils.sum, 0)

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 21138
part2.solution = 7185540

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 13, part1(testInput[0])],
                ['Test part 2', 30, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
