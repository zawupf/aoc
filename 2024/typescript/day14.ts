import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Grid = {
    width: number
    height: number
}
type Robot = {
    pos: [number, number]
    vel: [number, number]
}

function parseRobot(line: string): Robot {
    const [px, py, vx, vy] = line.match(/(-?\d+)/g)!.map(utils.parseInt)
    return { pos: [px, py], vel: [vx, vy] }
}

function mod(n: number, length: number): number {
    const m = n % length
    return m >= 0 ? m : m + length
}

function moveRobot(robot: Robot, n: number, grid: Grid): Robot {
    const {
        pos: [x, y],
        vel: [vx, vy],
    } = robot
    return {
        ...robot,
        pos: [mod(x + n * vx, grid.width), mod(y + n * vy, grid.height)],
    }
}

function quadrantCount(robots: Robot[], grid: Grid): number[] {
    const middleWidth = Math.floor(grid.width / 2)
    const middleHeight = Math.floor(grid.height / 2)
    const quadrantCount = new Array(4).fill(0)
    for (const { pos } of robots) {
        const [x, y] = pos
        if (x < middleWidth && y < middleHeight) {
            quadrantCount[0]++
        } else if (x > middleWidth && y < middleHeight) {
            quadrantCount[1]++
        } else if (x < middleWidth && y > middleHeight) {
            quadrantCount[2]++
        } else if (x > middleWidth && y > middleHeight) {
            quadrantCount[3]++
        }
    }
    return quadrantCount
}

function safetyFactor(robots: Robot[], grid: Grid): number {
    return quadrantCount(robots, grid).reduce((acc, n) => acc * n, 1)
}

function findTree(_robots: Robot[], grid: Grid) {
    for (let n = 1; n < 10000; n++) {
        const robots = _robots.map(r => moveRobot(r, n, grid))
        const counts = quadrantCount(robots, grid).map(n => n / robots.length)
        if (counts.some(n => n > 0.5)) {
            // const map = new Array(grid.height)
            //     .fill(0)
            //     .map(() => new Array(grid.width).fill(' '))
            // for (const { pos } of robots) {
            //     const [x, y] = pos
            //     map[y][x] = '#'
            // }
            // console.log(`Time: ${n}`)
            // console.log(map.map(row => row.join('')).join('\n'))
            return n
        }
    }

    throw new Error('No solution found')
}

export const part1: Part = input => () => {
    const g = { width: 101, height: 103 }
    return safetyFactor(
        input.map(parseRobot).map(r => moveRobot(r, 100, g)),
        g,
    )
}

export const part2: Part = input => () =>
    findTree(input.map(parseRobot), { width: 101, height: 103 })

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 233709840
part2.solution = 6620

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3
`,
    ])

    await utils.tests(
        () =>
            utils.test_all([
                'Test part 1',
                12,
                () => {
                    const g = { width: 11, height: 7 }
                    return safetyFactor(
                        testInput[0]
                            .map(parseRobot)
                            .map(r => moveRobot(r, 100, g)),
                        g,
                    )
                },
            ]),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
