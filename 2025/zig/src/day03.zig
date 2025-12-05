const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    _ = gpa;
    return solve(2, input);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    _ = gpa;
    return solve(12, input);
}

fn solve(comptime len: u8, input: []const u8) !u64 {
    var sum: u64 = 0;
    var banks = aoc.splitLines(aoc.trim(input));
    while (banks.next()) |bank| {
        var joltage: u64 = 0;
        var batteries = bank[0..];
        var n = len;
        while (n > 0) {
            n -= 1;
            const batteries_ = batteries[0 .. batteries.len - n];
            const i = std.mem.findMax(u8, batteries_);
            const v = batteries[i] - '0';
            joltage = joltage * 10 + v;
            batteries = batteries[i + 1 ..];
        }
        sum += joltage;
    }
    return sum;
}

pub const Day = aoc.DayInfo("03", u64, u64, 17613, 175304218462560, @This(), &.{.{
    .expected1 = 357,
    .expected2 = 3121910778619,
    .input =
    \\987654321111111
    \\811111111111119
    \\234234234234278
    \\818181911112111
    ,
}});

test "samples 1" {
    try Day.testPart1Samples();
}
test "samples 2" {
    try Day.testPart2Samples();
}
test "part 1" {
    try Day.testPart1();
}
test "part 2" {
    try Day.testPart2();
}
