import { type DayModule, type SolutionFactory } from './types'
import * as utils from './utils'

function countPaths(
    from: string,
    to: string,
    devices: Map<string, string[]>,
    except: string = '',
): number {
    const stack = [[from, new Set<string>([from])]] as [string, Set<string>][]
    let current: [string, Set<string>] | undefined
    let count = 0
    while ((current = stack.pop())) {
        const [device, visited] = current
        if (device === to) {
            count++
            continue
        }
        if (device === except) continue
        const next: [string, Set<string>][] =
            devices
                .get(device)
                ?.filter(d => {
                    const seen = visited.has(d)
                    utils.assert(!seen, `Loop detected at device ${d}`)
                    return !visited.has(d)
                })
                .map(d => [d, new Set(visited).add(d)]) || []
        stack.push(...next!)
    }
    return count
}

export const part1: Part = input => () => {
    const devices = new Map<string, string[]>()

    for (const line of input) {
        const [device, rest] = line.split(':').map(s => s.trim()) as [
            string,
            string,
        ]
        const outputs = rest.split(' ').map(s => s.trim())
        devices.set(device, outputs)
    }

    return countPaths('you', 'out', devices)
}

export const part2: Part = input => () => {
    const devices = new Map<string, string[]>()

    for (const line of input) {
        const [device, rest] = line.split(':').map(s => s.trim()) as [
            string,
            string,
        ]
        const outputs = rest.split(' ').map(s => s.trim())
        devices.set(device, outputs)
    }

    // const n_srv_fft = countPaths('svr', 'fft', devices, 'dac')
    // console.log('n_srv_fft', n_srv_fft)
    // const n_fft_dac = countPaths('fft', 'dac', devices)
    // console.log('n_fft_dac', n_fft_dac)
    // const n_dac_out = countPaths('dac', 'out', devices, 'fft')
    // console.log('n_dac_out', n_dac_out)

    // const n_srv_dac = countPaths('svr', 'dac', devices, 'fft')
    // console.log('n_srv_dac', n_srv_dac)
    // const n_dac_fft = countPaths('dac', 'fft', devices)
    // console.log('n_dac_fft', n_dac_fft)
    // const n_fft_out = countPaths('fft', 'out', devices, 'dac')
    // console.log('n_fft_out', n_fft_out)

    // console.log({
    //     n_srv_fft,
    //     n_fft_dac,
    //     n_dac_out,
    //     n_srv_dac,
    //     n_dac_fft,
    //     n_fft_out,
    // })

    // return n_srv_fft * n_fft_dac * n_dac_out + n_srv_dac * n_dac_fft * n_fft_out

    const stack = [['svr', 0]] as [string, number][]
    let current: [string, number] | undefined
    let count = 0
    const giveUp = new Set<string>()
    while ((current = stack.pop())) {
        const [device, n] = current
        // console.write(`\r${stack.length} ${device} ${n}   `)
        if (device === 'out') {
            if (n == 3) count++
            continue
        }
        const n_ = n | (device === 'dac' ? 1 : device === 'fft' ? 2 : 0)
        stack.push(
            ...devices.get(device)!.map(d => [d, n_] as [string, number]),
        )
    }
    // console.log()
    return count
}

export const day = import.meta.file.match(/day(\d+)/)![1]!
export const input = await utils.readInputLines(day)
part1.solution = 699
part2.solution = NaN

export const main = import.meta.main
if (main) {
    const module: Module = await import(import.meta.path)

    const testInput = utils.as_lines([
        `
        aaa: you hhh
        you: bbb ccc
        bbb: ddd eee
        ccc: ddd eee fff
        ddd: ggg
        eee: out
        fff: out
        ggg: out
        hhh: ccc fff iii
        iii: out
        `,
        `
        svr: aaa bbb
        aaa: fft
        fft: ccc
        bbb: tty
        tty: ccc
        ccc: ddd eee
        ddd: hub
        hub: fff
        eee: dac
        dac: fff
        fff: ggg hhh
        ggg: out
        hhh: out
        `,
    ])

    await utils.tests(
        () =>
            utils.test_all(
                ['Test part 1', 5, part1(testInput[0]!)],
                ['Test part 2', 2, part2(testInput[1]!)],
            ),
        () => utils.test_day(module),
    )
}

type In = typeof input
type Out = number
type Module = DayModule<Out, In>
type Part = SolutionFactory<Out, In>
