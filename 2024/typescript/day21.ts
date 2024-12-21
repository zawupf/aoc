import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type PadDirMap = Map<string, [number, string[]]>
type Maps = { codeMap: PadDirMap; dirMap: PadDirMap }

function initMaps(): Maps {
    // Code Keypad Layout:
    // 7 8 9
    // 4 5 6
    // 1 2 3
    //   0 A
    const codeMap = createDistanceMap([
        'A0:< A3:^ 02:^ 36:^ 32:< 25:^ 21:< 14:^ 69:^ 65:< 58:^ 54:< 47:^ 98:< 87:<',
        'A6:^^ A2:^<:<^ 03:^>:>^ 05:^^ 01:^< 39:^^ 35:^<:<^ 31:<< 26:^>:>^ 28:^^ 24:^<:<^ 15:^>:>^ 17:^^ 64:<< 68:<^:^< 59:^>:>^ 57:^<:<^ 48:^>:>^ 97:<<',
        'A9:^^^ A5:^^<:<^^:^<^ A1:^<<:<^< 06:^^>:>^^:^>^ 08:^^^ 04:^^<:^<^ 38:^^<:<^^^<^ 34:^<<:<<^:<^< 29:^^>:>^^:^>^ 27:^^<:<^^:^<^ 16:^>>:>>^:>^> 18:^^>:>^^:^>^ 67:^<<:<<^:<^< 49:^>>:>>^:>^>',
        'A8:^^^<:<^^^:^<^^:^^<^ A4:^^<<:^<<^:^<^< 09:^^^>:>^^^:^^>^:^>^^ 07:^^^<:^^<^:^<^^ 37:^^<<:<<^^:^<^<:^<<^:<^^<:<^<^ 19:^^>>:>>^^:^>>^:^>^>:>^^>:>^>^',
        'A7:^^^<<:^^<<^:^^<^<:^<<^^:^<^<^:^<^^<:<^^^<:<^^<^:<^<^^',
    ])

    // Direction Keypad Layout:
    //   ^ A
    // < v >
    const dirMap = createDistanceMap([
        'A^:< A>:v ^v:v >v:< v<:<',
        'Av:<v:v< ^>:v>:>v ^<:v< ><:<<',
        'A<:v<<:<v<',
    ])

    function reverse(dir: string): string {
        return dir
            .replaceAll(/./g, c => {
                switch (c) {
                    case '^':
                        return 'v'
                    case 'v':
                        return '^'
                    case '<':
                        return '>'
                    case '>':
                        return '<'
                    default:
                        throw new Error(`Invalid direction: ${c}`)
                }
            })
            .split('')
            .reverse()
            .join('')
    }

    function createDistanceMap(
        pathInfo: string[],
    ): Map<string, [number, string[]]> {
        return new Map(
            pathInfo.flatMap((row, i) => {
                return row.split(' ').flatMap(info => {
                    const [path, ...dirs] = info.split(':')
                    const [a, b] = path.split('')
                    return [
                        [a + b, [i + 2, dirs.map(dir => dir + 'A')]],
                        [b + a, [i + 2, dirs.map(dir => reverse(dir) + 'A')]],
                    ] as [string, [number, string[]]][]
                })
            }),
        )
    }

    return { codeMap, dirMap }
}

function pressKeys(
    map: PadDirMap,
    previous: string,
    keys: string,
    cache: Map<string, string[]>,
): string[] {
    const cache_key = previous + ':' + keys
    let result = cache.get(cache_key)
    if (result) {
        return cache.get(cache_key)!
    }

    const [current, rest] = [keys[0], keys.slice(1)]
    if (previous !== current && !map.has(previous + current)) {
        throw new Error(`Invalid key: ${previous + current}`)
    }

    const [_distance, dirs] =
        previous === current ? [1, ['A']] : map.get(previous + current)!
    console.assert(
        dirs.length !== 0,
        `dirs is empty with ${previous + current}`,
    )
    if (rest.length === 0) {
        result = dirs
    } else {
        const restDirs = pressKeys(map, current, rest, cache)
        result = dirs.flatMap(dir => restDirs.map(d => dir + d))
    }

    cache.set(cache_key, result)
    return result
}

function shortest(dirs: string[]): string[] {
    const originalLength = dirs.length
    const minDirLength = dirs.map(dir => dir.length).reduce(utils.min)
    const result = dirs.filter(dir => dir.length === minDirLength)
    const resultLength = result.length
    return result
}

function minSequenceLength(code: string, maps: Maps, n: number): number {
    const cache = new Map<string, string[]>()
    let dirs = pressKeys(maps.codeMap, 'A', code, cache)

    while (n--) {
        dirs = shortest(
            dirs.flatMap(dir => pressKeys(maps.dirMap, 'A', dir, cache)),
        )
    }

    return dirs[0].length * parseInt(code, 10)
}

export const part1: Part = input => () => {
    const maps = initMaps()
    return input.map(code => minSequenceLength(code, maps, 2)).reduce(utils.sum)
}

export const part2: Part = input => () => {
    const maps = initMaps()
    return input
        .map(code => minSequenceLength(code, maps, 25))
        .reduce(utils.sum)
}

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 215374
part2.solution = NaN

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
029A
980A
179A
456A
379A
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 126384, part1(testInput[0])],
                // ['Test part 2', NaN, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
