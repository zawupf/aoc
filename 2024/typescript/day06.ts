import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Field = '.' | '#' | 'X'
type Row = Field[]
type Grid = Row[]

type Pos = [number, number]
type Dir = 'up' | 'down' | 'left' | 'right'

type Guard = [Pos, Dir]
type State = [Grid, Guard]
type FinalKind = 'exited' | 'caught'
type FinalState = [State, FinalKind]

function parse(lines: string[]): State {
    const grid = lines.map(line => line.split('') as Row)
    const findGuard = (grid: Grid): Guard => {
        for (let y = 0; y < grid.length; y++) {
            for (let x = 0; x < grid[y].length; x++) {
                const cell = grid[y][x] as string
                if (cell === '^') {
                    grid[y][x] = 'X'
                    return [[x, y], 'up']
                }
            }
        }
        throw new Error('Guard not found')
    }
    const guard = findGuard(grid)
    return [grid, guard]
}

function walk(state: State): FinalState {
    let [grid, [pos, dir]] = state
    let [x, y] = pos
    const guardStates = new Set<string>()
    const guardKey = ([[x, y], dir]: Guard): string => `${x},${y},${dir}`
    guardStates.add(guardKey([[x, y], dir]))

    const next = (dir: Dir): Pos => {
        switch (dir) {
            case 'up':
                return [x, y - 1]
            case 'down':
                return [x, y + 1]
            case 'left':
                return [x - 1, y]
            case 'right':
                return [x + 1, y]
        }
    }

    const turnRight = (dir: Dir): Dir => {
        switch (dir) {
            case 'up':
                return 'right'
            case 'right':
                return 'down'
            case 'down':
                return 'left'
            case 'left':
                return 'up'
        }
    }

    while (true) {
        const nextPos = next(dir)
        const [nx, ny] = nextPos
        if (nx < 0 || ny < 0 || nx >= grid[0].length || ny >= grid.length) {
            return [state, 'exited']
        }

        const key = guardKey([[nx, ny], dir])
        if (guardStates.has(key)) {
            return [state, 'caught']
        }

        guardStates.add(key)
        const cell = grid[ny][nx]
        if (cell === '#') {
            dir = turnRight(dir)
            continue
        }

        ;[x, y] = nextPos
        grid[y][x] = 'X'
    }
}

function visitedCount(grid: Grid): number {
    return grid.flat().reduce(
        utils.sumBy(cell => (cell === 'X' ? 1 : 0)),
        0,
    )
}

export const part1: Part = input => async () =>
    visitedCount(walk(parse(input))[0][0])

export const part2: Part = input => async () => {
    const [originalGrid, startPos] = parse(input)
    let result = 0
    for (let y = 0; y < originalGrid.length; y++) {
        for (let x = 0; x < originalGrid[0].length; x++) {
            if (originalGrid[y][x] === '.') {
                const grid = originalGrid.map(row => row.slice())
                grid[y][x] = '#'
                const state: State = [grid, startPos]
                const [_, kind] = walk(state)
                if (kind === 'caught') {
                    result += 1
                }
            }
        }
    }
    return result
}

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 5177
part2.solution = 1686

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 41, part1(testInput[0])],
                ['Test part 2', 6, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
