import { test_day } from './utils'

async function* generateDays() {
    for (let i = 1; i <= 24; i++) {
        yield `${i}`.padStart(2, '0')
    }
}

for await (const day of generateDays()) {
    try {
        await test_day(await import(`./day${day}`))
    } catch (e) {
        if ((e as Error).name !== 'ResolveMessage') {
            throw e
        }
    }
}
