import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Tile = [x: number, y: number]
type TilePair = [a: Tile, b: Tile]

function area([[a0, a1], [b0, b1]]: TilePair): number {
    const width = Math.abs(a0 - b0) + 1
    const height = Math.abs(a1 - b1) + 1
    return width * height
}

function boundingBox(tiles: Tile[], offset: number = 0): [Tile, Tile] {
    const [minX, maxX, minY, maxY] = tiles.reduce(
        ([minX, maxX, minY, maxY], [x, y]) => [
            Math.min(minX, x),
            Math.max(maxX, x),
            Math.min(minY, y),
            Math.max(maxY, y),
        ],
        [Infinity, -Infinity, Infinity, -Infinity],
    )
    return [
        [minX - offset, minY - offset],
        [maxX + offset, maxY + offset],
    ]
}

function dump(tiles: Tile[]): void {
    const [[minX, minY], [maxX, maxY]] = boundingBox(tiles)
    for (let y = minY; y <= maxY; y++) {
        let line = ''
        for (let x = minX; x <= maxX; x++) {
            line += tiles.some(([tx, ty]) => tx === x && ty === y) ? '#' : '.'
        }
        console.log(line)
    }
    console.log('---')
}

export const part1: Part = input => () => {
    const corners = input.map(line => line.split(',').map(Number) as Tile)

    const boxes: TilePair[] = []
    for (let i = 0; i < corners.length; i++) {
        for (let j = i + 1; j < corners.length; j++) {
            boxes.push([corners[i]!, corners[j]!])
        }
    }

    return boxes.map(area).reduce((a, b) => Math.max(a, b), -Infinity)
}

export const part2: Part = input => () => {
    const _corners = input.map(line => line.split(',').map(Number) as Tile)
    const { corners, area } = shrinkCorners(_corners)
    // dump(corners)

    // const edges_ = edges(corners)
    // dump(Array.from(edges_).map(s => s.split(',').map(Number) as Tile))

    const outer = outerTiles(corners)
    // dump(Array.from(outer).map(s => s.split(',').map(Number) as Tile))

    const isOuter = (tile: Tile): boolean => outer.has(tile.join(','))

    const boxes: [TilePair, number][] = []
    for (let i = 0; i < corners.length; i++) {
        for (let j = i + 1; j < corners.length; j++) {
            const box: TilePair = [corners[i]!, corners[j]!]
            boxes.push([box, area(box)])
        }
    }
    boxes.sort(([, a], [, b]) => b - a)

    for (const [box, boxArea] of boxes) {
        const [[x_min, y_min], [x_max, y_max]] = boundingBox(box)

        let skip = false

        // test bbox corners first
        for (const corner of [
            [x_min, y_min],
            [x_min, y_max],
            [x_max, y_min],
            [x_max, y_max],
        ]) {
            if ((skip = isOuter(corner as Tile))) break
        }
        if (skip) continue

        // test bbox outline next
        for (let y = y_min + 1; y < y_max; y++) {
            if ((skip = isOuter([x_min, y]))) break
        }
        if (skip) continue

        for (let y = y_min + 1; y < y_max; y++) {
            if ((skip = isOuter([x_max, y]))) break
        }
        if (skip) continue

        for (let x = x_min + 1; x < x_max; x++) {
            if ((skip = isOuter([x, y_min]))) break
        }
        if (skip) continue

        for (let x = x_min + 1; x < x_max; x++) {
            if ((skip = isOuter([x, y_max]))) break
        }
        if (skip) continue

        for (let x = x_min + 1; x < x_max; x++) {
            for (let y = y_min + 1; y < y_max; y++) {
                if ((skip = isOuter([x, y]))) break
            }
            if (skip) break
        }

        if (!skip) return boxArea
    }

    utils.unreachable('No fitting box found')
}

function shrinkCorners(corners: Tile[]) {
    const xs = Array.from(new Set(corners.map(([x, _]) => x))).sort(
        (a, b) => a - b,
    )
    const ys = Array.from(new Set(corners.map(([_, y]) => y))).sort(
        (a, b) => a - b,
    )

    const expand = ([x_, y_]: Tile): Tile => [
        xs[x_ as number]!,
        ys[y_ as number]!,
    ]

    const area_ = (p: TilePair): number => area(p.map(expand) as TilePair)

    const corners_ = corners.map(
        ([x, y]) => [xs.indexOf(x), ys.indexOf(y)] as Tile,
    )

    return { corners: corners_, area: area_ } as const
}

function edges(corners: Tile[]): Set<string> {
    const tiles = new Set<string>()
    for (let i = 0; i < corners.length; i++) {
        let [x, y] = corners[i]!
        const [x_end, y_end] = corners[(i + 1) % corners.length]!
        const dx = Math.sign(x_end - x)
        const dy = Math.sign(y_end - y)
        utils.assert(dx === 0 || dy === 0, 'Only orthogonal edges supported')
        while (x !== x_end || y !== y_end) {
            tiles.add([x, y].join(','))
            x += dx
            y += dy
        }
    }
    return tiles
}

function outerTiles(corners: Tile[]): Set<string> {
    const [[x_min, y_min], [x_max, y_max]] = boundingBox(corners, 1)
    const edges_ = edges(corners)

    const tiles = new Set<string>()
    const stack: Tile[] = [[x_min, y_min]]
    let current: Tile | undefined
    while ((current = stack.pop())) {
        const key = current.join(',')
        if (tiles.has(key) || edges_.has(key)) {
            continue
        }

        tiles.add(key)
        const [x, y] = current
        const adjacents = (
            [
                [x + 1, y],
                [x - 1, y],
                [x, y + 1],
                [x, y - 1],
            ] as Tile[]
        ).filter(
            ([x, y]) => x >= x_min && x <= x_max && y >= y_min && y <= y_max,
        )
        stack.push(...adjacents)
    }
    return tiles
}

export const day = import.meta.file.match(/day(\d+)/)![1]!
export const input = await utils.readInputLines(day)
part1.solution = 4763509452
part2.solution = 1516897893

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
