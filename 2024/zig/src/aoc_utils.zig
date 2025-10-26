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

pub fn Pt2(T: type) type {
    return struct {
        x: T,
        y: T,

        const Self = @This();

        pub fn add(self: Self, other: Self) Self {
            return .{ .x = self.x + other.x, .y = self.y + other.y };
        }

        pub fn sub(self: Self, other: Self) Self {
            return .{ .x = self.x - other.x, .y = self.y - other.y };
        }

        pub fn incBy(self: *Self, other: Self) void {
            self.x += other.x;
            self.y += other.y;
        }

        pub fn decBy(self: *Self, other: Self) void {
            self.x -= other.x;
            self.y -= other.y;
        }

        pub fn manhattan(self: Self, other: Self) T {
            const dx = if (self.x > other.x) self.x - other.x else other.x - self.x;
            const dy = if (self.y > other.y) self.y - other.y else other.y - self.y;
            return dx + dy;
        }
    };
}

pub const TurnDir = enum { clockwise, counter_clockwise };

pub const Orientation = enum {
    up,
    right,
    down,
    left,

    const Self = @This();

    pub fn next(self: Self, p: anytype) @TypeOf(p) {
        return self.step(1, p);
    }

    pub fn nextOrNull(self: Self, p: Pt2(usize), grid: anytype) ?@TypeOf(grid).Pos {
        return self.stepOrNull(1, p, grid);
    }

    pub fn step(self: Self, comptime offset: usize, p: anytype) @TypeOf(p) {
        return switch (self) {
            .up => .{ .x = p.x, .y = p.y - offset },
            .right => .{ .x = p.x + offset, .y = p.y },
            .down => .{ .x = p.x, .y = p.y + offset },
            .left => .{ .x = p.x - offset, .y = p.y },
        };
    }

    pub fn stepOrNull(self: Self, comptime offset: usize, p: Pt2(usize), grid: anytype) ?@TypeOf(grid).Pos {
        return switch (self) {
            .up => if (p.y >= offset) .{ .x = p.x, .y = p.y - offset } else null,
            .right => if (p.x + offset < grid.width) .{ .x = p.x + offset, .y = p.y } else null,
            .down => if (p.y + offset < grid.height) .{ .x = p.x, .y = p.y + offset } else null,
            .left => if (p.x >= offset) .{ .x = p.x - offset, .y = p.y } else null,
        };
    }

    pub fn turn(self: Self, dir: TurnDir) Self {
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

    const Self = @This();

    pub fn next(self: Self, p: Pt2(usize)) Pt2(usize) {
        return self.step(1, p);
    }

    pub fn nextOrNull(self: Self, p: Pt2(usize), grid: anytype) ?Pt2(usize) {
        return self.stepOrNull(1, p, grid);
    }

    pub fn step(self: Self, comptime offset: usize, p: Pt2(usize)) Pt2(usize) {
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

    pub fn stepOrNull(self: Self, comptime offset: usize, p: Pt2(usize), grid: anytype) ?Pt2(usize) {
        return switch (self) {
            .north => if (p.y >= offset) .{ .x = p.x, .y = p.y - offset } else null,
            .east => if (p.x + offset < grid.width) .{ .x = p.x + offset, .y = p.y } else null,
            .south => if (p.y + offset < grid.height) .{ .x = p.x, .y = p.y + offset } else null,
            .west => if (p.x >= offset) .{ .x = p.x - offset, .y = p.y } else null,
            .north_east => if (p.y >= offset and p.x + offset < grid.width) .{ .x = p.x + offset, .y = p.y - offset } else null,
            .south_east => if (p.y + offset < grid.height and p.x + offset < grid.width) .{ .x = p.x + offset, .y = p.y + offset } else null,
            .south_west => if (p.y + offset < grid.height and p.x >= offset) .{ .x = p.x - offset, .y = p.y + offset } else null,
            .north_west => if (p.y >= offset and p.x >= offset) .{ .x = p.x - offset, .y = p.y - offset } else null,
        };
    }

    pub fn turn(self: Self, dir: TurnDir) Self {
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
        width: P, // columns (not including the trailing '\n')
        height: P, // number of rows

        const Self = @This();
        pub const Pos = Pt2(P);

        pub fn inBound(self: Self, p: Pos) bool {
            return p.x >= 0 and p.x < self.width and p.y >= 0 and p.y < self.height;
        }

        pub fn ptr(self: *Self, p: Pos) *T {
            const x: usize, const y: usize = .{ p.x, p.y };
            return &self.buf[y * self.width + x];
        }

        pub fn at(self: Self, p: Pos) T {
            const x: usize, const y: usize = .{ p.x, p.y };
            return self.buf[y * self.width + x];
        }

        pub fn setAt(self: *Self, p: Pos, value: T) void {
            const x: usize, const y: usize = .{ p.x, p.y };
            self.buf[y * self.width + x] = value;
        }

        pub fn row(self: Self, y: P) []const T {
            const offset = y * self.width;
            return self.buf[offset .. offset + self.width];
        }

        pub fn findScalar(self: Self, value: T) ?Pos {
            const idx = std.mem.findScalar(T, self.buf, value) orelse return null;
            return self.indexToPos(idx);
        }

        pub fn indexToPos(self: Self, index: usize) Pos {
            const width: usize = @intCast(self.width);
            return .{
                .x = @intCast(index % width),
                .y = @intCast(index / width),
            };
        }

        pub fn posToIndex(self: Self, p: Pos) usize {
            const x: usize, const y: usize = .{ p.x, p.y };
            return y * self.width + x;
        }

        pub fn subarray(
            self: Self,
            comptime len: usize,
            pStart: Pos,
            comptime dir: Direction,
            comptime offset: usize,
        ) ?[len]T {
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

        pub fn init(input: []const u8, gpa: Allocator) !Self {
            return try Self._init(input, null, gpa);
        }

        pub fn initMapped(input: []const u8, comptime mapping: fn (u8) T, gpa: Allocator) !Self {
            return try Self._init(input, mapping, gpa);
        }

        pub fn _init(input: []const u8, comptime mapping: ?fn (u8) T, gpa: Allocator) !Self {
            const width = std.mem.findScalar(u8, input, '\n') orelse input.len;
            const len = input.len - std.mem.count(u8, input, "\n");
            const height = len / width;
            std.debug.assert(len % width == 0);

            var buf = try gpa.alloc(T, len);
            errdefer gpa.free(buf);

            var i: usize = 0;
            if (mapping) |fun| {
                for (input) |value| {
                    if (value == '\n') continue;
                    buf[i] = fun(value);
                    i += 1;
                }
            } else {
                var iter = std.mem.tokenizeScalar(u8, input, '\n');
                while (iter.next()) |line| : (i += width) {
                    @memcpy(buf[i .. i + width], @as([]const T, @ptrCast(line)));
                }
            }

            return .{ .buf = buf, .width = @intCast(width), .height = @intCast(height) };
        }

        pub fn deinit(self: *const Self, gpa: Allocator) void {
            gpa.free(self.buf);
        }
    };
}

pub fn DayInfo(
    comptime day_: *const [2:0]u8,
    comptime T1: type,
    comptime T2: type,
    comptime solution1_: ?T1,
    comptime solution2_: ?T2,
    comptime module_: type,
    comptime testData: []const struct { input: []const u8, expected1: ?T1, expected2: ?T2 },
) type {
    return struct {
        pub const day = day_;
        pub const Result1 = T1;
        pub const Result2 = T2;
        pub const solution1 = solution1_;
        pub const solution2 = solution2_;
        pub const tests = testData;
        pub const module = module_;

        pub fn testPart1Samples() !void {
            try testSamples(0);
        }

        pub fn testPart2Samples() !void {
            try testSamples(1);
        }

        fn testSamples(comptime partIdx: u1) !void {
            const gpa = std.testing.allocator;
            const func = if (partIdx == 0) module.part1 else module.part2;
            for (tests, 1..) |tst, index| {
                const expected = if (partIdx == 0) tst.expected1 else tst.expected2;
                if (expected == null) continue;
                const result = try func(tst.input, gpa);
                _ = index;
                // std.debug.print("Test day {s}, part {d}, sample {}: {}\n", .{ day, (@as(u8, partIdx)) + 1, index, result });
                try std.testing.expectEqual(expected, result);
            }
        }

        pub fn testPart1() !void {
            try testPart(0);
        }

        pub fn testPart2() !void {
            try testPart(1);
        }

        fn testPart(comptime partIdx: u1) !void {
            const solution = if (partIdx == 0) solution1 else solution2;
            if (solution == null) return;

            const gpa = std.testing.allocator;
            const input = try readInput(day, gpa);
            defer gpa.free(input);

            const func = if (partIdx == 0) module.part1 else module.part2;
            const result = try func(input, gpa);
            // std.debug.print("Day {s}, part {d}: {}\n", .{ day, (@as(u8, partIdx)) + 1, result });
            try std.testing.expectEqual(solution, result);
        }

        pub fn runPart1(gpa: Allocator) !void {
            try runPart(0, gpa);
        }

        pub fn runPart2(gpa: Allocator) !void {
            try runPart(1, gpa);
        }

        fn runPart(comptime partIdx: u1, gpa: Allocator) !void {
            const description = "Day {s} (part {d}): ";
            const partNum = (@as(u8, partIdx)) + 1;

            std.debug.print(description, .{ day, partNum });
            const func = if (partIdx == 0) module.part1 else module.part2;
            const solution = if (partIdx == 0) solution1 else solution2;

            if (solution == null) {
                std.debug.print("\r🚧 " ++ description ++ "Skipped\n", .{ day, partNum });
                return;
            }

            const input = try readInput(day, gpa);
            defer gpa.free(input);

            var timer = try std.time.Timer.start();
            const result = try func(input, gpa);

            var elapsed: f64, var unit: []const u8 = .{ @floatFromInt(timer.read()), "ns" };
            if (elapsed >= 1000.0) {
                elapsed /= 1000.0;
                unit = "µs";
            }
            if (elapsed >= 1000.0) {
                elapsed /= 1000.0;
                unit = "ms";
            }
            if (elapsed >= 1000.0) {
                elapsed /= 1000.0;
                unit = "s";
            }

            const prefix = if (result == solution) "✅" else "❌";
            std.debug.print(
                "\r{s} " ++ description ++ "{any}  [ {d:.1} {s} ]\n",
                .{ prefix, day, partNum, result, elapsed, unit },
            );
        }
    };
}
