import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Pos = [number, number]
type Path = [Pos] | [Pos, Path]
enum Orientation {
    North = 0,
    East,
    South,
    West,
}
type State = [Pos, Orientation, Path]
type Score = number
type Field = {
    isWall: boolean
    scores: [number, number, number, number]
}
type Maze = Field[][]

function parse(lines: string[]): Maze {
    return lines.map(line =>
        line.split('').map(c => ({
            isWall: c === '#',
            scores: [Infinity, Infinity, Infinity, Infinity],
        })),
    )
}

function nextPos([x, y]: Pos, o: Orientation): Pos {
    return [
        x + (o === Orientation.East ? 1 : o === Orientation.West ? -1 : 0),
        y + (o === Orientation.South ? 1 : o === Orientation.North ? -1 : 0),
    ]
}

function left(o: Orientation): Orientation {
    return o === Orientation.North
        ? Orientation.West
        : o === Orientation.West
        ? Orientation.South
        : o === Orientation.South
        ? Orientation.East
        : Orientation.North
}

function right(o: Orientation): Orientation {
    return o === Orientation.North
        ? Orientation.East
        : o === Orientation.East
        ? Orientation.South
        : o === Orientation.South
        ? Orientation.West
        : Orientation.North
}

type Result = { score: Score; paths: Path[] }
type CancelPathPred = (previousScore: Score, currentScore: Score) => boolean
type JoinPathFn = (a: Pos, b: Path) => Path
function walk(
    maze: Maze,
    cancelCurrentPath: CancelPathPred,
    joinPath: JoinPathFn,
): Result {
    const result: Result = { score: Infinity, paths: [] }

    const start: Pos = [1, maze.length - 2]
    field(start).scores[Orientation.East] = 0
    const stack: State[] = [[start, Orientation.East, [start]]]
    const [end_x, end_y]: Pos = [maze[0].length - 2, 1]

    let state: State | undefined
    while ((state = stack.pop())) {
        if (isEnd(state)) {
            const [pos, orientation, path] = state
            const score = field(pos).scores[orientation]
            if (score < result.score) {
                result.score = score
                result.paths = [path]
            } else if (score === result.score) {
                result.paths.push(path)
            }

            continue
        }

        const current = state
        if ((state = tryTurnAndMove(left, current))) {
            stack.push(state)
        }

        if ((state = tryTurnAndMove(right, current))) {
            stack.push(state)
        }

        if ((state = tryMove(current))) {
            stack.push(state)
        }
    }

    return result

    function isEnd([[x, y], _orientation, _path]: State): boolean {
        return x === end_x && y === end_y
    }

    function field([x, y]: Pos): Field {
        return maze[y][x]
    }

    function tryMove([pos, orientation, path]: State): State | undefined {
        const current = field(pos)
        const p = nextPos(pos, orientation)

        const next = field(p)
        if (next.isWall) {
            return undefined
        }

        const score = current.scores[orientation] + 1
        if (cancelCurrentPath(next.scores[orientation], score)) {
            return undefined
        }

        next.scores[orientation] = score
        return [p, orientation, joinPath(p, path)]
    }

    function tryTurnAndMove(
        turn: (o: Orientation) => Orientation,
        [pos, orientation, path]: State,
    ): State | undefined {
        const nextOrientation = turn(orientation)
        if (field(nextPos(pos, nextOrientation)).isWall) {
            return undefined
        }

        const current = field(pos)
        const score = current.scores[orientation] + 1000
        if (cancelCurrentPath(current.scores[nextOrientation], score)) {
            return undefined
        }

        current.scores[nextOrientation] = score
        return tryMove([pos, nextOrientation, path])
    }
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
