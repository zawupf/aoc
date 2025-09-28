const std = @import("std");

const aoc = @import("aoc_utils");

fn part1(input: []const u8, gpa: std.mem.Allocator) !Day.Result1 {
    _ = gpa;
    _ = input;
    return 0;
}

fn part2(input: []const u8, gpa: std.mem.Allocator) !Day.Result2 {
    _ = gpa;
    _ = input;
    return 0;
}

const Day = aoc.DayInfo("00", u32, u32, null, null, &.{});

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
