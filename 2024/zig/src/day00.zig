//! By convention, root.zig is the root source file when making a library.
const std = @import("std");

const aoc = @import("aoc_utils");

fn part1(input: []const u8, gpa: std.mem.Allocator) !u32 {
    _ = gpa;
    _ = input;
    return 0;
}

fn part2(input: []const u8, gpa: std.mem.Allocator) !u32 {
    _ = gpa;
    _ = input;
    return 0;
}

const testInputs = [_]struct { []const u8, u32, u32 }{.{
    &.{},
    0,
    0,
}};

test "day 00 part 1 sample 1" {
    const input, const expected, _ = testInputs[0];
    const result = try part1(input, std.testing.allocator);
    try std.testing.expectEqual(expected, result);
}

test "day 00 part 1" {
    const gpa = std.testing.allocator;
    const input = try aoc.readInput("00", gpa);
    defer gpa.free(input);
    const result = try part1(input, gpa);
    try std.testing.expectEqual(@as(u32, 0), result);
}

test "day 00 part 2 sample 1" {
    const input, _, const expected = testInputs[0];
    const result = try part2(input, std.testing.allocator);
    try std.testing.expectEqual(expected, result);
}

test "day 00 part 2" {
    const gpa = std.testing.allocator;
    const input = try aoc.readInput("00", gpa);
    defer gpa.free(input);
    const result = try part2(input, gpa);
    try std.testing.expectEqual(@as(u32, 0), result);
}
