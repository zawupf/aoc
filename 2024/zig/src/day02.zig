const std = @import("std");

const aoc = @import("aoc_utils");

const Order = enum {
    ascending,
    descending,
};

fn order(a: u32, b: u32) ?Order {
    if (a < b and b - a <= 3) return .ascending;
    if (a > b and a - b <= 3) return .descending;
    return null; // equal or out of range
}

fn split(numbers: []const u8) struct { u32, []const u8 } {
    var iter = aoc.tokenize(numbers, ' ');
    const head = std.fmt.parseInt(u32, iter.next().?, 10) catch unreachable;
    const tail = iter.rest();
    return .{ head, tail };
}

fn isSafe(head: u32, tail: []const u8, canSkip: bool, refOrder: ?Order) bool {
    if (tail.len == 0) return true; // single number left

    const next, const rest = split(tail);

    if (refOrder) |o| {
        const safe = o == order(head, next);
        return safe and isSafe(next, rest, canSkip, o) or canSkip and isSafe(head, rest, false, o);
    }

    const o1 = order(head, next);
    return o1 != null and isSafe(next, rest, canSkip, o1) or
        (canSkip and isSafe(next, rest, false, null)) or
        (canSkip and isSafe(head, rest, false, null));
}

fn part1(input: []const u8, gpa: std.mem.Allocator) !u32 {
    _ = gpa;
    var result: u32 = 0;
    var lines = aoc.splitLines(input);
    while (lines.next()) |line| {
        const head, const tail = split(line);
        result += if (isSafe(head, tail, false, null)) 1 else 0;
    }
    return result;
}

fn part2(input: []const u8, gpa: std.mem.Allocator) !u32 {
    _ = gpa;
    var result: u32 = 0;
    var lines = aoc.splitLines(input);
    while (lines.next()) |line| {
        const head, const tail = split(line);
        result += if (isSafe(head, tail, true, null)) 1 else 0;
    }
    return result;
}

const Day = aoc.DayInfo("02", u32, u32, 490, 536, &.{.{
    .expected1 = 2,
    .expected2 = 4,
    .input =
    \\7 6 4 2 1
    \\1 2 7 8 9
    \\9 7 6 2 1
    \\1 3 2 4 5
    \\8 6 4 4 1
    \\1 3 6 7 9
    ,
}});

test "samples 1" {
    try Day.testPart1Samples(part1);
}
test "samples 2" {
    try Day.testPart2Samples(part2);
}
test "part 1" {
    try Day.testPart1(part1);
}
test "part 2" {
    try Day.testPart2(part2);
}
