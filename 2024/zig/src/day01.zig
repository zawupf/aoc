//! By convention, root.zig is the root source file when making a library.
const std = @import("std");

const aoc = @import("aoc_utils");

fn parse(gpa: std.mem.Allocator, input: []const u8) !struct { []u32, []u32 } {
    var list1: std.ArrayList(u32) = .empty;
    var list2: std.ArrayList(u32) = .empty;
    errdefer {
        list1.deinit(gpa);
        list2.deinit(gpa);
    }

    var lines = aoc.splitLines(input);
    while (lines.next()) |line| {
        var numbers = aoc.tokenize(line, ' ');
        const a = try std.fmt.parseInt(u32, numbers.next().?, 10);
        const b = try std.fmt.parseInt(u32, numbers.next().?, 10);
        try list1.append(gpa, a);
        try list2.append(gpa, b);
    }

    const l1 = try list1.toOwnedSlice(gpa);
    errdefer gpa.free(l1);
    const l2 = try list2.toOwnedSlice(gpa);

    return .{ l1, l2 };
}

fn part1(input: []const u8, gpa: std.mem.Allocator) !u32 {
    const list1, const list2 = try parse(gpa, input);
    defer {
        gpa.free(list1);
        gpa.free(list2);
    }

    const ascending = std.sort.asc(u32);
    std.sort.block(u32, list1, {}, ascending);
    std.sort.block(u32, list2, {}, ascending);

    var result: u32 = 0;
    for (list1, list2) |v1, v2| {
        result += @max(v1, v2) - @min(v1, v2);
    }

    return result;
}

fn part2(input: []const u8, gpa: std.mem.Allocator) !u32 {
    const list1, const list2 = try parse(gpa, input);
    defer {
        gpa.free(list1);
        gpa.free(list2);
    }

    std.sort.block(u32, list2, {}, std.sort.asc(u32));

    var result: u32 = 0;
    for (list1) |v| {
        const eq = struct {
            fn order(a: u32, b: u32) std.math.Order {
                return std.math.order(a, b);
            }
        }.order;
        const a, const b = std.sort.equalRange(u32, list2, v, eq);
        result += v * @as(u32, @intCast(b - a));
    }

    return result;
}

const testInputs = [_]struct { []const u8, u32, u32 }{.{
    \\3   4
    \\4   3
    \\2   5
    \\1   3
    \\3   9
    \\3   3
    ,
    11,
    31,
}};

test "day 01 part 1 sample 1" {
    const input, const expected, _ = testInputs[0];
    const result = try part1(input, std.testing.allocator);
    try std.testing.expectEqual(expected, result);
}

test "day 01 part 1" {
    const gpa = std.testing.allocator;
    const input = try aoc.readInput("01", gpa);
    defer gpa.free(input);
    const result = try part1(input, gpa);
    try std.testing.expectEqual(@as(u32, 1651298), result);
}

test "day 01 part 2 sample 1" {
    const input, _, const expected = testInputs[0];
    const result = try part2(input, std.testing.allocator);
    try std.testing.expectEqual(expected, result);
}

test "day 01 part 2" {
    const gpa = std.testing.allocator;
    const input = try aoc.readInput("01", gpa);
    defer gpa.free(input);
    const result = try part2(input, gpa);
    try std.testing.expectEqual(@as(u32, 21306195), result);
}
