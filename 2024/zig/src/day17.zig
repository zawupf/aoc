const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Opcode = enum(u3) {
    adv,
    bxl,
    bst,
    jnz,
    bxc,
    out,
    bdv,
    cdv,

    pub fn operand(self: Opcode, value: u3) Operand {
        return switch (self) {
            .adv, .bdv, .cdv, .bst, .out => .{ .combo = value },
            .bxl, .bxc, .jnz => .{ .literal = value },
        };
    }
};

const Operand = union(enum) {
    literal: u3,
    combo: u3,

    pub fn value(self: Operand, registers: Registers) u64 {
        return switch (self) {
            .literal => |v| v,
            .combo => |v| switch (v) {
                0...3 => v,
                4 => registers.a,
                5 => registers.b,
                6 => registers.c,
                7 => unreachable,
            },
        };
    }
};

const Registers = struct { a: u64, b: u64, c: u64 };

const BufferSize = 16;
const Computer = struct {
    programBuffer: [BufferSize]u3 = undefined,
    programLen: usize = 0,

    outputBuffer: [BufferSize]u3 = undefined,
    outputLen: usize = 0,

    pc: usize = 0,
    registers: Registers = .{ .a = 0, .b = 0, .c = 0 },

    pub fn init(input: []const u8) !Computer {
        var computer = Computer{};

        var lines = aoc.splitLines(input);
        computer.registers.a = try std.fmt.parseUnsigned(u64, lines.next().?[12..], 10);
        computer.registers.b = try std.fmt.parseUnsigned(u64, lines.next().?[12..], 10);
        computer.registers.c = try std.fmt.parseUnsigned(u64, lines.next().?[12..], 10);
        _ = lines.next(); // skip blank line

        var buf: std.ArrayList(u3) = .initBuffer(&computer.programBuffer);
        var codes = aoc.split(lines.next().?[9..], ',');
        while (codes.next()) |codeChar| {
            const code = try std.fmt.parseUnsigned(u3, codeChar, 10);
            try buf.appendBounded(code);
        }

        computer.programLen = buf.items.len;
        if (computer.programLen % 2 != 0) {
            return error.InvalidProgramLength;
        }

        return computer;
    }

    pub fn program(self: *const Computer) []const u3 {
        return self.programBuffer[0..self.programLen];
    }

    pub fn output(self: *const Computer) []const u3 {
        return self.outputBuffer[0..self.outputLen];
    }

    pub fn writeOutput(self: *Computer, value: u3) !void {
        if (self.outputLen >= BufferSize) {
            return error.OutputBufferFull;
        }
        self.outputBuffer[self.outputLen] = value;
        self.outputLen += 1;
    }

    pub fn isHalted(self: *const Computer) bool {
        return self.pc >= self.programLen;
    }

    pub fn step(self: *Computer) !void {
        if (self.isHalted()) return error.Halted;

        const opcode: Opcode = @enumFromInt(self.programBuffer[self.pc]);
        const operand: Operand = opcode.operand(self.programBuffer[self.pc + 1]);
        const value = operand.value(self.registers);
        // std.debug.print("PC: {d}, Opcode: {any}, Operand: {any}, Value: {d}\n", .{ self.pc, opcode, operand, value });

        self.pc += 2;
        switch (opcode) {
            .adv => self.registers.a >>= @intCast(value),
            .bxl => self.registers.b ^= value,
            .bst => self.registers.b = value & 0b111,
            .jnz => self.pc = if (self.registers.a != 0) value else self.pc,
            .bxc => self.registers.b ^= self.registers.c,
            .out => try self.writeOutput(@intCast(value & 0b111)),
            .bdv => self.registers.b = self.registers.a >> @intCast(value),
            .cdv => self.registers.c = self.registers.a >> @intCast(value),
        }
    }

    pub fn nextOutput(self: *Computer) !u3 {
        while (self.outputLen == 0) {
            try self.step();
        }
        self.outputLen -= 1;
        return self.outputBuffer[self.outputLen];
    }

    pub fn run(self: *Computer) !void {
        while (!self.isHalted()) {
            try self.step();
        }
    }
};

