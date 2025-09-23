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

pub fn tokenize(input: []const u8, delimiter: u8) std.mem.TokenIterator(u8, .scalar) {
    return std.mem.tokenizeScalar(u8, input, delimiter);
}

pub fn trim(input: []const u8) []const u8 {
    return std.mem.trim(u8, input, &std.ascii.whitespace);
}

pub const Direction = enum {
    north,
    east,
    south,
    west,
    north_east,
    south_east,
    south_west,
    north_west,

    pub fn step(self: @This(), comptime offset: usize, r: usize, c: usize) struct { usize, usize } {
        return switch (self) {
            .north => .{ r - offset, c },
            .east => .{ r, c + offset },
            .south => .{ r + offset, c },
            .west => .{ r, c - offset },
            .north_east => .{ r - offset, c + offset },
            .south_east => .{ r + offset, c + offset },
            .south_west => .{ r + offset, c - offset },
            .north_west => .{ r - offset, c - offset },
        };
    }
};

pub const GridView = struct {
    buf: []const u8,
    width: usize, // columns (not including the trailing '\n')
    height: usize, // number of rows

    pub inline fn at(self: @This(), r: usize, c: usize) u8 {
        return self.buf[r * (self.width + 1) + c];
    }

    pub inline fn row(self: @This(), r: usize) [:'\n']const u8 {
        const off = r * (self.width + 1);
        return self.buf[off .. off + self.width :'\n'];
    }

    pub fn subarray(self: @This(), comptime len: usize, rStart: usize, cStart: usize, comptime dir: Direction, comptime offset: usize) ?[len]u8 {
        if (len == 0) return null;

        // check ranges
        const len_ = len + offset;
        switch (dir) {
            .north, .north_east, .north_west => if (rStart >= self.height or rStart < len_ - 1) return null,
            .south, .south_east, .south_west => if (rStart + len_ > self.height) return null,
            else => {},
        }
        switch (dir) {
            .west, .north_west, .south_west => if (cStart >= self.width or cStart < len_ - 1) return null,
            .east, .north_east, .south_east => if (cStart + len_ > self.width) return null,
            else => {},
        }

        var i: usize, var r: usize, var c: usize = .{ 0, rStart, cStart };
        r, c = dir.step(offset, r, c);

        var buffer: [len]u8 = undefined;
        while (true) {
            buffer[i] = self.at(r, c);

            i += 1;
            if (i == len) break;

            r, c = dir.step(1, r, c);
        }
        return buffer;
    }

    pub fn init(input: []const u8) GridView {
        const first_nl = std.mem.indexOfScalar(u8, input, '\n') orelse @panic("no newline found in input");
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

        return .{ .buf = input, .width = width, .height = height };
    }
};
