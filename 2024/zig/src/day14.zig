const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Pos = @Vector(2, i32);
const Vel = @Vector(2, i32);

fn Grid(comptime width: usize, comptime height: usize) type {
    return struct {
        ps: []Pos,
        vs: []const Vel,

        const Self = @This();

        pub fn init(input: []const u8, gpa: Allocator) !Self {
            var ps = std.ArrayList(Pos).empty;
            var vs = std.ArrayList(Vel).empty;
            errdefer {
                ps.deinit(gpa);
                vs.deinit(gpa);
            }

            var lines = aoc.splitLines(input);
            while (lines.next()) |line| {
                var buf: [4]i32 = undefined;
                var iter = aoc.tokenizeAny(line, "pv=, ");
                const ints = try aoc.parseInts(i32, &iter, &buf);
                try ps.append(gpa, ints[0..2].*);
                try vs.append(gpa, ints[2..4].*);
            }

            return .{
                .ps = try ps.toOwnedSlice(gpa),
                .vs = try vs.toOwnedSlice(gpa),
            };
        }

        pub fn deinit(self: *Self, gpa: Allocator) void {
            gpa.free(self.ps);
            gpa.free(self.vs);
        }

        pub fn step(self: *Self, comptime n: usize) void {
            comptime std.debug.assert(n > 0);
            const r: Pos = .{ width, height };
            for (self.ps, self.vs) |*p, v| {
                const p_ = p.* + if (n == 1) v else v * @as(Vel, @splat(n));
                p.* = @rem(@rem(p_, r) + r, r);
            }
        }

        pub fn quadrantCount(self: Self) @Vector(4, u32) {
            var counts = [4]u32{ 0, 0, 0, 0 };
            const mx = width / 2;
            const my = height / 2;
            for (self.ps) |p| {
                const x, const y = p;
                if (x == mx or y == my) continue;
                const quadrant =
                    @as(usize, if (x < mx) 0 else 1) +
                    @as(usize, if (y < my) 0 else 2);
                counts[quadrant] += 1;
            }
            return counts;
        }
    };
}

fn solve1(
    comptime width: usize,
    comptime height: usize,
    input: []const u8,
    gpa: Allocator,
) !Day.Result1 {
    var grid = try Grid(width, height).init(input, gpa);
    defer grid.deinit(gpa);
    grid.step(100);
    return @intCast(@reduce(.Mul, grid.quadrantCount()));
}

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    return solve1(101, 103, input, gpa);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    var grid = try Grid(101, 103).init(input, gpa);
    defer grid.deinit(gpa);

    const threshold = grid.ps.len / 2;
    var stepCount: u32 = 1;
    while (stepCount != 0) : (stepCount += 1) {
        grid.step(1);
        if (@reduce(.Max, grid.quadrantCount()) > threshold) return stepCount;
    }
    unreachable;
}

pub const Day = aoc.DayInfo("14", u32, u32, 233709840, 6620, @This(), &.{.{
    .expected1 = 12,
    .expected2 = null,
    .input =
    \\p=0,4 v=3,-3
    \\p=6,3 v=-1,-3
    \\p=10,3 v=-1,2
    \\p=2,0 v=2,-1
    \\p=0,0 v=1,3
    \\p=3,0 v=-2,-2
    \\p=7,6 v=-1,-3
    \\p=3,0 v=-1,-2
    \\p=9,3 v=2,3
    \\p=7,3 v=-1,2
    \\p=2,4 v=2,-3
    \\p=9,5 v=-3,-3
    ,
}});

test "samples 1" {
    const data = Day.tests[0];
    try std.testing.expectEqual(
        data.expected1,
        try solve1(11, 7, data.input, std.testing.allocator),
    );
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
