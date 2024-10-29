import * as utils from './utils'

const day = '01'

const numbersMap: Record<string, number> = {
    one: 1,
    two: 2,
    three: 3,
    four: 4,
    five: 5,
    six: 6,
    seven: 7,
    eight: 8,
    nine: 9,
}
function toNumber(str: string): number {
    return numbersMap[str] ?? parseInt(str)
}

function getCalibrationSum(digitPattern: string, input: string[]): number {
    const rxA = new RegExp(`.*?(${digitPattern})`)
    const rxB = new RegExp(`.*(${digitPattern})`)
    return input
        .map(line => {
            const a = toNumber(line.match(rxA)![1])
            const b = toNumber(line.match(rxB)![1])
            return +`${a}${b}`
        })
        .reduce((acc, val) => acc + val, 0)
}

export function part1(data = input) {
    return () => getCalibrationSum('\\d', data)
}

export function part2(data = input) {
    return () =>
        getCalibrationSum(
            '\\d|one|two|three|four|five|six|seven|eight|nine',
            data,
        )
}

const input = await utils.readInputLines(day)
part1.solution = 54990
part2.solution = 54473

if (import.meta.main) {
    utils.test_run('Part 1', part1.solution, part1())
    utils.test_run('Part 2', part2.solution, part2())
}
