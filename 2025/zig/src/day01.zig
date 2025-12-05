const std = @import("std");
const Allocator = std.mem.Allocator;
const sign = std.math.sign;

const aoc = @import("aoc_utils");

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    _ = gpa;

    var count: u16 = 0;
    var pos: i16 = 50;
    var lines = aoc.splitLines(aoc.trim(input));
    while (lines.next()) |line| {
        const direction: i2 = if (line[0] == 'L') -1 else 1;
        const distance: i16 = @intCast(try std.fmt.parseUnsigned(u16, line[1..], 10));
        pos += direction * distance;
        if (@mod(pos, 100) == 0) count += 1;
    }
    return count;
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    _ = gpa;

    var count: u16 = 0;
    var pos: i16 = 50;
    var lines = aoc.splitLines(aoc.trim(input));
    while (lines.next()) |line| {
        const direction: i2 = if (line[0] == 'L') -1 else 1;
        const distance: u16 = try std.fmt.parseUnsigned(u16, line[1..], 10);
        const overflows: u16 = distance / 100;
        const dist: i16 = @rem(@as(i16, @intCast(distance)), 100);

        const nextPos = pos + direction * dist;
        const increment =
            nextPos <= -100 or
            nextPos >= 100 or
            nextPos == 0 or
            (sign(pos) != sign(nextPos) and
                sign(pos) != 0 and
                sign(nextPos) != 0);
        count += overflows + if (increment) @as(u16, 1) else 0;
        pos = @rem(nextPos, 100);
    }
    return count;
}

pub const Day = aoc.DayInfo("01", u16, u16, 1120, 6554, @This(), &.{.{
    .expected1 = 3,
    .expected2 = 6,
    .input =
    \\L68
    \\L30
    \\R48
    \\L5
    \\R60
    \\L55
    \\L1
    \\L99
    \\R14
    \\L82
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
