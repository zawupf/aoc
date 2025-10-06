const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

fn parse(gpa: Allocator, input: []const u8) !struct { []u32, []u32 } {
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

pub fn part1(input: []const u8, gpa: Allocator) !u32 {
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

pub fn part2(input: []const u8, gpa: Allocator) !u32 {
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

pub const Day = aoc.DayInfo("01", u32, u32, 1651298, 21306195, @This(), &.{.{
    .expected1 = 11,
    .expected2 = 31,
    .input =
    \\3   4
    \\4   3
    \\2   5
    \\1   3
    \\3   9
    \\3   3
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
