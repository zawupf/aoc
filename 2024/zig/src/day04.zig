const std = @import("std");

const aoc = @import("aoc_utils");

fn part1(input: []const u8, gpa: std.mem.Allocator) !u32 {
    const gridBuffer = try gpa.dupe(u8, input);
    defer gpa.free(gridBuffer);
    const grid = aoc.Grid.init(gridBuffer);
    var result: u32 = 0;
    var p = aoc.Pos{ .x = 0, .y = 0 };
    while (p.y < grid.height) : (p.y += 1) {
        p.x = 0;
        while (p.x < grid.width) : (p.x += 1) {
            if (grid.at(p) != 'X') continue;
            inline for (@typeInfo(aoc.Direction).@"enum".fields) |field| {
                const dir = @field(aoc.Direction, field.name);
                if (grid.subarray(3, p, dir, 1)) |word| {
                    if (std.mem.eql(u8, &word, "MAS")) result += 1;
                }
            }
        }
    }
    return result;
}

fn part2(input: []const u8, gpa: std.mem.Allocator) !u32 {
    const gridBuffer = try gpa.dupe(u8, input);
    defer gpa.free(gridBuffer);
    const grid = aoc.Grid.init(gridBuffer);
    var result: u32 = 0;
    var p = aoc.Pos{ .x = 1, .y = 1 };
    while (p.y < grid.height - 1) : (p.y += 1) {
        p.x = 1;
        while (p.x < grid.width - 1) : (p.x += 1) {
            if (grid.at(p) != 'A') {
                continue;
            }
            const corners = [_]u8{
                grid.at(aoc.Direction.north_west.next(p)),
                grid.at(aoc.Direction.north_east.next(p)),
                grid.at(aoc.Direction.south_west.next(p)),
                grid.at(aoc.Direction.south_east.next(p)),
            };
            if (std.mem.eql(u8, &corners, "MMSS") or
                std.mem.eql(u8, &corners, "SSMM") or
                std.mem.eql(u8, &corners, "MSMS") or
                std.mem.eql(u8, &corners, "SMSM")) result += 1;
        }
    }
    return result;
}

const Day = aoc.DayInfo("04", u32, u32, 2517, 1960, &.{
    .{
        .expected1 = 12,
        .expected2 = null,
        .input =
        \\XMAS.SAMX
        \\MM.....MM
        \\A.A...A.A
        \\S..S.S..S
        \\.........
        \\S..S.S..S
        \\A.A...A.A
        \\MM.....MM
        \\XMAS.SAMX
        ,
    },
    .{
        .expected1 = 3,
        .expected2 = null,
        .input =
        \\XMAS
        \\MM..
        \\A.A.
        \\S..S
        ,
    },
    .{
        .expected1 = 3,
        .expected2 = null,
        .input =
        \\XMAS
        \\MM..
        \\A.A.
        \\S..S
        ,
    },
    .{
        .expected1 = 3,
        .expected2 = null,
        .input =
        \\S..S
        \\A.A.
        \\MM..
        \\XMAS
        ,
    },
    .{
        .expected1 = 3,
        .expected2 = null,
        .input =
        \\SAMX
        \\..MM
        \\.A.A
        \\S..S
        ,
    },
    .{
        .expected1 = 3,
        .expected2 = null,
        .input =
        \\S..S
        \\.A.A
        \\..MM
        \\SAMX
        ,
    },
    .{
        .expected1 = 18,
        .expected2 = 9,
        .input =
        \\MMMSXXMASM
        \\MSAMXMSMSA
        \\AMXSXMAAMM
        \\MSAMASMSMX
        \\XMASAMXAMM
        \\XXAMMXXAMA
        \\SMSMSASXSS
        \\SAXAMASAAA
        \\MAMMMXMMMM
        \\MXMXAXMASX
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
