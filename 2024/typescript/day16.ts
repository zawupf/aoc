import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Pos = [number, number]
type Path = Pos[]
type Orientation = 'north' | 'east' | 'south' | 'west'
type State = [Pos, Orientation, Path]
type Score = { score: number }
type Field = {
    isWall: boolean
    scores: Record<Orientation, Score>
}
type Maze = Field[][]

function parse(lines: string[]): Maze {
    return lines.map(line =>
        line.split('').map(c => ({
            isWall: c === '#',
            scores: {
                north: { score: Infinity, paths: [] },
                east: { score: Infinity, paths: [] },
                south: { score: Infinity, paths: [] },
                west: { score: Infinity, paths: [] },
            },
        })),
    )
}

function left(orientation: Orientation): Orientation {
    return orientation === 'north'
        ? 'west'
        : orientation === 'west'
        ? 'south'
        : orientation === 'south'
        ? 'east'
        : 'north'
}

function right(orientation: Orientation): Orientation {
    return orientation === 'north'
        ? 'east'
        : orientation === 'east'
        ? 'south'
        : orientation === 'south'
        ? 'west'
        : 'north'
}

function walk(maze: Maze) {
    const startPos = [1, maze.length - 2] as Pos
    const endPos = [maze[0].length - 2, 1] as Pos
    const start = [startPos, 'east', []] as State
    maze[startPos[1]][startPos[0]].scores.east = { score: 0 }

    function isEnd([pos]: State): boolean {
        return pos[0] === endPos[0] && pos[1] === endPos[1]
    }

    function tryMove([pos, orientation]: State): State | null {
        const [x, y] = pos
        const current = maze[y][x]
        const [x2, y2] = [
            x + (orientation === 'east' ? 1 : orientation === 'west' ? -1 : 0),
            y +
                (orientation === 'south'
                    ? 1
                    : orientation === 'north'
                    ? -1
                    : 0),
        ] as Pos

        const next = maze[y2][x2]
        if (next.isWall) {
            return null
        }

        const score = current.scores[orientation].score + 1
        if (next.scores[orientation].score <= score) {
            return null
        }

        next.scores[orientation].score = score
        return [[x2, y2], orientation, []]
    }

    function tryTurn(
        turn: (o: Orientation) => Orientation,
        [pos, orientation]: State,
    ): State | null {
        const [x, y] = pos
        const current = maze[y][x]
        const nextOrientation = turn(orientation)
        const score = current.scores[orientation].score + 1000
        if (current.scores[nextOrientation].score <= score) {
            return null
        }

        current.scores[nextOrientation].score = score
        return [pos, nextOrientation, []]
    }

    const stack = [start]
    while (stack.length > 0) {
        let state = stack.pop()!
        let newState = tryMove(state)
        if (newState && !isEnd(newState)) stack.push(newState)
        newState = tryTurn(left, state)
        if (newState) stack.push(newState)
        newState = tryTurn(right, state)
        if (newState) stack.push(newState)
    }
}

type Result = { score: number; paths: Path[] }
function walk2(maze: Maze): Result {
    const startPos = [1, maze.length - 2] as Pos
    const endPos = [maze[0].length - 2, 1] as Pos
    const start = [startPos, 'east', [startPos]] as State
    maze[startPos[1]][startPos[0]].scores.east = { score: 0 }

    function isEnd([pos]: State): boolean {
        return pos[0] === endPos[0] && pos[1] === endPos[1]
    }

    function tryMove([pos, orientation, path]: State): State | null {
        const [x, y] = pos
        const current = maze[y][x]
        const [x2, y2] = [
            x + (orientation === 'east' ? 1 : orientation === 'west' ? -1 : 0),
            y +
                (orientation === 'south'
                    ? 1
                    : orientation === 'north'
                    ? -1
                    : 0),
        ] as Pos

        const next = maze[y2][x2]
        if (next.isWall) {
            return null
        }

        const score = current.scores[orientation].score + 1
        if (next.scores[orientation].score < score) {
            return null
        }

        next.scores[orientation].score = score
        return [[x2, y2], orientation, [...path, [x2, y2]]]
    }

    function tryTurn(
        turn: (o: Orientation) => Orientation,
        [pos, orientation, path]: State,
    ): State | null {
        const [x, y] = pos
        const current = maze[y][x]
        const nextOrientation = turn(orientation)
        const score = current.scores[orientation].score + 1000
        if (current.scores[nextOrientation].score < score) {
            return null
        }

        current.scores[nextOrientation].score = score
        return [pos, nextOrientation, path]
    }

    const stack = [start]
    const result: Result = { score: Infinity, paths: [] }
    while (stack.length > 0) {
        let state = stack.pop()!
        let newState = tryMove(state)
        if (newState) {
            if (isEnd(newState)) {
                const score =
                    maze[endPos[1]][endPos[0]].scores[newState[1]].score
                if (score < result.score) {
                    result.score = score
                    result.paths = [newState[2]]
                } else if (score === result.score) {
                    result.paths.push(newState[2])
                }
            } else {
                stack.push(newState)
            }
        }
        newState = tryTurn(left, state)
        if (newState) stack.push(newState)
        newState = tryTurn(right, state)
        if (newState) stack.push(newState)
    }

    return result
}

function lowestScore(maze: Maze): number {
    const [x, y] = [maze[0].length - 2, 1] as Pos
    walk(maze)
    return Math.min(...Object.values(maze[y][x].scores).map(s => s.score))
}

function posCount(maze: Maze): number {
    const result = walk2(maze)
    return new Set(
        result.paths.flatMap(path => path.map(([x, y]) => `${x},${y}`)),
    ).size
}

export const part1: Part = input => () => lowestScore(parse(input))

export const part2: Part = input => () => posCount(parse(input))

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 85432
part2.solution = 465

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
###############
#.......#....E#
#.#.###.#.###.#
#.....#.#...#.#
#.###.#####.#.#
#.#.#.......#.#
#.#.#####.###.#
#...........#.#
###.#.#####.#.#
#...#.....#.#.#
#.#.#.###.#.#.#
#.....#...#.#.#
#.###.#.#.#.#.#
#S..#.....#...#
###############
`,
        `
#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 7036, part1(testInput[0])],
                ['Test part 1', 11048, part1(testInput[1])],
                ['Test part 2', 45, part2(testInput[0])],
                ['Test part 2', 64, part2(testInput[1])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
