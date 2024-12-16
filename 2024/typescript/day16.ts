import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Pos = [number, number]
type Path = [Pos] | [Pos, Path]
type Orientation = 'north' | 'east' | 'south' | 'west'
type State = [Pos, Orientation, Path]
type Score = number
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
                north: Infinity,
                east: Infinity,
                south: Infinity,
                west: Infinity,
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

type Result = { score: Score; paths: Path[] }
type CancelPathPred = (previousScore: Score, currentScore: Score) => boolean
type JoinPathFn = (a: Pos, b: Path) => Path
function walk(
    maze: Maze,
    cancelCurrentPath: CancelPathPred,
    joinPath: JoinPathFn,
): Result {
    const startPos = [1, maze.length - 2] as Pos
    const [startX, startY] = startPos
    maze[startY][startX].scores.east = 0
    const [endX, endY] = [maze[0].length - 2, 1] as Pos

    function isEnd([[x, y], _orientation, _path]: State): boolean {
        return x === endX && y === endY
    }

    function tryMove([[x, y], orientation, path]: State): State | null {
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

        const score = current.scores[orientation] + 1
        if (cancelCurrentPath(next.scores[orientation], score)) {
            return null
        }

        next.scores[orientation] = score
        return [[x2, y2], orientation, joinPath([x2, y2], path)]
    }

    function tryTurn(
        turn: (o: Orientation) => Orientation,
        [pos, orientation, path]: State,
    ): State | null {
        const [x, y] = pos
        const current = maze[y][x]
        const nextOrientation = turn(orientation)
        const score = current.scores[orientation] + 1000
        if (cancelCurrentPath(current.scores[nextOrientation], score)) {
            return null
        }

        current.scores[nextOrientation] = score
        return [pos, nextOrientation, path]
    }

    const stack: State[] = [[startPos, 'east', [startPos]]]
    const result: Result = { score: Infinity, paths: [] }
    let state: State | undefined
    while ((state = stack.pop())) {
        let newState = tryMove(state)
        if (newState) {
            const [_, newOrientation, newPath] = newState
            if (isEnd(newState)) {
                const score = maze[endY][endX].scores[newOrientation]
                if (score < result.score) {
                    result.score = score
                    result.paths = [newPath]
                } else if (score === result.score) {
                    result.paths.push(newPath)
                }
            } else {
                stack.push(newState)
            }
        }

        if ((newState = tryTurn(left, state))) {
            stack.push(newState)
        }

        if ((newState = tryTurn(right, state))) {
            stack.push(newState)
        }
    }

    return result
}

function lowestScore(maze: Maze): number {
    return walk(
        maze,
        (previousScore, currentScore) => previousScore <= currentScore,
        (_, path) => path,
    ).score
}

function flat(path: Path): Pos[] {
    return path.length === 1 ? path : [path[0], ...flat(path[1])]
}

function posCount(maze: Maze): number {
    return new Set(
        walk(
            maze,
            (previousScore, currentScore) => previousScore < currentScore,
            (pos, path) => [pos, path],
        ).paths.flatMap(path => flat(path).map(([x, y]) => `${x},${y}`)),
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
