//! By convention, root.zig is the root source file when making a library.
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
    var iter = std.mem.tokenizeScalar(u8, numbers, ' ');
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

fn part1(input: []const u8) !u32 {
    var result: u32 = 0;
    var lines = std.mem.tokenizeScalar(u8, input, '\n');
    while (lines.next()) |line| {
        const head, const tail = split(line);
        result += if (isSafe(head, tail, false, null)) 1 else 0;
    }
    return result;
}

fn part2(input: []const u8) !u32 {
    var result: u32 = 0;
    var lines = std.mem.tokenizeScalar(u8, input, '\n');
    while (lines.next()) |line| {
        const head, const tail = split(line);
        result += if (isSafe(head, tail, true, null)) 1 else 0;
    }
    return result;
}

const testInputs = [_]struct { []const u8, u32, u32 }{.{
    \\7 6 4 2 1
    \\1 2 7 8 9
    \\9 7 6 2 1
    \\1 3 2 4 5
    \\8 6 4 4 1
    \\1 3 6 7 9
    ,
    2,
    4,
}};

test "day 02 part 1 sample 1" {
    const input, const expected, _ = testInputs[0];
    const result = try part1(input);
    try std.testing.expectEqual(@as(u32, expected), result);
}

test "day 02 part 1" {
    const gpa = std.testing.allocator;
    const input = try aoc.readInput("02", gpa);
    defer gpa.free(input);
    const result = try part1(input);
    try std.testing.expectEqual(@as(u32, 490), result);
}

test "day 02 part 2 sample 1" {
    const input, _, const expected = testInputs[0];
    const result = try part2(input);
    try std.testing.expectEqual(@as(u32, expected), result);
}

test "day 02 part 2" {
    const gpa = std.testing.allocator;
    const input = try aoc.readInput("02", gpa);
    defer gpa.free(input);
    const result = try part2(input);
    try std.testing.expectEqual(@as(u32, 536), result);
}
