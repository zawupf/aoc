type Input = string | string[]
export type Solution = string | number
export type SolutionFun<T extends Solution> = () => T | Promise<T>
export interface SolutionFactory<T extends Solution, In extends Input> {
    (input?: In): SolutionFun<T>
    solution: T
}

type DayModule_<T extends Solution, In extends Input> = {
    part1: SolutionFactory<T, In>
    part2: SolutionFactory<T, In>
}
export type DayModule = DayModule_<Solution, Input>

export class NotImplementedError extends Error {
    constructor(message?: string) {
        super(message || 'Not implemented')
        this.name = 'NotImplementedError'
    }
}

const gray = Bun.color('gray', 'ansi') ?? ''
const red = Bun.color('red', 'ansi') ?? ''
const green = Bun.color('green', 'ansi') ?? ''
const blue = Bun.color('blue', 'ansi') ?? ''
const clear = '\x1b[0m'
const marker = `${red}|${clear}`
const marked = (value: Solution) => `${marker}${value}${marker}`
export const ansi = { gray, red, green, blue, clear, marker, marked }
