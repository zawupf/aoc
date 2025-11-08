const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Mode = enum { part1, part2 };

pub fn solve(comptime mode: Mode, input: []const u8, gpa: Allocator) !u64 {
    var lines = aoc.splitLines(input);

    var patternsIter = aoc.splitSeq(lines.next().?, ", ");
    var patterns = std.ArrayList([]const u8).empty;
    defer patterns.deinit(gpa);
    while (patternsIter.next()) |pattern| try patterns.append(gpa, pattern);
    _ = lines.next().?;

    const Count = switch (mode) {
        .part1 => u1,
        .part2 => u64,
    };
    const Cache = std.StringHashMapUnmanaged(Count);
    var cache = Cache.empty;
    defer cache.deinit(gpa);
    const count = struct {
        fn count(design: []const u8, patterns_: []const []const u8, cache_: *Cache, gpa_: Allocator) !Count {
            if (design.len == 0) return 1;
            if (cache_.get(design)) |v| return v;

            var total: Count = 0;
            for (patterns_) |pattern| {
                if (std.mem.startsWith(u8, design, pattern)) {
                    const rest = design[pattern.len..];
                    total += try count(rest, patterns_, cache_, gpa_);
                    if (mode == .part1 and total != 0) break;
                }
            }
            try cache_.putNoClobber(gpa_, design, total);
            return total;
        }
    }.count;

    var result: u64 = 0;
    while (lines.next()) |design| {
        result += try count(design, patterns.items, &cache, gpa);
    }

    return result;
}

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    return try solve(.part1, input, gpa);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    return try solve(.part2, input, gpa);
}

pub const Day = aoc.DayInfo("19", u64, u64, 333, 678536865274732, @This(), &.{.{
    .expected1 = 6,
    .expected2 = 16,
    .input =
    \\r, wr, b, g, bwu, rb, gb, br
    \\
    \\brwrr
    \\bggr
    \\gbbr
    \\rrbgbr
    \\ubwu
    \\bwurrg
    \\brgr
    \\bbrgwb
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
