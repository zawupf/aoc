const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Key = struct { u64, u8 };
const Cache = std.AutoHashMapUnmanaged(Key, u64);

fn digits(value: u64) u8 {
    std.debug.assert(value != 0);
    var n: u8 = 0;
    var v = value;
    while (v != 0) : (v /= 10) n += 1;
    return n;
}

test "digits" {
    try std.testing.expect(digits(1) == 1);
    try std.testing.expect(digits(9) == 1);
    try std.testing.expect(digits(10) == 2);
    try std.testing.expect(digits(99) == 2);
    try std.testing.expect(digits(100) == 3);
    try std.testing.expect(digits(999) == 3);
    try std.testing.expect(digits(1000) == 4);
    try std.testing.expect(digits(2024) == 4);
}

fn count(value: u64, blinks: u8, cache: *Cache, gpa: Allocator) !u64 {
    if (blinks == 0) return 1;

    const key = Key{ value, blinks };
    if (cache.get(key)) |v| {
        return v;
    }

    const n = if (value == 0) try count(1, blinks - 1, cache, gpa) else blk: {
        const len = digits(value);
        if (len % 2 == 0) {
            const f = try std.math.powi(u64, 10, len / 2);
            const left = value / f;
            const right = value % f;
            break :blk try count(left, blinks - 1, cache, gpa) + try count(right, blinks - 1, cache, gpa);
        } else {
            break :blk try count(value * 2024, blinks - 1, cache, gpa);
        }
    };

    try cache.put(gpa, key, n);
    return n;
}

pub fn solve(blinks: u8, input: []const u8, gpa: Allocator) !u64 {
    var cache = Cache.empty;
    defer cache.deinit(gpa);

    var result: u64 = 0;
    var iter = aoc.tokenize(aoc.trim(input), ' ');
    while (iter.next()) |value| {
        const v = try std.fmt.parseUnsigned(u64, value, 10);
        result += try count(v, blinks, &cache, gpa);
    }
    return result;
}

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    return try solve(25, input, gpa);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    return try solve(75, input, gpa);
}

pub const Day = aoc.DayInfo("11", u64, u64, 220722, 261952051690787, @This(), &.{.{
    .expected1 = 55312,
    .expected2 = null,
    .input =
    \\125 17
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
