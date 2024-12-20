import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Pos = [number, number]
type Dist2EndMap = Map<string, number>
type Jump = { from: Pos; to: Pos }

type Track = {
    start: Pos
    end: Pos
    dist2End: Dist2EndMap
    jumps: () => Generator<Jump>
}

function posKey([x, y]: Pos): string {
    return `${x},${y}`
}

function parse(lines: string[], maxDuration: number): Track {
    function neighbors([x, y]: Pos): Pos[] {
        return [
            [x - 1, y],
            [x + 1, y],
            [x, y - 1],
            [x, y + 1],
        ]
    }

    function* cheats([x, y]: Pos): Generator<Pos> {
        for (let duration = 2; duration <= maxDuration; duration++) {
            for (let d = 0; d <= duration; d++) {
                const [dx, dy] = [d, duration - d]
                if (dx === 0) {
                    yield [x, y + dy] as Pos
                    yield [x, y - dy] as Pos
                } else if (dy === 0) {
                    yield [x + dx, y] as Pos
                    yield [x - dx, y] as Pos
                } else {
                    yield [x + dx, y + dy] as Pos
                    yield [x - dx, y + dy] as Pos
                    yield [x + dx, y - dy] as Pos
                    yield [x - dx, y - dy] as Pos
                }
            }
        }
    }

    function isWall([x, y]: Pos): boolean {
        return (grid[y]?.[x] ?? '#') === '#'
    }

    const grid = lines.map(line => line.split(''))
    const fields = grid.flatMap((row, y) =>
        row.map((c, x) => [c, [x, y] as Pos] as const),
    )
    const start = fields.find(([c]) => c === 'S')![1]
    const end = fields.find(([c]) => c === 'E')![1]

    const dist2End = new Map<string, number>()
    const track: Pos[] = []

    let steps = 0
    let current: Pos | undefined = end
    while (current) {
        track.push(current)
        dist2End.set(posKey(current), steps++)
        current = neighbors(current).find(
            pos => !isWall(pos) && !dist2End.has(posKey(pos)),
        )
    }

    const jumps = function* (): Generator<Jump> {
        for (const current of track) {
            yield* cheats(current)
                .filter(pos => !isWall(pos))
                .map(to => ({ from: current, to } as Jump))
        }
    }

    return { start, end, dist2End, jumps }
}

function distance({ from, to }: Jump): number {
    return Math.abs(from[0] - to[0]) + Math.abs(from[1] - to[1])
}

function countJumpsWithSaving(
    pred: (saving: number) => boolean,
    track: Track,
): number {
    function saving(jump: Jump): number {
        return (
            track.dist2End.get(posKey(jump.from))! -
            track.dist2End.get(posKey(jump.to))! -
            distance(jump)
        )
    }

    return track
        .jumps()
        .map(saving)
        .filter(pred)
        .reduce((count, _b) => count + 1, 0)
}

export const part1: Part = input => () =>
    countJumpsWithSaving(x => x >= 100, parse(input, 2))

export const part2: Part = input => () =>
    countJumpsWithSaving(x => x >= 100, parse(input, 20))

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 1415
part2.solution = 1022577

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    // const testInput = utils.as_lines([
    //     `
    // ###############
    // #...#...#.....#
    // #.#.#.#.#.###.#
    // #S#...#.#.#...#
    // #######.#.#.###
    // #######.#.#...#
    // #######.#.###.#
    // ###..E#...#...#
    // ###.#######.###
    // #...###...#...#
    // #.#####.#.###.#
    // #.#...#.#.#...#
    // #.#.#.#.#.#.###
    // #...#...#...###
    // ###############
    // `,
    // ])

    // const track1 = parse(testInput[0], 2)
    // const test1 = (pred: (distSaving: number) => boolean) => () =>
    //     countJumpsWithSaving(pred, track1)
    // const track2 = parse(testInput[0], 20)
    // const test2 = (pred: (distSaving: number) => boolean) => () =>
    //     countJumpsWithSaving(pred, track2)

    await utils.tests(
        // () =>
        //     utils.test_all(
        //         ['Test 1', 14, test1(x => x === 2)],
        //         ['Test 1', 14, test1(x => x === 4)],
        //         ['Test 1', 2, test1(x => x === 6)],
        //         ['Test 1', 4, test1(x => x === 8)],
        //         ['Test 1', 2, test1(x => x === 10)],
        //         ['Test 1', 3, test1(x => x === 12)],
        //         ['Test 1', 1, test1(x => x === 20)],
        //         ['Test 1', 1, test1(x => x === 36)],
        //         ['Test 1', 1, test1(x => x === 38)],
        //         ['Test 1', 1, test1(x => x === 40)],
        //         ['Test 1', 1, test1(x => x === 64)],
        //     ),
        // () =>
        //     utils.test_all(
        //         ['Test 2', 32, test2(x => x === 50)],
        //         ['Test 2', 31, test2(x => x === 52)],
        //         ['Test 2', 29, test2(x => x === 54)],
        //         ['Test 2', 39, test2(x => x === 56)],
        //         ['Test 2', 25, test2(x => x === 58)],
        //         ['Test 2', 23, test2(x => x === 60)],
        //         ['Test 2', 20, test2(x => x === 62)],
        //         ['Test 2', 19, test2(x => x === 64)],
        //         ['Test 2', 12, test2(x => x === 66)],
        //         ['Test 2', 14, test2(x => x === 68)],
        //         ['Test 2', 12, test2(x => x === 70)],
        //         ['Test 2', 22, test2(x => x === 72)],
        //         ['Test 2', 4, test2(x => x === 74)],
        //         ['Test 2', 3, test2(x => x === 76)],
        //     ),
        // () =>
        //     utils.test_all(
        //         ['Test part 1', 0, part1(testInput[0])],
        //         ['Test part 2', 0, part2(testInput[0])],
        //     ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
