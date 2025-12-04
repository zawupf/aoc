import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

export const part1: Part = input => () => {
    const grid = input.map(line => line!.split(''))
    return findPossibleRolls(grid).size
}

export const part2: Part = input => () => {
    const grid = input.map(line => line!.split(''))

    let count = 0
    let possibleRolls = findPossibleRolls(grid)
    while (possibleRolls.size > 0) {
        count += possibleRolls.size

        const nextCandidates: Set<string> = new Set()
        for (const pos of possibleRolls) {
            const [x, y] = toCoords(pos)
            grid[y]![x] = '.'
            for (const [nx, ny] of adjacentPositions(x, y, grid)) {
                nextCandidates.add(`${nx},${ny}`)
            }
        }

        possibleRolls = findPossibleRolls(grid, nextCandidates)
    }

    return count
}

function toCoords(pos: string): [number, number] {
    const [xStr, yStr] = pos.split(',') as [string, string]
    return [Number(xStr), Number(yStr)]
}

function findPossibleRolls(
    grid: string[][],
    process?: Set<string>,
): Set<string> {
    const width = grid[0]!.length
    const height = grid.length
    const rolls: Set<string> = new Set()
    const canRemove = (x: number, y: number) => {
        if (grid[y]![x] !== '@') return false
        let adjacentRollsCount = 0
        for (const [nx, ny] of adjacentPositions(x, y, grid)) {
            if (grid[ny]![nx] !== '@') continue
            if (++adjacentRollsCount > 3) return false
        }
        return true
    }
    const addIfRemovable = (x: number, y: number) => {
        if (canRemove(x, y)) rolls.add(`${x},${y}`)
    }

    if (process) {
        for (const pos of process) addIfRemovable(...toCoords(pos))
    } else {
        for (let y = 0; y < height; y++)
            for (let x = 0; x < width; x++) addIfRemovable(x, y)
    }
    return rolls
}

function* adjacentPositions(
    x: number,
    y: number,
    grid: string[][],
): IterableIterator<[number, number]> {
    const width = grid[0]!.length
    const height = grid.length
    const deltas = [-1, 0, 1]
    for (const dx of deltas) {
        for (const dy of deltas) {
            if (dx === 0 && dy === 0) continue
            const nx = x + dx
            const ny = y + dy
            if (ny >= 0 && ny < height && nx >= 0 && nx < width) {
                yield [nx, ny]
            }
        }
    }
}

export const day = import.meta.file.match(/day(\d+)/)![1]!
export const input = await utils.readInputLines(day)
part1.solution = 1523
part2.solution = 9290

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
        ..@@.@@@@.
        @@@.@.@.@@
        @@@@@.@.@@
        @.@@@@..@.
        @@.@@@@.@@
        .@@@@@@@.@
        .@.@.@.@@@
        @.@@@.@@@@
        .@@@@@@@@.
        @.@.@@@.@.
        `,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 13, part1(testInput[0]!)],
                ['Test part 2', 43, part2(testInput[0]!)],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
