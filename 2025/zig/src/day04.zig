const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Field = enum(u8) { empty = '.', roll = '@' };
const Grid = aoc.Grid(Field, u8);

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    const grid = try Grid.init(input, gpa);
    defer grid.deinit(gpa);

    const rollsBuffer = try gpa.alloc(Grid.Pos, grid.buf.len);
    defer gpa.free(rollsBuffer);

    const rolls = try findRemovableRolls(&grid, null, rollsBuffer);
    return @intCast(rolls.len);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    var grid = try Grid.init(input, gpa);
    defer grid.deinit(gpa);

    const rollsBuffer = try gpa.alloc(Grid.Pos, grid.buf.len);
    defer gpa.free(rollsBuffer);

    var count: u16 = 0;
    var rolls = try findRemovableRolls(&grid, null, rollsBuffer);

    var nextCandidates = std.AutoArrayHashMapUnmanaged(Grid.Pos, void).empty;
    defer nextCandidates.deinit(gpa);
    while (rolls.len > 0) {
        count += @intCast(rolls.len);

        nextCandidates.clearRetainingCapacity();
        for (rolls) |pos| grid.setAt(pos, .empty);
        for (rolls) |pos| {
            for (dirs) |dir| {
                if (dir.nextOrNull(grid, pos)) |next| {
                    if (grid.at(next) == .roll) {
                        try nextCandidates.put(gpa, next, {});
                    }
                }
            }
        }

        rolls = try findRemovableRolls(&grid, nextCandidates.keys(), rollsBuffer);
    }

    return count;
}

fn findRemovableRolls(grid: *const Grid, process: ?[]Grid.Pos, buffer: []Grid.Pos) ![]Grid.Pos {
    var rolls = std.ArrayList(Grid.Pos).initBuffer(buffer);

    if (process) |positions| {
        for (positions) |pos| {
            if (canRemoveRoll(grid, pos)) try rolls.appendBounded(pos);
        }
    } else {
        for (grid.buf, 0..) |field, index| {
            if (field != .roll) continue;
            const pos = grid.indexToPos(index);
            if (canRemoveRoll(grid, pos)) try rolls.appendBounded(pos);
        }
    }

    return rolls.items;
}

const dirs = std.enums.values(aoc.Direction);
fn canRemoveRoll(grid: *const Grid, pos: Grid.Pos) bool {
    std.debug.assert(grid.at(pos) == .roll);
    var adjacentRollsCount: u8 = 0;
    for (dirs) |dir| {
        if (dir.nextOrNull(grid.*, pos)) |next| {
            if (grid.at(next) != .roll) continue;
            adjacentRollsCount += 1;
            if (adjacentRollsCount > 3) return false;
        }
    }
    return true;
}

pub const Day = aoc.DayInfo("04", u16, u16, 1523, 9290, @This(), &.{.{
    .expected1 = 13,
    .expected2 = 43,
    .input =
    \\..@@.@@@@.
    \\@@@.@.@.@@
    \\@@@@@.@.@@
    \\@.@@@@..@.
    \\@@.@@@@.@@
    \\.@@@@@@@.@
    \\.@.@.@.@@@
    \\@.@@@.@@@@
    \\.@@@@@@@@.
    \\@.@.@@@.@.
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
