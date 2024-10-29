import { test_run } from './utils'

const days = Array.from({ length: 24 }, (_, i) =>
    (i + 1).toString().padStart(2, '0'),
)

for await (const day of days) {
    try {
        const { part1, part2 } = await import(`./day${day}`)
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
