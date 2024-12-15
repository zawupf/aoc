import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Move = '^' | 'v' | '<' | '>'
type Field = '.' | '#' | 'O' | '@' | '[' | ']'
type Pos = [number, number]
type Grid = Field[][]
type State = {
    grid: Grid
    moves: Move[]
    robot: Pos
}

function parse(input: string): State {
    const [gridChunk, movesChunk] = utils.split_sections(input)
    const grid = utils
        .split_lines(gridChunk)
        .map(line => line.split('') as Field[])
    const moves = movesChunk.replaceAll('\n', '').split('') as Move[]
    const robot = grid
        .flatMap((row, y) => row.map((cell, x) => [cell, x, y]))
        .find(([cell]) => cell === '@')!
        .slice(1) as Pos
    return { grid, moves, robot }
}

function scaleUp({ grid, moves, robot: [x, y] }: State): State {
    grid = grid.map(row =>
        row
            .map(cell => {
                switch (cell) {
                    case '@':
                        return '@.'
                    case 'O':
                        return '[]'
                    case '.':
                        return '..'
                    case '#':
                        return '##'
                    default:
                        utils.unreachable(`Invalid field: ${cell}`)
                }
            })
            .join('')
            .split(''),
    ) as Grid
    return { grid, moves, robot: [x * 2, y] }
}

function nextPos([x, y]: Pos, move: Move): Pos {
    switch (move) {
        case '^':
            return [x, y - 1]
        case 'v':
            return [x, y + 1]
        case '<':
            return [x - 1, y]
        case '>':
            return [x + 1, y]
        default:
            utils.unreachable(`Invalid move: ${move}`)
    }
}

function canMove(grid: Grid, [x, y]: Pos, move: Move): boolean {
    const [nx, ny] = nextPos([x, y], move)
    switch (grid[ny][nx]) {
        case '.':
            return true
        case 'O':
            return canMove(grid, [nx, ny], move)
        case '[':
            switch (move) {
                case '<':
                    return canMove(grid, [nx, ny], move)
                case '>':
                    return canMove(grid, [nx + 1, ny], move)
                case '^':
                case 'v':
                    return (
                        canMove(grid, [nx, ny], move) &&
                        canMove(grid, [nx + 1, ny], move)
                    )
                default:
                    utils.unreachable(`Invalid move: ${move}`)
            }
        case ']':
            switch (move) {
                case '<':
                    return canMove(grid, [nx - 1, ny], move)
                case '>':
                    return canMove(grid, [nx, ny], move)
                case '^':
                case 'v':
                    return (
                        canMove(grid, [nx, ny], move) &&
                        canMove(grid, [nx - 1, ny], move)
                    )
                default:
                    utils.unreachable(`Invalid move: ${move}`)
            }
        case '#':
            return false
        default:
            utils.unreachable(`Invalid field: ${grid[ny][nx]}`)
    }
}

function doMove(grid: Grid, [x, y]: Pos, move: Move): Pos {
    const [nx, ny] = nextPos([x, y], move)

    switch (grid[ny][nx]) {
        case '.':
            break
        case 'O':
            doMove(grid, [nx, ny], move)
            break
        case '[':
            if (move === '>') {
                doMove(grid, [nx + 1, ny], move)
                doMove(grid, [nx, ny], move)
            } else {
                doMove(grid, [nx, ny], move)
                doMove(grid, [nx + 1, ny], move)
            }
            break
        case ']':
            if (move === '<') {
                doMove(grid, [nx - 1, ny], move)
                doMove(grid, [nx, ny], move)
            } else {
                doMove(grid, [nx, ny], move)
                doMove(grid, [nx - 1, ny], move)
            }
            break
        case '#':
            utils.unreachable('Invalid move: hit a wall')
        default:
            utils.unreachable(`Invalid field: ${grid[ny][nx]}`)
    }

    grid[ny][nx] = grid[y][x]
    grid[y][x] = '.'
    return [nx, ny]
}

function tryMove(grid: Grid, [x, y]: Pos, move: Move): [boolean, Pos] {
    const ok = canMove(grid, [x, y], move)
    return [ok, ok ? doMove(grid, [x, y], move) : [x, y]]
}

function moveRobot(state: State) {
    const { grid, moves, robot } = state
    const [_, pos] = moves.reduce(
        ([_, pos], move) => tryMove(grid, pos, move),
        [true, robot],
    )
    return { grid, moves: [], robot: pos }
}

function gpsSum(state: State): number {
    return state.grid
        .flatMap((row, y) =>
            row.map((cell, x) => [cell, x, y] as [Field, number, number]),
        )
        .filter(([cell]) => cell === 'O' || cell === '[')
        .reduce((acc, [_, x, y]) => acc + 100 * y + x, 0)
}

export const part1: Part = input => () => gpsSum(moveRobot(parse(input)))

export const part2: Part = input => () =>
    gpsSum(moveRobot(scaleUp(parse(input))))

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputText(day)
part1.solution = 1563092
part2.solution = 1582688

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_text([
        `
########
#..O.O.#
##@.O..#
#...O..#
#.#.O..#
#...O..#
#......#
########

<^^>>>vv<v>>v<<
`,
        `
##########
#..O..O.O#
#......O.#
#.OO..O.O#
#..O@..O.#
#O#..O...#
#O..O..O.#
#.OO.O.OO#
#....O...#
##########

<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 2028, part1(testInput[0])],
                ['Test part 1', 10092, part1(testInput[1])],
                ['Test part 2', 9021, part2(testInput[1])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
