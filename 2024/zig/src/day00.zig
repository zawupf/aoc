const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    _ = gpa;
    _ = input;
    return 0;
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    _ = gpa;
    _ = input;
    return 0;
}

pub const Day = aoc.DayInfo("00", u32, u32, null, null, @This(), &.{.{
    .expected1 = null,
    .expected2 = null,
    .input =
    \\
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
