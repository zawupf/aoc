//! By convention, root.zig is the root source file when making a library.
const std = @import("std");

const aoc = @import("aoc_utils");

fn part1(input: []const u8) !u32 {
    const grid = aoc.GridView.init(input);
    var result: u32 = 0;
    var r: usize = 0;
    while (r < grid.height) : (r += 1) {
        var c: usize = 0;
        while (c < grid.width) : (c += 1) {
            if (grid.at(r, c) != 'X') continue;
            inline for (@typeInfo(aoc.Direction).@"enum".fields) |field| {
                const dir = @field(aoc.Direction, field.name);
                if (grid.subarray(3, r, c, dir, 1)) |word| {
                    if (std.mem.eql(u8, &word, "MAS")) result += 1;
                }
            }
        }
    }
    return result;
}

fn part2(input: []const u8) !u32 {
    const grid = aoc.GridView.init(input);
    var result: u32 = 0;
    var r: usize = 1;
    while (r < grid.height - 1) : (r += 1) {
        var c: usize = 1;
        while (c < grid.width - 1) : (c += 1) {
            if (grid.at(r, c) != 'A') {
                continue;
            }
            const corners = [_]u8{
                grid.at(r - 1, c - 1), grid.at(r - 1, c + 1),
                grid.at(r + 1, c - 1), grid.at(r + 1, c + 1),
            };
            if (std.mem.eql(u8, &corners, "MMSS") or
                std.mem.eql(u8, &corners, "SSMM") or
                std.mem.eql(u8, &corners, "MSMS") or
                std.mem.eql(u8, &corners, "SMSM")) result += 1;
        }
    }
    return result;
}

const testInputs = [_]struct { []const u8, u32, u32 }{
    .{
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
        12,
        0,
    },
    .{
        \\XMAS
        \\MM..
        \\A.A.
        \\S..S
        ,
        3,
        0,
    },
    .{
        \\S..S
        \\A.A.
        \\MM..
        \\XMAS
        ,
        3,
        0,
    },
    .{
        \\SAMX
        \\..MM
        \\.A.A
        \\S..S
        ,
        3,
        0,
    },
    .{
        \\S..S
        \\.A.A
        \\..MM
        \\SAMX
        ,
        3,
        0,
    },
    .{
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
        18,
        9,
    },
};

test "day 04 part 1 sample 1" {
    const input, const expected, _ = testInputs[0];
    const result = try part1(input);
    try std.testing.expectEqual(expected, result);
}
test "day 04 part 1 sample 2" {
    const input, const expected, _ = testInputs[1];
    const result = try part1(input);
    try std.testing.expectEqual(expected, result);
}
test "day 04 part 1 sample 3" {
    const input, const expected, _ = testInputs[2];
    const result = try part1(input);
    try std.testing.expectEqual(expected, result);
}
test "day 04 part 1 sample 4" {
    const input, const expected, _ = testInputs[3];
    const result = try part1(input);
    try std.testing.expectEqual(expected, result);
}
test "day 04 part 1 sample 5" {
    const input, const expected, _ = testInputs[4];
    const result = try part1(input);
    try std.testing.expectEqual(expected, result);
}

test "day 04 part 1 sample 6" {
    const input, const expected, _ = testInputs[5];
    const result = try part1(input);
    try std.testing.expectEqual(expected, result);
}

test "day 04 part 1" {
    const gpa = std.testing.allocator;
    const input = try aoc.readInput("04", gpa);
    defer gpa.free(input);
    const result = try part1(input);
    try std.testing.expectEqual(@as(u32, 2517), result);
}

test "day 04 part 2 sample 2" {
    const input, _, const expected = testInputs[0];
    const result = try part2(input);
    try std.testing.expectEqual(expected, result);
}

test "day 04 part 2" {
    const gpa = std.testing.allocator;
    const input = try aoc.readInput("04", gpa);
    defer gpa.free(input);
    const result = try part2(input);
    try std.testing.expectEqual(@as(u32, 1960), result);
}
