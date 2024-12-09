import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

type Entry = {
    id: number
    used: number
    free: number
}

function parse(input: string): Entry[] {
    const entries = []
    for (let i = 0, id = 0; i < input.length; i += 2) {
        entries.push({
            id: id++,
            used: parseInt(input[i]),
            free: parseInt(input[i + 1] ?? 0),
        })
    }
    return entries
}

function fragment(entries: Entry[]): Entry[] {
    const result = []
    while (entries.length > 0) {
        const left = entries.shift()!
        let free = left.free
        result.push({ ...left, free: 0 })
        while (free && entries.length > 0) {
            const right = entries.pop()!
            if (right.used <= free) {
                free -= right.used
                result.push({ ...right, free: 0 })
                continue
            }

            result.push({ ...right, used: free, free: 0 })
            entries.push({
                ...right,
                used: right.used - free,
                free: right.free + free,
            })
            free = 0
        }
    }
    return result
}

function defragment(entries: Entry[]): Entry[] {
    const findFreeSlot = (n: number, start: number, end: number) => {
        for (let i = start; i < end; i++) {
            const entry = entries[i]
            if (entry.free >= n) {
                return i
            }
        }
        return -1
    }

    let start = findFreeSlot(1, 0, entries.length)
    for (let i = entries.length - 1; i >= start && start !== -1; ) {
        const e = entries[i]
        const j = findFreeSlot(e.used, start, i)
        if (j === -1) {
            i--
            continue
        }

        entries.splice(i, 1)
        entries[i - 1].free += e.used + e.free
        e.free = entries[j].free - e.used
        entries[j].free = 0
        entries.splice(j + 1, 0, e)

        start = findFreeSlot(1, start, i)
    }

    return entries
}

function checksum(entries: Entry[]): number {
    let count = 0
    let index = 0
    for (let i = 0; i < entries.length; i++) {
        const entry = entries[i]
        for (let j = 0; j < entry.used; j++) {
            count += entry.id * index++
        }
        index += entry.free
    }
    return count
}

export const part1: Part = input => () => {
    const entries = fragment(parse(input))
    return checksum(entries)
}

export const part2: Part = input => () => {
    const entries = defragment(parse(input))
    return checksum(entries)
}

export const day = import.meta.file.match(/day(\d+)/)![1]
export const input = await utils.readInputText(day)
part1.solution = 6607511583593
part2.solution = 6636608781232

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_text([`2333133121414131402`])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 1928, part1(testInput[0])],
                ['Test part 2', 2858, part2(testInput[0])],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
