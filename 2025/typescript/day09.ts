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
    const { corners, area, grid } = shrinkCorners(_corners)

    markOuterTiles(corners, grid)

    const isOuter = ([x, y]: Tile): boolean => grid[y]![x]! === 1

    const boxes_: [TilePair, number][] = []
    for (let i = 0; i < corners.length; i++) {
        for (let j = i + 1; j < corners.length; j++) {
            const box: TilePair = [corners[i]!, corners[j]!]
            boxes_.push([box, area(box)])
        }
    }
    const boxes = utils.PriorityQueue.from(boxes_, (a, b) => b[1] - a[1])

    let box_: [TilePair, number] | undefined
    while ((box_ = boxes.pop())) {
        const [box, boxArea] = box_
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

        if (!skip) {
            return boxArea
        }
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

    const grid_: number[][] = []
    for (let y = 0; y < ys.length; y++) {
        grid_[y] = new Array<number>(xs.length).fill(0)
    }
    for (const [x, y] of corners_) {
        grid_[y]![x]! = -1
    }

    return { corners: corners_, area: area_, grid: grid_ } as const
}

function markEdges(corners: Tile[], grid: number[][]): void {
    for (let i = 0; i < corners.length; i++) {
        let [x, y] = corners[i]!
        const [x_end, y_end] = corners[(i + 1) % corners.length]!
        const dx = Math.sign(x_end - x)
        const dy = Math.sign(y_end - y)
        utils.assert(dx === 0 || dy === 0, 'Only orthogonal edges supported')
        while (x !== x_end || y !== y_end) {
            grid[y]![x]! -= 1
            x += dx
            y += dy
        }
    }
}

function markOuterTiles(corners: Tile[], grid: number[][]): void {
    const [[x_min, y_min], [x_max, y_max]] = boundingBox(corners)
    utils.assert(
        x_min === 0 && y_min === 0,
        'Expected bounding box min to be 0,0 after shrinking',
    )
    markEdges(corners, grid)

    const width = x_max - x_min + 1
    const height = y_max - y_min + 1

    const stack = [
        ...Array.from({ length: width }, (_, x) => [x, y_min] as Tile),
        ...Array.from({ length: width }, (_, x) => [x, y_max] as Tile),
        ...Array.from({ length: height }, (_, y) => [x_min, y] as Tile),
        ...Array.from({ length: height }, (_, y) => [x_max, y] as Tile),
    ]
    let current: Tile | undefined
    while ((current = stack.pop())) {
        const [x, y] = current
        if (grid[y]![x]!) {
            continue
        }

        grid[y]![x]! = 1

        const adjacents = (
            [
                [x + 1, y],
                [x - 1, y],
                [x, y + 1],
                [x, y - 1],
            ] as Tile[]
        ).filter(
            ([x, y]) =>
                x >= 0 &&
                x < width &&
                y >= 0 &&
                y < height &&
                grid[y]![x]! === 0,
        )
        stack.push(...adjacents)
    }
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
