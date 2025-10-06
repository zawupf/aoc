const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Op = enum { add, mul, concat };

fn canSolve(expected: u64, values: []const u64, ops: []const Op, gpa: Allocator) !bool {
    var stack = std.ArrayList(struct { u64, []const u64 }).empty;
    defer stack.deinit(gpa);
    try stack.append(gpa, .{ values[0], values[1..] });

    while (stack.pop()) |entry| {
        const current, const rest = entry;
        if (current == expected and rest.len == 0) return true;
        if (current > expected or rest.len == 0) continue;
        const next = rest[0];
        for (ops) |op| {
            const current_ = switch (op) {
                .add => current + next,
                .mul => current * next,
                .concat => blk: {
                    std.debug.assert(next != 0);
                    var c = current;
                    var n = next;
                    while (n != 0) : (n /= 10) c *= 10;
                    break :blk c + next;
                },
            };
            try stack.append(gpa, .{ current_, rest[1..] });
        }
    }

    return false;
}

fn solve(ops: []const Op, input: []const u8, gpa: Allocator) !u64 {
    var result: u64 = 0;
    var buffer: [32]u64 = undefined;
    var lines = aoc.splitLines(input);
    while (lines.next()) |line| {
        var it = aoc.tokenizeAny(line, ": ");
        const numbers = try aoc.parseInts(u64, &it, &buffer);
        const expected = numbers[0];
        result += if (try canSolve(expected, numbers[1..], ops, gpa)) expected else 0;
    }
    return result;
}

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    return try solve(&.{ .add, .mul }, input, gpa);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    return try solve(&.{ .add, .mul, .concat }, input, gpa);
}

pub const Day = aoc.DayInfo("07", u64, u64, 1260333054159, 162042343638683, @This(), &.{.{
    .expected1 = 3749,
    .expected2 = 11387,
    .input =
    \\190: 10 19
    \\3267: 81 40 27
    \\83: 17 5
    \\156: 15 6
    \\7290: 6 8 6 15
    \\161011: 16 10 13
    \\192: 17 8 14
    \\21037: 9 7 18 13
    \\292: 11 6 16 20
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
