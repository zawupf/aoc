import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Pos = [number, number]
type Field = number
type Grid = Field[][]

type State = {
    grid: Grid
    bytes: Pos[]
}

function parseBytes(lines: string[]): Pos[] {
    return lines.map(line => utils.split_numbers(line, ',', 2) as Pos)
}

function init(bytes: Pos[], dropCount: number): State {
    const gridSize = Math.max(...bytes.map(pos => Math.max(...pos))) + 1
    const grid: Grid = Array.from({ length: gridSize }, () =>
        Array.from({ length: gridSize }, () => Infinity),
    )

    bytes.slice(0, dropCount).forEach(([x, y]) => {
        grid[y][x] = 0
    })

    return { grid, bytes }
}

function minimumSteps({ grid }: State): number {
    const size = grid.length
    grid[0][0] = 0

    const isEnd = ([x, y]: Pos) => x === size - 1 && y === size - 1

    const field = ([x, y]: Pos) => grid[y][x]

    function* nextSteps([x, y]: Pos): Generator<Pos> {
        if (x > 0) yield [x - 1, y]
        if (x < size - 1) yield [x + 1, y]
        if (y > 0) yield [x, y - 1]
        if (y < size - 1) yield [x, y + 1]
    }

    const isValid = ([x, y]: Pos, steps: number) => steps < grid[y][x]

    const stack: Pos[] = [[0, 0]]
    let current: Pos | undefined
    while ((current = stack.pop())) {
        if (isEnd(current)) {
            continue
        }

        const steps = field(current) + 1
        const next = [...nextSteps(current).filter(pos => isValid(pos, steps))]
        next.forEach(([x, y]) => {
            grid[y][x] = steps
        })
        stack.push(...next)
    }
    return grid[size - 1][size - 1]
}

function firstBlockingByte(state: State): string {
    const { bytes } = state
    let okDrops = 0
    let failDrops = bytes.length - 1
    while (failDrops - okDrops > 1) {
        const drops = Math.floor((okDrops + failDrops) / 2)
        const state = init(bytes, drops)
        const steps = minimumSteps(state)
        if (steps === Infinity) {
            failDrops = drops
        } else {
            okDrops = drops
        }
    }
    let [x, y] = bytes[okDrops]
    return `${x},${y}`
}

export const part1: Part = input => () =>
    minimumSteps(init(parseBytes(input), 1024)).toString()

export const part2: Part = input => () =>
    firstBlockingByte(init(parseBytes(input), 0))

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = '292'
part2.solution = '58,44'

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
5,4
4,2
4,5
3,0
2,1
6,3
2,4
1,5
0,6
3,3
2,6
5,1
1,2
5,5
2,5
6,5
1,4
0,4
6,4
1,1
6,1
1,0
0,5
1,6
2,0
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                [
                    'Test part 1',
                    '22',
                    () =>
                        minimumSteps(
                            init(parseBytes(testInput[0]), 12),
                        ).toString(),
                ],
                [
                    'Test part 2',
                    '6,1',
                    () => firstBlockingByte(init(parseBytes(testInput[0]), 0)),
                ],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = string
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
