const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Score = enum { countEnds, countWays };

const Field = struct {
    value: u8,
    visitCount: u8,

    pub fn fromChar(c: u8) @This() {
        return .{
            .value = std.fmt.charToDigit(c, 10) catch unreachable,
            .visitCount = 0,
        };
    }
};

const Grid = aoc.Grid(Field, usize);

const directions = std.enums.values(aoc.Orientation);

pub fn solve(comptime score: Score, input: []const u8, gpa: Allocator) !u32 {
    var grid = try Grid.initMapped(input, Field.fromChar, gpa);
    defer grid.deinit(gpa);

    var stack = std.ArrayList(Grid.Pos).empty;
    defer stack.deinit(gpa);

    var totalScore: u32 = 0;
    for (grid.buf, 0..) |field, i| {
        if (field.value != 0) continue;

        if (score == .countEnds) {
            for (grid.buf) |*f| f.visitCount = 0;
        }

        try stack.append(gpa, grid.indexToPos(i));
        while (stack.pop()) |p| {
            const nextValue = grid.at(p).value + 1;
            for (directions) |direction| {
                if (direction.nextOrNull(p, grid)) |p_| {
                    const field_ = grid.ptr(p_);
                    const keepGoing = switch (score) {
                        .countEnds => field_.value == nextValue and field_.visitCount == 0,
                        .countWays => field_.value == nextValue,
                    };
                    if (keepGoing) {
                        field_.visitCount += 1;
                        try stack.append(gpa, p_);
                    }
                }
            }
        }

        if (score == .countEnds) {
            for (grid.buf) |f| totalScore += if (f.value == 9 and f.visitCount > 0) 1 else 0;
        }
    }

    if (score == .countWays) {
        for (grid.buf) |f| totalScore += if (f.value == 9) f.visitCount else 0;
    }

    return totalScore;
}

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    return try solve(.countEnds, input, gpa);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    return try solve(.countWays, input, gpa);
}

pub const Day = aoc.DayInfo("10", u32, u32, 789, 1735, @This(), &.{.{
    .expected1 = 36,
    .expected2 = 81,
    .input =
    \\89010123
    \\78121874
    \\87430965
    \\96549874
    \\45678903
    \\32019012
    \\01329801
    \\10456732
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
