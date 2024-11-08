import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Range = { src: number; len: number }
type Mapping = { dst: number; src: number; len: number }
type Mappings = Mapping[]

function parseRange(range: string): Mapping {
    const [dst, src, len] = range.split(' ').map(utils.parseInt)
    return { dst, src, len }
}

function parse(input: string): [number[], Mappings[]] {
    const [seeds, ...maps] = input.trim().split('\n\n')
    return [
        seeds.split(': ')[1].split(' ').map(utils.parseInt),
        maps.map(map => map.split('\n').slice(1).map(parseRange)),
    ]
}

function begin(r: Range): number {
    return r.src
}

function end(r: Range): number {
    return r.src + r.len
}

function splitRange(
    range: Range,
    mapping: Mapping,
): { inside?: Range; outside: Range[] } {
    const r = range
    const m: Range = { src: mapping.src, len: mapping.len }

    // Case 1: Range is entirely inside mapping
    if (begin(r) >= begin(m) && end(r) <= end(m)) {
        return {
            inside: range,
            outside: [],
        }
    }

    // Case 2: Range is entirely outside mapping
    if (end(r) <= begin(m) || begin(r) >= end(m)) {
        return {
            inside: undefined,
            outside: [range],
        }
    }

    // The remaining cases are all partial overlaps
    // and produce both one inside- and one or two outside-ranges

    // Case 3: Range is partially inside mapping with overlap at end
    if (begin(r) >= begin(m)) {
        utils.assert(
            end(r) > end(m),
            'Range is partially inside mapping with overlap at end',
        )
        return {
            inside: { src: begin(r), len: end(m) - begin(r) },
            outside: [{ src: end(m), len: end(r) - end(m) }],
        }
    }

    // Case 4: Range is partially inside mapping with overlap at begin
    if (end(r) <= end(m)) {
        utils.assert(
            begin(r) < begin(m),
            'Range is partially inside mapping with overlap at begin',
        )
        return {
            inside: { src: begin(m), len: end(r) - begin(m) },
            outside: [{ src: begin(r), len: begin(m) - begin(r) }],
        }
    }

    // Case 5: Range fully exceeds mapping
    utils.assert(
        begin(r) < begin(m) && end(r) > end(m),
        'Range fully exceeds mapping',
    )
    return {
        inside: m,
        outside: [
            { src: begin(r), len: begin(m) - begin(r) },
            { src: end(m), len: end(r) - end(m) },
        ],
    }
}

function applyMappings(map: Mappings, range: Range): Range[] {
    const mapRangePartially = (
        [todos, done]: Range[][],
        mapping: Mapping,
    ): Range[][] => [
        todos.flatMap(todo => {
            const { inside, outside } = splitRange(todo, mapping)
            if (inside) {
                done.push({
                    ...inside,
                    src: mapping.dst + inside.src - mapping.src,
                })
            }
            return outside
        }),
        done,
    ]
    return map.reduce(mapRangePartially, [[range], []]).flat()
}

function findMinLocation(
    input: string,
    generateInitialRanges: (numbers: number[]) => Range[],
) {
    const [numbers, maps] = parse(input)
    const seeds = generateInitialRanges(numbers)
    return maps
        .reduce(
            (ranges, map) => ranges.flatMap(range => applyMappings(map, range)),
            seeds,
        )
        .reduce(
            utils.minBy(_ => _.src),
            Infinity,
        )
}

export const part1: Part = input => async () =>
    findMinLocation(input, _ => _.map(src => ({ src, len: 1 })))

export const part2: Part = input => async () =>
    findMinLocation(input, _ =>
        _.reduce(
            (seeds, len, i, s) =>
                i % 2 === 0 ? seeds : [...seeds, { src: s[i - 1], len }],
            [] as Range[],
        ),
    )

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputText(day)
part1.solution = 175622908
part2.solution = 5200543

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_text([
        `
seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4
`,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 35, part1(testInput[0])],
                ['Test part 2', 46, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
