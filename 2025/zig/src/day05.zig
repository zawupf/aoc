const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Range = [2]u64;

fn parseRange(line: []const u8) !Range {
    var dashIter = aoc.split(line, '-');
    return .{
        try std.fmt.parseUnsigned(u64, dashIter.next().?, 10),
        try std.fmt.parseUnsigned(u64, dashIter.next().?, 10),
    };
}

fn parseRanges(input: []const u8, gpa: Allocator) ![]Range {
    var ranges = std.ArrayList(Range).empty;
    errdefer ranges.deinit(gpa);
    var rangeIter = aoc.splitLines(input);
    while (rangeIter.next()) |line| {
        try ranges.append(gpa, try parseRange(line));
    }
    return ranges.toOwnedSlice(gpa);
}

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    var chunksIter = aoc.splitChunks(aoc.trim(input));

    const ranges = try parseRanges(chunksIter.next().?, gpa);
    defer gpa.free(ranges);

    var count: u16 = 0;
    const idsInput = chunksIter.next().?;
    var idsIter = aoc.splitLines(idsInput);
    while (idsIter.next()) |idStr| {
        const id = try std.fmt.parseUnsigned(u64, idStr, 10);
        for (ranges) |r| {
            if (id >= r[0] and id <= r[1]) {
                count += 1;
                break;
            }
        }
    }

    return count;
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    var chunksIter = aoc.splitChunks(aoc.trim(input));

    const ranges = try parseRanges(chunksIter.next().?, gpa);
    defer gpa.free(ranges);
    if (ranges.len == 0) return 0;

    std.mem.sortUnstable(Range, ranges, {}, struct {
        pub fn inner(_: void, a: Range, b: Range) bool {
            return if (a[0] != b[0]) a[0] < b[0] else a[1] < b[1];
        }
    }.inner);

    var lastEnd = ranges[0][1];
    var count: u64 = lastEnd - ranges[0][0] + 1;
    for (ranges[1..]) |r| {
        if (r[0] > lastEnd) {
            count += r[1] - r[0] + 1;
            lastEnd = r[1];
        } else if (r[1] > lastEnd) {
            count += r[1] - lastEnd;
            lastEnd = r[1];
        }
    }

    return count;
}

pub const Day = aoc.DayInfo("05", u16, u64, 613, 336495597913098, @This(), &.{.{
    .expected1 = 3,
    .expected2 = 14,
    .input =
    \\3-5
    \\10-14
    \\16-20
    \\12-18
    \\
    \\1
    \\5
    \\8
    \\11
    \\17
    \\32
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
