import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Tile = [x: number, y: number]
type TilePair = [a: Tile, b: Tile]

function area([[a0, a1], [b0, b1]]: TilePair): number {
    const width = Math.abs(a0 - b0) + 1
    const height = Math.abs(a1 - b1) + 1
    return width * height
}

function dump(tiles: Tile[]): void {
    const minX = Math.min(...tiles.map(([x]) => x)) - 1
    const maxX = Math.max(...tiles.map(([x]) => x)) + 1
    const minY = Math.min(...tiles.map(([, y]) => y)) - 1
    const maxY = Math.max(...tiles.map(([, y]) => y)) + 1

    for (let y = minY; y <= maxY; y++) {
        let line = ''
        for (let x = minX; x <= maxX; x++) {
            line += tiles.some(([tx, ty]) => tx === x && ty === y) ? '#' : '.'
        }
        console.log(line)
    }
}

export const part1: Part = input => () => {
    const tiles = input.map(line => line.split(',').map(Number) as Tile)

    const tilePairs: TilePair[] = []
    for (let i = 0; i < tiles.length; i++) {
        for (let j = i + 1; j < tiles.length; j++) {
            tilePairs.push([tiles[i]!, tiles[j]!])
        }
    }

    return tilePairs.map(area).reduce((a, b) => Math.max(a, b), -Infinity)
}

export const part2: Part = input => () => utils.notImplemented()

export const day = import.meta.file.match(/day(\d+)/)![1]!
export const input = await utils.readInputLines(day)
part1.solution = 4763509452
part2.solution = NaN

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
        7,1
        11,1
        11,7
        9,7
        9,5
        2,5
        2,3
        7,3
        `,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 50, part1(testInput[0]!)],
                ['Test part 2', 24, part2(testInput[0]!)],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
