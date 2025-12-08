const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    var rowTokens = std.ArrayList(std.mem.TokenIterator(u8, .scalar)).empty;
    defer rowTokens.deinit(gpa);

    var lines = aoc.splitLines(input);
    while (lines.next()) |line| {
        try rowTokens.append(gpa, aoc.tokenize(line, ' '));
    }

    const numberRows = rowTokens.items[0 .. rowTokens.items.len - 1];

    var result: u64 = 0;
    var operators = rowTokens.items[rowTokens.items.len - 1];
    while (operators.next()) |op| {
        const o = op[0];
        std.debug.assert(o == '*' or o == '+');
        var acc: u64 = if (o == '*') 1 else 0;
        for (numberRows) |*row| {
            const value = try std.fmt.parseUnsigned(u64, row.next().?, 10);
            if (o == '*') {
                acc *= value;
            } else {
                acc += value;
            }
        }
        result += acc;
    }

    return result;
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    std.debug.assert(input[input.len - 1] == '\n');
    const height = std.mem.count(u8, input, "\n");
    std.debug.assert(input.len % height == 0);
    const width = input.len / height;

    const digits = input[0 .. input.len - width];

    const rowBuffer = try gpa.alloc(u8, height - 1);
    defer gpa.free(rowBuffer);

    var col: usize = 0;
    var result: u64 = 0;
    var operators = aoc.tokenize(input[input.len - width .. input.len - 1], ' ');
    while (operators.next()) |op| {
        const o = op[0];
        std.debug.assert(o == '*' or o == '+');

        var acc: u64 = if (o == '*') 1 else 0;
        while (true) {
            for (rowBuffer, 0..) |*c, i| c.* = digits[i * width + col];
            col += 1;

            const row = aoc.trim(rowBuffer);
            if (row.len == 0) {
                result += acc;
                break;
            }

            const value = try std.fmt.parseUnsigned(u64, row, 10);
            if (o == '*') {
                acc *= value;
            } else {
                acc += value;
            }
        }
    }

    return result;
}

pub const Day = aoc.DayInfo("06", u64, u64, 5171061464548, 10189959087258, @This(), &.{.{
    .expected1 = 4277556,
    .expected2 = 3263827,
    .input = "123 328  51 64 \n 45 64  387 23 \n  6 98  215 314\n*   +   *   +  \n",
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
