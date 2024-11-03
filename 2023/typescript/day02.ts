import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

const CubeKeys = ['red', 'green', 'blue'] as const
type Cube = (typeof CubeKeys)[number]
type Bag = Record<Cube, number>

function isValidBagWithRef(bag: Bag, reference: Bag): boolean {
    return CubeKeys.every(key => (bag[key] ?? 0) <= (reference[key] ?? 0))
}

function powerOfBag(bag: Bag): number {
    return CubeKeys.reduce((acc, key) => acc * (bag[key] ?? 0), 1)
}

function parseBag(line: string): Bag {
    const bag: Bag = { red: 0, green: 0, blue: 0 }
    const parts = line.split(', ')
    parts.forEach(part => {
        const [count, cube] = part.split(' ')
        bag[cube as Cube] = parseInt(count)
    })
    return bag
}

type Id = number
type Game = {
    id: Id
    bags: Bag[]
}

function isValidGameWithRef(game: Game, reference: Bag): boolean {
    return game.bags.every(bag => isValidBagWithRef(bag, reference))
}

function findSmallestValidBag(game: Game) {
    const result = game.bags.reduce((acc, bag) => {
        CubeKeys.forEach(key => {
            acc[key] = Math.max(acc[key] ?? 0, bag[key] ?? 0)
        })
        return acc
    }, {} as Bag)

    return result
}

function parseGame(line: string): Game {
    const [id, bags] = line.split(': ')
    const game: Game = {
        id: parseInt(id.substring(5)),
        bags: bags.split('; ').map(parseBag),
    }
    return game
}

export const part1: Part = function (input) {
    const bag = { red: 12, green: 13, blue: 14 }
    return async function () {
        return input
            .map(parseGame)
            .filter(game => isValidGameWithRef(game, bag))
            .reduce((acc, game) => acc + game.id, 0)
    }
}

export const part2: Part = function (input) {
    return async function () {
        return input
            .map(parseGame)
            .map(findSmallestValidBag)
            .reduce((acc, val) => acc + powerOfBag(val), 0)
    }
}

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 2204
part2.solution = 71036

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 8, part1(testInput[0])],
                ['Test part 2', 2286, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
