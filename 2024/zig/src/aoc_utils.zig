const std = @import("std");
const Allocator = std.mem.Allocator;

pub fn inputPath(comptime day: *const [2:0]u8) []const u8 {
    return "../_inputs/Day" ++ day ++ ".txt";
}

pub fn readInput(comptime day: *const [2:0]u8, gpa: Allocator) ![]u8 {
    return std.fs.cwd().readFileAlloc(inputPath(day), gpa, .unlimited);
}

pub fn splitChunks(input: []const u8) std.mem.SplitIterator(u8, .sequence) {
    const input_ = std.mem.trim(u8, input, "\n");
    return std.mem.splitSequence(u8, input_, "\n\n");
}

pub fn splitLines(input: []const u8) std.mem.SplitIterator(u8, .scalar) {
    const input_ = std.mem.trim(u8, input, "\n");
    return std.mem.splitScalar(u8, input_, '\n');
}

pub fn split(input: []const u8, delimiter: u8) std.mem.SplitIterator(u8, .scalar) {
    return std.mem.splitScalar(u8, input, delimiter);
}

pub fn splitAny(input: []const u8, delimiters: []const u8) std.mem.SplitIterator(u8, .any) {
    return std.mem.splitAny(u8, input, delimiters);
}

pub fn tokenize(input: []const u8, delimiter: u8) std.mem.TokenIterator(u8, .scalar) {
    return std.mem.tokenizeScalar(u8, input, delimiter);
}

pub fn tokenizeAny(input: []const u8, delimiters: []const u8) std.mem.TokenIterator(u8, .any) {
    return std.mem.tokenizeAny(u8, input, delimiters);
}

pub fn trim(input: []const u8) []const u8 {
    return std.mem.trim(u8, input, &std.ascii.whitespace);
}

pub fn parseInts(comptime T: type, iter: anytype, buf: []T) ![]T {
    var array = std.ArrayList(T).initBuffer(buf);
    while (iter.next()) |text| {
        const value = try std.fmt.parseInt(T, text, 10);
        try array.appendBounded(value);
    }
    return array.items;
}

pub const Pos = struct { x: usize, y: usize };

pub const TurnDir = enum { clockwise, counter_clockwise };

pub const Orientation = enum {
    up,
    right,
    down,
    left,

    pub fn next(self: @This(), p: Pt2(usize)) Pt2(usize) {
        return self.step(1, p);
    }

    pub fn step(self: @This(), comptime offset: usize, p: Pt2(usize)) Pt2(usize) {
        return switch (self) {
            .up => .{ .x = p.x, .y = p.y - offset },
            .right => .{ .x = p.x + offset, .y = p.y },
            .down => .{ .x = p.x, .y = p.y + offset },
            .left => .{ .x = p.x - offset, .y = p.y },
        };
    }

    pub fn turn(self: @This(), dir: TurnDir) @This() {
        return switch (dir) {
            .clockwise => switch (self) {
                .up => .right,
                .right => .down,
                .down => .left,
                .left => .up,
            },
            .counter_clockwise => switch (self) {
                .up => .left,
                .left => .down,
                .down => .right,
                .right => .up,
            },
        };
    }
};

pub const Direction = enum {
    north,
    east,
    south,
    west,
    north_east,
    south_east,
    south_west,
    north_west,

    pub fn next(self: @This(), p: Pt2(usize)) Pt2(usize) {
        return self.step(1, p);
    }

    pub fn step(self: @This(), comptime offset: usize, p: Pt2(usize)) Pt2(usize) {
        return switch (self) {
            .north => .{ .x = p.x, .y = p.y - offset },
            .east => .{ .x = p.x + offset, .y = p.y },
            .south => .{ .x = p.x, .y = p.y + offset },
            .west => .{ .x = p.x - offset, .y = p.y },
            .north_east => .{ .x = p.x + offset, .y = p.y - offset },
            .south_east => .{ .x = p.x + offset, .y = p.y + offset },
            .south_west => .{ .x = p.x - offset, .y = p.y + offset },
            .north_west => .{ .x = p.x - offset, .y = p.y - offset },
        };
    }

    pub fn turn(self: @This(), dir: TurnDir) @This() {
        return switch (dir) {
            .clockwise => switch (self) {
                .north => .north_east,
                .north_east => .east,
                .east => .south_east,
                .south_east => .south,
                .south => .south_west,
                .south_west => .west,
                .west => .north_west,
                .north_west => .north,
            },
            .counter_clockwise => switch (self) {
                .north => .north_west,
                .north_west => .west,
                .west => .south_west,
                .south_west => .south,
                .south => .south_east,
                .south_east => .east,
                .east => .north_east,
                .north_east => .north,
            },
        };
    }
};