var outputBuffer: [256]u8 = undefined;

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    _ = gpa;

    var computer: Computer = try .init(input);
    try computer.run();

    var writer = std.Io.Writer.fixed(&outputBuffer);
    for (computer.output()) |v| try writer.print("{d},", .{v});
    writer.undo(1);

    return writer.buffered();
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    const c_: Computer = try .init(input);
    const len_ = c_.programLen;

    const State = struct { usize, u64 };
    var stack: std.ArrayList(State) = .empty;
    defer stack.deinit(gpa);
    try stack.append(gpa, .{ 0, 0 });

    var c = c_;
    var result: u64 = 0;
    while (stack.pop()) |state| {
        var len, result = state;
        if (len == len_) break;

        len += 1;
        const a_ = result << 3;
        const expectedOutput = c.programBuffer[len_ - len];
        for (0..8) |j| {
            const code = 7 - j;
            const a = a_ + code;
            c.pc = c_.pc;
            c.registers.a = a;
            if (try c.nextOutput() == expectedOutput) {
                try stack.append(gpa, .{ len, a });
            }
        }
    }
    return result;
}

pub const Day = aoc.DayInfo("17", []const u8, u64, "4,1,5,3,1,5,3,5,7", 164542125272765, @This(), &.{
    .{
        .expected1 = "4,6,3,5,6,3,5,2,1,0",
        .expected2 = null,
        .input =
        \\Register A: 729
        \\Register B: 0
        \\Register C: 0
        \\
        \\Program: 0,1,5,4,3,0
        ,
    },
    .{
        .expected1 = null,
        .expected2 = 117440,
        .input =
        \\Register A: 2024
        \\Register B: 0
        \\Register C: 0
        \\
        \\Program: 0,3,5,4,3,0
        ,
    },
});

test {
    const program = [_]u3{ 2, 6 };
    var c: Computer = .{
        .programBuffer = program ++ .{0} ** (BufferSize - program.len),
        .programLen = program.len,
        .pc = 0,
        .registers = .{ .a = 0, .b = 0, .c = 9 },
    };
    try c.run();
    try std.testing.expectEqual(1, c.registers.b);
}

test {
    const program = [_]u3{ 1, 7 };
    var c: Computer = .{
        .programBuffer = program ++ .{0} ** (BufferSize - program.len),
        .programLen = program.len,
        .pc = 0,
        .registers = .{ .a = 0, .b = 29, .c = 0 },
    };
    try c.run();
    try std.testing.expectEqual(26, c.registers.b);
}

test {
    const program = [_]u3{ 4, 0 };
    var c: Computer = .{
        .programBuffer = program ++ .{0} ** (BufferSize - program.len),
        .programLen = program.len,
        .pc = 0,
        .registers = .{ .a = 0, .b = 2024, .c = 43690 },
    };
    try c.run();
    try std.testing.expectEqual(44354, c.registers.b);
}

test {
    const program = [_]u3{ 5, 0, 5, 1, 5, 4 };
    var c: Computer = .{
        .programBuffer = program ++ .{0} ** (BufferSize - program.len),
        .programLen = program.len,
        .pc = 0,
        .registers = .{ .a = 10, .b = 0, .c = 0 },
    };
    try c.run();
    try std.testing.expectEqualSlices(u3, &.{ 0, 1, 2 }, c.output());
}

test {
    const program = [_]u3{ 0, 1, 5, 4, 3, 0 };
    var c: Computer = .{
        .programBuffer = program ++ .{0} ** (BufferSize - program.len),
        .programLen = program.len,
        .pc = 0,
        .registers = .{ .a = 2024, .b = 0, .c = 0 },
    };
    try c.run();
    try std.testing.expectEqual(0, c.registers.a);
    try std.testing.expectEqualSlices(u3, &.{ 4, 2, 5, 6, 7, 7, 7, 7, 3, 1, 0 }, c.output());
}

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
