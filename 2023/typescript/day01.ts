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

const input = await utils.readInputLines(day)

function part1(data = input) {
    return () => getCalibrationSum('\\d', data)
}

function part2(data = input) {
    return () =>
        getCalibrationSum(
            '\\d|one|two|three|four|five|six|seven|eight|nine',
            data,
        )
}

utils.test_run('Part 1', 54990, part1())
utils.test_run('Part 2', 54473, part2())
