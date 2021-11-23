export function orderWeight(strng: string): string {
  return strng
    .trim()
    .split(/\s+/)
    .sort((a, b) => cheat(a) - cheat(b) || a.localeCompare(b))
    .join(' ')
}

const cheat = (weight: string): number => {
  return weight.split('').reduce((sum, char) => sum + parseInt(char, 10), 0)
}
