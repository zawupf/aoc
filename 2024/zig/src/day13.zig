const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

fn Pair(comptime T: type) type {
    return struct { T, T };
}
const Numbers = Pair(u64);
const SignedNumbers = Pair(i64);

const Game = struct {
    btn_a: Numbers,
    btn_b: Numbers,
    prize: Numbers,

    pub fn init(input: []const u8) !Game {
        var lines = aoc.splitLines(input);
        return .{
            .btn_a = try numbers(lines.next().?),
            .btn_b = try numbers(lines.next().?),
            .prize = try numbers(lines.next().?),
        };
    }

    fn solve(self: Game, comptime offset: u64) u64 {
        const a_dx, const a_dy = toSigned(self.btn_a);
        const b_dx, const b_dy = toSigned(self.btn_b);
        const px, const py = toSigned(pos: {
            const x, const y = self.prize;
            break :pos .{ x + offset, y + offset };
        });
        const det = a_dx * b_dy - a_dy * b_dx;
        const a = @divFloor(px * b_dy - py * b_dx, det);
        const b = @divFloor(a_dx * py - a_dy * px, det);
        return if (a_dx * a + b_dx * b == px and a_dy * a + b_dy * b == py) @intCast(a * 3 + b) else 0;
    }

    fn numbers(line: []const u8) !Numbers {
        var iter = aoc.splitAny(line, "+,=");
        _ = iter.next();
        const x = try std.fmt.parseUnsigned(u64, iter.next().?, 10);
        _ = iter.next();
        const y = try std.fmt.parseUnsigned(u64, iter.next().?, 10);
        return .{ x, y };
    }

    fn toSigned(values: Numbers) SignedNumbers {
        return .{ @intCast(values.@"0"), @intCast(values.@"1") };
    }

    fn fromSigned(values: SignedNumbers) Numbers {
        return .{ @intCast(values.@"0"), @intCast(values.@"1") };
    }
};

fn solve(input: []const u8, comptime offset: u64) !u64 {
    var total: u64 = 0;
    var games = aoc.splitChunks(input);
    while (games.next()) |game_input| {
        const game: Game = try .init(game_input);
        total += game.solve(offset);
    }
    return total;
}

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    _ = gpa;
    return try solve(input, 0);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    _ = gpa;
    return try solve(input, 10000000000000);
}

pub const Day = aoc.DayInfo("13", u64, u64, 35729, 88584689879723, @This(), &.{.{
    .expected1 = 480,
    .expected2 = null,
    .input =
    \\Button A: X+94, Y+34
    \\Button B: X+22, Y+67
    \\Prize: X=8400, Y=5400
    \\
    \\Button A: X+26, Y+66
    \\Button B: X+67, Y+21
    \\Prize: X=12748, Y=12176
    \\
    \\Button A: X+17, Y+86
    \\Button B: X+84, Y+37
    \\Prize: X=7870, Y=6450
    \\
    \\Button A: X+69, Y+23
    \\Button B: X+27, Y+71
    \\Prize: X=18641, Y=10279
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
