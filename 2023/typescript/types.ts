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
