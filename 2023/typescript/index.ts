import type { DayModule } from './types'
import { test_run } from './utils'

async function* generateDays() {
    for (let i = 1; i <= 24; i++) {
        yield `${i}`.padStart(2, '0')
    }
}

for await (const day of generateDays()) {
    try {
        const { part1, part2 }: DayModule = await import(`./day${day}`)
        test_run(`Day ${day} (part 1)`, part1.solution, part1())
        test_run(`Day ${day} (part 2)`, part2.solution, part2())
    } catch (e) {
        const { name, message } = e as Error
        switch (name) {
            case 'ResolveMessage':
                // ignore import resolution errors
                break
            default:
                console.log(`Skipping day ${day}: [${name}] ${message}`)
                break
        }
    }
}
