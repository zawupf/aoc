const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Grid = aoc.Grid(Field, u8);
const Field = enum(u2) { accessible, corrupted };

const dirs = std.enums.values(aoc.Orientation);

const Variant = union(enum) {
    part1: struct { size: usize, len: usize },
    part2: struct { size: usize },
};
const Result = union(enum) {
    part1: Day.Result1,
    part2: Day.Result2,
};

pub fn solve(comptime variant: Variant, input: []const u8, gpa: Allocator) !Result {
    const size = switch (variant) {
        .part1 => |v| v.size,
        .part2 => |v| v.size,
    };
    const len = switch (variant) {
        .part1 => |v| v.len,
        .part2 => input.len,
    };

    var grid: Grid = try .initSize(size, size, .accessible, gpa);
    defer grid.deinit(gpa);

    var lines = aoc.splitLines(input);
    var bytes: std.ArrayList([]const u8) = .empty;
    defer bytes.deinit(gpa);
    var i: usize = 0;
    while (lines.next()) |line| {
        switch (variant) {
            .part1 => {
                if (i == len) {
                    try bytes.append(gpa, "0,0");
                    break;
                }
                i += 1;
            },
            .part2 => try bytes.append(gpa, line),
        }

        var it = aoc.split(line, ',');
        const x = try std.fmt.parseUnsigned(u8, it.next().?, 10);
        const y = try std.fmt.parseUnsigned(u8, it.next().?, 10);
        grid.setAt(.{ .x = x, .y = y }, .corrupted);
    }

    var stepsCount: aoc.Grid(u16, u8) = try .initSize(size, size, 0, gpa);
    defer stepsCount.deinit(gpa);

    const State = struct { pos: Grid.Pos, steps: u16 };
    var stack: std.ArrayList(State) = .empty;
    defer stack.deinit(gpa);

    var starts: std.ArrayList(State) = .empty;
    defer starts.deinit(gpa);
    try starts.append(gpa, .{ .pos = .{ .x = 0, .y = 0 }, .steps = 0 });

    while (bytes.pop()) |line| {
        var it = aoc.split(line, ',');
        const x = try std.fmt.parseUnsigned(u8, it.next().?, 10);
        const y = try std.fmt.parseUnsigned(u8, it.next().?, 10);
        grid.setAt(.{ .x = x, .y = y }, .accessible);

        try stack.appendSlice(gpa, starts.items);
        starts.clearRetainingCapacity();
        while (stack.pop()) |current| {
            switch (variant) {
                .part1 => {},
                .part2 => {
                    if (current.pos.x == size - 1 and current.pos.y == size - 1) {
                        return .{ .part2 = line };
                    }

                    if (grid.at(current.pos) == .corrupted) {
                        try starts.append(gpa, current);
                        continue;
                    }
                },
            }

            const steps = stepsCount.at(current.pos);
            if (steps != 0 and steps <= current.steps) continue;

            stepsCount.setAt(current.pos, current.steps);
            const nextSteps = current.steps + 1;
            for (dirs) |dir| {
                if (dir.nextOrNull(grid, current.pos)) |pos| {
                    switch (variant) {
                        .part1 => if (grid.at(pos) == .accessible) {
                            try stack.append(gpa, .{ .pos = pos, .steps = nextSteps });
                        },
                        .part2 => {
                            const array = switch (grid.at(pos)) {
                                .corrupted => &starts,
                                .accessible => &stack,
                            };
                            try array.append(gpa, .{ .pos = pos, .steps = nextSteps });
                        },
                    }
                }
            }
        }

        switch (variant) {
            .part1 => {
                return .{ .part1 = stepsCount.at(.{ .x = size - 1, .y = size - 1 }) };
            },
            .part2 => {},
        }
    }

    unreachable;
}

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    const result = try solve(.{ .part1 = .{ .size = 71, .len = 1024 } }, input, gpa);
    return result.part1;
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    const result = try solve(.{ .part2 = .{ .size = 71 } }, input, gpa);
    return result.part2;
}

pub const Day = aoc.DayInfo("18", u32, []const u8, 292, "58,44", @This(), &.{.{
    .expected1 = null,
    .expected2 = null,
    .input =
    \\
    ,
}});

test "samples 1 solve 1 + 2" {
    const input =
        \\5,4
        \\4,2
        \\4,5
        \\3,0
        \\2,1
        \\6,3
        \\2,4
        \\1,5
        \\0,6
        \\3,3
        \\2,6
        \\5,1
        \\1,2
        \\5,5
        \\2,5
        \\6,5
        \\1,4
        \\0,4
        \\6,4
        \\1,1
        \\6,1
        \\1,0
        \\0,5
        \\1,6
        \\2,0
    ;

    const expected1: Day.Result1 = 22;
    const result1 = try solve(.{ .part1 = .{ .size = 7, .len = 12 } }, input, std.testing.allocator);
    try std.testing.expectEqual(expected1, result1.part1);

    const expected2: Day.Result2 = "6,1";
    const result2 = try solve(.{ .part2 = .{ .size = 7 } }, input, std.testing.allocator);
    try std.testing.expectEqualStrings(expected2, result2.part2);
}

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
