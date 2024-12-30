import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type KeyMap = Map<string, string[]>

function initKeyMap(): KeyMap {
    const map = createKeyMap([
        // Code Keypad Layout:
        // 7 8 9
        // 4 5 6
        // 1 2 3
        //   0 A
        'A3:^ A6:^^ A9:^^^ A0:< A2:<^:^< A5:<^^:^^< A8:<^^^:^^^< A1:^<< A4:^^<< A7:^^^<<',
        '02:^ 03:^>:>^ 01:^< 05:^^ 06:^^>:>^^ 04:^^< 08:^^^ 09:^^^>:>^^^ 07:^^^<',
        '36:^ 39:^^ 32:< 35:^<:<^ 38:^^<:<^^ 31:<< 34:^<<:<<^ 37:^^<<:<<^^',
        '21:< 26:^>:>^ 25:^ 24:^<:<^ 28:^^ 29:^^>:>^^ 27:^^<:<^^',
        '14:^ 15:^>:>^ 16:^>>:>>^ 17:^^ 18:^^>:>^^ 19:^^>>:>>^^',
        '65:< 64:<< 69:^ 68:<^:^< 67:^<<:<<^',
        '54:< 59:^>:>^ 58:^ 57:^<:<^',
        '47:^ 48:^>:>^ 49:^>>:>>^',
        '98:< 97:<<',
        '87:<',

        // Direction Keypad Layout:
        //   ^ A
        // < v >
        'A^:< A>:v Av:v<:<v A<:v<<',
        '^v:v ^>:v>:>v ^<:v<',
        '>v:< ><:<<',
        'v<:<',
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

    function createKeyMap(pathInfo: string[]): KeyMap {
        return new Map(
            pathInfo.flatMap(row => {
                return row.split(' ').flatMap(info => {
                    const [path, ...dirs] = info.split(':')
                    const [a, b] = path.split('')

                    return [
                        [a + b, dirs.map(d => d + 'A')],
                        [b + a, dirs.map(d => reverse(d) + 'A')],
                    ]
                })
            }),
        )
    }

    const numKeys = 'A0123456789'.split('')
    for (let i = 0; i < numKeys.length; i++) {
        const ki = numKeys[i]
        map.set(ki + ki, ['A'])
        for (let j = i + 1; j < numKeys.length; j++) {
            const kj = numKeys[j]
            console.assert(map.has(ki + kj), `Missing key: ${ki + kj}`)
            console.assert(map.has(kj + ki), `Missing key: ${kj + ki}`)
        }
    }

    const dirKeys = 'A<>^v'.split('')
    for (let i = 0; i < dirKeys.length; i++) {
        const ki = dirKeys[i]
        map.set(ki + ki, ['A'])
        for (let j = i + 1; j < dirKeys.length; j++) {
            const kj = dirKeys[j]
            console.assert(map.has(ki + kj), `Missing key: ${ki + kj}`)
            console.assert(map.has(kj + ki), `Missing key: ${kj + ki}`)
        }
    }

    return map
}

function countKeys(
    map: KeyMap,
    previous: string,
    keys: string,
    n: number,
    cache: Map<string, number> = new Map(),
): number {
    if (n === 0) {
        return keys.length
    }

    let result = 0
    for (let i = 0; i < keys.length; i++) {
        const current = keys[i]
        const pair = previous + current
        previous = current

        const cacheKey = n + pair
        if (cache.has(cacheKey)) {
            result += cache.get(cacheKey)!
            continue
        }

        const length = map
            .get(pair)!
            .map(keys => countKeys(map, 'A', keys, n - 1, cache))
            .reduce(utils.min)
        cache.set(cacheKey, length)
        result += length
    }

    return result
}

export const part1: Part = input => () => {
    const map = initKeyMap()
    return input
        .map(code => countKeys(map, 'A', code, 3) * parseInt(code, 10))
        .reduce(utils.sum)
}

export const part2: Part = input => () => {
    const map = initKeyMap()
    return input
        .map(code => countKeys(map, 'A', code, 26) * parseInt(code, 10))
        .reduce(utils.sum)
}

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputLines(day)
part1.solution = 215374
part2.solution = 260586897262600

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
        () => utils.test_all(['Test part 1', 126384, part1(testInput[0])]),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
