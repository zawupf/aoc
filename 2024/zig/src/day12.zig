const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");
const Diag = aoc.Direction;
const Ori = aoc.Orientation;

const Grid = aoc.Grid(u8, usize);
const SelectionCriteria = enum { edges, corners };

const dirs = std.enums.values(Ori);
const up_left: std.EnumSet(Ori) = .initMany(&.{ .up, .left });
const up_right: std.EnumSet(Ori) = .initMany(&.{ .up, .right });
const down_left: std.EnumSet(Ori) = .initMany(&.{ .down, .left });
const down_right: std.EnumSet(Ori) = .initMany(&.{ .down, .right });

fn isArea(pos: ?Grid.Pos, grid: Grid, field: u8) bool {
    return if (pos) |p| grid.at(p) == field else false;
}

fn isEdge(pos: ?Grid.Pos, grid: Grid, field: u8) bool {
    return !isArea(pos, grid, field);
}

fn solve(comptime countingStrategy: SelectionCriteria, input: []const u8, gpa: Allocator) !u32 {
    const grid: Grid = try .init(input, gpa);
    defer grid.deinit(gpa);

    var visited: std.DynamicBitSetUnmanaged = try .initEmpty(gpa, grid.buf.len);
    defer visited.deinit(gpa);

    var stack: std.ArrayList(Grid.Pos) = .empty;
    defer stack.deinit(gpa);

    var result: u32 = 0;
    for (grid.buf, 0..) |field, idx| {
        if (visited.isSet(idx)) continue;

        visited.set(idx);
        var fieldCount: u32 = 1;
        var itemCount: u32 = 0;
        try stack.append(gpa, grid.indexToPos(idx));
        while (stack.pop()) |pos| {
            var edges: std.EnumSet(Ori) = .initEmpty();
            for (dirs) |dir| {
                const p = dir.nextOrNull(grid, pos);
                if (isEdge(p, grid, field)) {
                    if (countingStrategy == .edges) itemCount += 1 else edges.insert(dir);
                    continue;
                }

                const i = grid.posToIndex(p.?);
                if (visited.isSet(i)) continue;

                visited.set(i);
                fieldCount += 1;
                try stack.append(gpa, p.?);
            }

            if (countingStrategy == .corners) {
                if (edges.supersetOf(up_left)) itemCount += 1;
                if (edges.supersetOf(up_right)) itemCount += 1;
                if (edges.supersetOf(down_left)) itemCount += 1;
                if (edges.supersetOf(down_right)) itemCount += 1;

                const areas = edges.complement();
                if (areas.supersetOf(up_left) and !isArea(Diag.north_west.nextOrNull(grid, pos), grid, field)) itemCount += 1;
                if (areas.supersetOf(up_right) and !isArea(Diag.north_east.nextOrNull(grid, pos), grid, field)) itemCount += 1;
                if (areas.supersetOf(down_left) and !isArea(Diag.south_west.nextOrNull(grid, pos), grid, field)) itemCount += 1;
                if (areas.supersetOf(down_right) and !isArea(Diag.south_east.nextOrNull(grid, pos), grid, field)) itemCount += 1;
            }
        }

        result += fieldCount * itemCount;
    }
    return result;
}

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    return try solve(.edges, input, gpa);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    return try solve(.corners, input, gpa);
}

pub const Day = aoc.DayInfo("12", u32, u32, 1381056, 834828, @This(), &.{.{
    .expected1 = 1930,
    .expected2 = 1206,
    .input =
    \\RRRRIICCFF
    \\RRRRIICCCF
    \\VVRRRCCFFF
    \\VVRCCCJFFF
    \\VVVVCJJCFE
    \\VVIVCCJJEE
    \\VVIIICJJEE
    \\MIIIIIJJEE
    \\MIIISIJEEE
    \\MMMISSJEEE
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
