const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Grid = aoc.Grid(Field, u8);
const Field = enum(u8) { wall = '#', track = '.', start = 'S', end = 'E' };

const dirs = std.enums.values(aoc.Orientation);

pub fn solve(comptime maxCheatTime: u8, comptime threshold: u8, input: []const u8, gpa: Allocator) !u32 {
    const grid = try Grid.init(input, gpa);
    defer grid.deinit(gpa);

    var distances = try aoc.Grid(i16, u8).initSize(grid.width, grid.height, 0, gpa);
    defer distances.deinit(gpa);

    const track = blk: {
        var track = std.ArrayList(Grid.Pos).empty;
        errdefer track.deinit(gpa);
        var i: i16 = 0;
        var pos = grid.findScalar(.start).?;
        while (true) {
            i += 1;
            distances.setAt(pos, i);
            try track.append(gpa, pos);

            if (grid.at(pos) == .end) break :blk try track.toOwnedSlice(gpa);

            pos = dir: for (dirs) |dir| {
                const nextPos = dir.next(pos);
                if (grid.at(nextPos) != .wall and distances.at(nextPos) == 0) break :dir nextPos;
            } else unreachable;
        } else unreachable;
    };
    defer gpa.free(track);

    var result: u32 = 0;
    var i: i16 = 0;
    for (track) |pos| {
        i += 1;

        if (maxCheatTime == 2) {
            if (aoc.Orientation.up.stepOrNull(grid, pos, maxCheatTime)) |p| {
                const d = distances.at(p) - i - maxCheatTime;
                if (d >= threshold) result += 1;
            }
            if (aoc.Orientation.down.stepOrNull(grid, pos, maxCheatTime)) |p| {
                const d = distances.at(p) - i - maxCheatTime;
                if (d >= threshold) result += 1;
            }
            if (aoc.Orientation.left.stepOrNull(grid, pos, maxCheatTime)) |p| {
                const d = distances.at(p) - i - maxCheatTime;
                if (d >= threshold) result += 1;
            }
            if (aoc.Orientation.right.stepOrNull(grid, pos, maxCheatTime)) |p| {
                const d = distances.at(p) - i - maxCheatTime;
                if (d >= threshold) result += 1;
            }
        } else {
            const px: i16 = @intCast(pos.x);
            const py: i16 = @intCast(pos.y);

            var dy: i16 = -@as(i16, maxCheatTime);
            while (dy <= maxCheatTime) : (dy += 1) {
                const maxDeltaX: i16 = @intCast(maxCheatTime - @abs(dy));
                var dx: i16 = -maxDeltaX;
                while (dx <= maxDeltaX) : (dx += 1) {
                    const cheatTime: i16 = @intCast(@abs(dy) + @abs(dx));
                    const x = px + dx;
                    const y = py + dy;
                    if (x < 0 or y < 0 or x >= grid.width or y >= grid.height) continue;
                    const p: Grid.Pos = .{ .x = @intCast(x), .y = @intCast(y) };
                    const d = distances.at(p) - i - cheatTime;
                    if (d >= threshold) result += 1;
                }
            }
        }
    }

    return result;
}

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    return try solve(2, 100, input, gpa);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    return try solve(20, 100, input, gpa);
}

pub const Day = aoc.DayInfo("20", u32, u32, 1415, 1022577, @This(), &.{.{
    .expected1 = null,
    .expected2 = null,
    .input =
    \\###############
    \\#...#...#.....#
    \\#.#.#.#.#.###.#
    \\#S#...#.#.#...#
    \\#######.#.#.###
    \\#######.#.#...#
    \\#######.#.###.#
    \\###..E#...#...#
    \\###.#######.###
    \\#...###...#...#
    \\#.#####.#.###.#
    \\#.#...#.#.#...#
    \\#.#.#.#.#.#.###
    \\#...#...#...###
    \\###############
    ,
}});

test "samples 1" {
    try std.testing.expectEqual(5, try solve(2, 20, Day.tests[0].input, std.testing.allocator));
    try Day.testPart1Samples();
}
test "samples 2" {
    try std.testing.expectEqual(7, try solve(20, 74, Day.tests[0].input, std.testing.allocator));
    try Day.testPart2Samples();
}
test "part 1" {
    try Day.testPart1();
}
test "part 2" {
    try Day.testPart2();
}
