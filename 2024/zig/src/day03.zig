const std = @import("std");

const aoc = @import("aoc_utils");

const MulIterType = enum { simple, advanced };
fn MulIterator(comptime T: type, comptime kind: MulIterType) type {
    return struct {
        buffer: []const u8,
        index: ?usize,
        enabled: bool,

        const Self = @This();
        const prefix = "mul(";
        const suffix = ")";
        const delimiter = ",";
        const do = "do()";
        const dont = "don't()";

        pub fn next(self: *Self) ?T {
            const index = self.index orelse return null;

            const mulIndex = std.mem.indexOfPos(u8, self.buffer, index, prefix) orelse {
                self.index = null;
                return null;
            };
            const start = switch (kind) {
                .simple => mulIndex,
                .advanced => blk: {
                    const doIndex = std.mem.indexOfPos(u8, self.buffer, index, do) orelse mulIndex;
                    const dontIndex = std.mem.indexOfPos(u8, self.buffer, index, dont) orelse mulIndex;
                    const minIndex = @min(mulIndex, doIndex, dontIndex);
                    if (minIndex == mulIndex) break :blk mulIndex;
                    self.enabled = minIndex == doIndex;
                    self.index = self.index.? + if (minIndex == doIndex) do.len else dont.len;
                    return self.next();
                },
            };

            const contentIndex = start + prefix.len;
            const end = std.mem.indexOfPos(u8, self.buffer, contentIndex, suffix) orelse {
                self.index = null;
                return null;
            };

            const nextToken = switch (kind) {
                .simple => std.mem.indexOfPos(u8, self.buffer, contentIndex, prefix),
                .advanced => @as(?usize, @min(
                    std.mem.indexOfPos(u8, self.buffer, contentIndex, prefix) orelse end,
                    std.mem.indexOfPos(u8, self.buffer, contentIndex, do) orelse end,
                    std.mem.indexOfPos(u8, self.buffer, contentIndex, dont) orelse end,
                )),
            };
            if (nextToken) |n| if (n < end) {
                self.index = n;
                return self.next();
            };
            self.index = end + suffix.len;
            if (!self.enabled) return self.next();

            const content = self.buffer[contentIndex..end];
            if (content.len < 3 or content.len > 7) return self.next();

            const mid = std.mem.indexOfAnyPos(u8, content, 1, delimiter) orelse return self.next();

            const left = content[0..mid];
            if (left.len < 1 or left.len > 3) return self.next();
            for (left) |c| if (!std.ascii.isDigit(c)) return self.next();
            const a = std.fmt.parseInt(T, left, 10) catch unreachable;

            const right = content[mid + 1 ..];
            if (right.len < 1 or right.len > 3) return self.next();
            for (right) |c| if (!std.ascii.isDigit(c)) return self.next();
            const b = std.fmt.parseInt(T, right, 10) catch unreachable;

            return a * b;
        }
    };
}

fn mulIter(input: []const u8, comptime kind: MulIterType) MulIterator(u32, kind) {
    return MulIterator(u32, kind){
        .buffer = input,
        .index = 0,
        .enabled = true,
    };
}

fn part1(input: []const u8, gpa: std.mem.Allocator) !u32 {
    _ = gpa;
    var sum: u32 = 0;
    var products = mulIter(input, .simple);
    while (products.next()) |v| sum += v;
    return sum;
}

fn part2(input: []const u8, gpa: std.mem.Allocator) !u32 {
    _ = gpa;
    var sum: u32 = 0;
    var products = mulIter(input, .advanced);
    while (products.next()) |v| sum += v;
    return sum;
}

const Day = aoc.DayInfo("03", u32, u32, 173529487, 99532691, &.{
    .{
        .expected1 = 161,
        .expected2 = null,
        .input =
        \\xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))
        ,
    },
    .{
        .expected1 = null,
        .expected2 = 48,
        .input =
        \\xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))
        ,
    },
});

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
