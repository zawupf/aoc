const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    return solve(.part1, input, gpa);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    return solve(.part2, input, gpa);
}

fn countDigits(n: u64) u8 {
    var count: u8 = 0;
    var num = n;
    while (num != 0) : (num /= 10) {
        count += 1;
    }
    return count;
}

const Range = [2]u64;
const PartId = enum { part1, part2 };
fn solve(comptime part: PartId, input: []const u8, gpa: Allocator) !u64 {
    var ranges = std.ArrayList(Range).empty;
    defer ranges.deinit(gpa);

    var rangesIter = aoc.split(aoc.trim(input), ',');
    while (rangesIter.next()) |range_str| {
        var valueIter = aoc.split(range_str, '-');
        const a = try std.fmt.parseUnsigned(u64, valueIter.next().?, 10);
        const b = try std.fmt.parseUnsigned(u64, valueIter.next().?, 10);
        const da = countDigits(a);
        const db = countDigits(b);
        const n = db - da + 1;
        var i: u8 = 0;
        while (i < n) : (i += 1) {
            try ranges.append(gpa, .{
                if (i == 0) a else try std.math.powi(u64, 10, da + i - 1),
                if (i == n - 1) b else try std.math.powi(u64, 10, da + i) - 1,
            });
        }
    }

    var sum: u64 = 0;
    var ids = std.AutoHashMapUnmanaged(u64, void).empty;
    defer ids.deinit(gpa);
    for (ranges.items) |range| {
        ids.clearRetainingCapacity();

        const start, const end = range;
        const digitCount = countDigits(start);
        const maxSliceCount = if (part == .part1) 2 else digitCount;

        var sliceCount: u8 = 2;
        while (sliceCount <= maxSliceCount) : (sliceCount += 1) {
            if (digitCount % sliceCount != 0) continue;

            const sliceSize = digitCount / sliceCount;
            const d = try std.math.powi(u64, 10, sliceSize * (sliceCount - 1));
            const a, const b = .{ start / d, end / d };

            const f = try std.math.powi(u64, 10, sliceSize);
            var i = a;
            while (i <= b) : (i += 1) {
                var n: u64 = 0;
                var j = sliceCount;
                while (j > 0) : (j -= 1) n = n * f + i;
                if (n < start or n > end) continue;

                const gop = try ids.getOrPut(gpa, n);
                if (gop.found_existing) continue;

                sum += n;
            }
        }
    }

    return sum;
}

pub const Day = aoc.DayInfo("02", u64, u64, 23039913998, 35950619148, @This(), &.{.{
    .expected1 = 1227775554,
    .expected2 = 4174379265,
    .input =
    \\11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124
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