pub fn Grid(T: type, P: type) type {
    return struct {
        buf: []T,
        width: usize, // columns (not including the trailing '\n')
        height: usize, // number of rows

        pub const Pos = Pt2(P);

        pub fn inBound(self: @This(), p: Pos) bool {
            return p.x >= 0 and p.x < self.width and p.y >= 0 and p.y < self.height;
        }

        pub fn at(self: @This(), p: Pos) T {
            return self.buf[p.y * (self.width + 1) + p.x];
        }

        pub fn setAt(self: *@This(), p: Pos, value: T) void {
            self.buf[p.y * (self.width + 1) + p.x] = value;
        }

        pub fn row(self: @This(), y: usize) [:'\n']const T {
            const off = y * (self.width + 1);
            return self.buf[off .. off + self.width :'\n'];
        }

        pub fn findScalar(self: @This(), value: T) ?Pos {
            const idx = std.mem.findScalar(T, self.buf, value) orelse return null;
            return self.indexToPos(idx);
        }

        pub fn indexToPos(self: @This(), index: usize) Pos {
            const stride = self.width + 1;
            return .{ .x = @intCast(index % stride), .y = @intCast(index / stride) };
        }

        pub fn posToIndex(self: @This(), p: Pos) usize {
            const stride: P = @intCast(self.width + 1);
            return @intCast(p.y * stride + p.x);
        }

        pub fn subarray(self: @This(), comptime len: usize, pStart: Pos, comptime dir: Direction, comptime offset: usize) ?[len]T {
            if (len == 0) return null;

            // check ranges
            const len_ = len + offset;
            const sx = pStart.x;
            const sy = pStart.y;
            switch (dir) {
                .north, .north_east, .north_west => if (sy >= self.height or sy < len_ - 1) return null,
                .south, .south_east, .south_west => if (sy + len_ > self.height) return null,
                else => {},
            }
            switch (dir) {
                .west, .north_west, .south_west => if (sx >= self.width or sx < len_ - 1) return null,
                .east, .north_east, .south_east => if (sx + len_ > self.width) return null,
                else => {},
            }

            var i: usize, var p: Pos = .{ 0, pStart };
            p = dir.step(offset, p);

            var buffer: [len]T = undefined;
            while (true) {
                buffer[i] = self.at(p);

                i += 1;
                if (i == len) break;

                p = dir.next(p);
            }
            return buffer;
        }

        pub fn init(input: []u8) @This() {
            const first_nl = std.mem.findScalar(u8, input, '\n') orelse @panic("no newline found in input");
            const width = first_nl;
            const stride = width + 1;
            if (input.len % stride != 0 and (input.len + 1) % stride != 0) @panic("input length is not a multiple of row stride (W+1)");
            const height = input.len / stride + if (input.len % stride == 0) @as(usize, 0) else @as(usize, 1);

            const builtin = @import("builtin");
            if (builtin.mode == .Debug or builtin.mode == .ReleaseSafe) {
                var i: usize = stride - 1;
                while (i < input.len) : (i += stride) {
                    if (input[i] != '\n') @panic("line not terminated by newline");
                }
            }

            return .{ .buf = @ptrCast(input), .width = @intCast(width), .height = @intCast(height) };
        }
    };
}

pub fn DayInfo(
    comptime day_: *const [2:0]u8,
    comptime T1: type,
    comptime T2: type,
    comptime solution1_: ?T1,
    comptime solution2_: ?T2,
    comptime testData: []const struct { input: []const u8, expected1: ?T1, expected2: ?T2 },
) type {
    return struct {
        pub const day = day_;
        pub const Result1 = T1;
        pub const Result2 = T2;
        pub const solution1 = solution1_;
        pub const solution2 = solution2_;
        pub const tests = testData;

        pub fn testPart1Samples(part1: fn ([]const u8, Allocator) anyerror!T1) !void {
            const gpa = std.testing.allocator;
            for (@This().tests, 1..) |tst, index| {
                if (tst.expected1 == null) continue;

                const result = try part1(tst.input, gpa);
                _ = index;
                // std.debug.print("Test day {s}, part 1, sample {}: {}\n", .{ @This().day, index, result });
                try std.testing.expectEqual(tst.expected1, result);
            }
        }

        pub fn testPart2Samples(part2: fn ([]const u8, Allocator) anyerror!T2) !void {
            const gpa = std.testing.allocator;
            for (@This().tests, 1..) |tst, index| {
                if (tst.expected2 == null) continue;

                const result = try part2(tst.input, gpa);
                _ = index;
                // std.debug.print("Test day {s}, part 2, sample {}: {}\n", .{ @This().day, index, result });
                try std.testing.expectEqual(tst.expected2, result);
            }
        }

        pub fn testPart1(part1: fn ([]const u8, Allocator) anyerror!T1) !void {
            if (@This().solution1 == null) return;
            const gpa = std.testing.allocator;
            const input = try readInput(@This().day, gpa);
            defer gpa.free(input);
            const result = try part1(input, gpa);
            // std.debug.print("Day {s}, part 1: {}\n", .{ @This().day, result });
            try std.testing.expectEqual(@This().solution1, result);
        }

        pub fn testPart2(part2: fn ([]const u8, Allocator) anyerror!T2) !void {
            if (@This().solution2 == null) return;
            const gpa = std.testing.allocator;
            const input = try readInput(@This().day, gpa);
            defer gpa.free(input);
            const result = try part2(input, gpa);
            // std.debug.print("Day {s}, part 2: {}\n", .{ @This().day, result });
            try std.testing.expectEqual(@This().solution2, result);
        }
    };
}
