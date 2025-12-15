const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    return solve(.part1, input, gpa);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    return solve(.part2, input, gpa);
}

const PartId = enum { part1, part2 };
fn ResultType(comptime partId: PartId) type {
    return switch (partId) {
        .part1 => Day.Result1,
        .part2 => Day.Result2,
    };
}

fn solve(comptime partId: PartId, input: []const u8, gpa: Allocator) !ResultType(partId) {
    var rows = aoc.splitLines(aoc.trim(input));
    const beams = rows.next().?;

    const len = beams.len;
    const beamsCount = try gpa.alloc(u64, len);
    defer gpa.free(beamsCount);
    for (beams, beamsCount) |beam, *n| {
        n.* = if (beam == '.') 0 else 1;
    }

    var splitCount: Day.Result1 = 0;
    while (rows.next()) |row| {
        for (beamsCount, 0..) |n, i| {
            if (row[i] == '.' or n == 0) continue;

            splitCount += 1;
            const pn = beamsCount[i - 1 .. i + 2];
            pn[0] += n;
            pn[1] = 0;
            pn[2] += n;
        }
    }

    return switch (partId) {
        .part1 => splitCount,
        .part2 => blk: {
            var total: Day.Result2 = 0;
            for (beamsCount) |n| total += n;
            break :blk total;
        },
    };
}

pub const Day = aoc.DayInfo("07", u16, u64, 1672, 231229866702355, @This(), &.{.{
    .expected1 = 21,
    .expected2 = 40,
    .input =
    \\.......S.......
    \\...............
    \\.......^.......
    \\...............
    \\......^.^......
    \\...............
    \\.....^.^.^.....
    \\...............
    \\....^.^...^....
    \\...............
    \\...^.^...^.^...
    \\...............
    \\..^...^.....^..
    \\...............
    \\.^.^.^.^.^...^.
    \\...............
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
