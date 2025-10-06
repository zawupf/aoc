const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Disk = struct {
    const Block = union(enum) {
        file: struct { id: u32, size: u4 },
        free: u4,
    };

    blocks: std.ArrayList(Block),

    pub fn init(input: []const u8, gpa: Allocator) !Disk {
        var blocks = try std.ArrayList(Block).initCapacity(gpa, input.len + input.len / 2);
        errdefer blocks.deinit(gpa);

        for (aoc.trim(input), 0..) |d, i| {
            const count = try std.fmt.charToDigit(d, 10);

            const isFile = i & 1 == 0;
            if (isFile) {
                std.debug.assert(count != 0);
                blocks.appendAssumeCapacity(.{ .file = .{
                    .id = @intCast(i >> 1),
                    .size = @intCast(count),
                } });
            } else if (count > 0) blocks.appendAssumeCapacity(.{
                .free = @intCast(count),
            });
        }

        return .{ .blocks = blocks };
    }

    pub fn deinit(self: *Disk, gpa: Allocator) void {
        self.blocks.deinit(gpa);
    }

    pub fn floodFillEmptyBlocks(self: *Disk) void {
        var e: usize = self.blocks.items.len - 1;
        var i: usize = self.findFreeBlock(0, e, null) orelse return;
        std.debug.assert(std.mem.eql(u8, @tagName(self.blocks.items[e]), "file"));

        while (i < e) {
            // self.dump();
            const available = self.blocks.items[i].free;
            const fileSize = self.blocks.items[e].file.size;
            if (fileSize == available) {
                self.blocks.items[i] = self.blocks.items[e];
                _ = self.blocks.pop();
                e = self.stripFreeBlocksAtEnd();
                i = self.findFreeBlock(i + 1, e, null) orelse break;
            } else if (fileSize < available) {
                self.blocks.items[i].free -= fileSize;
                self.blocks.insertAssumeCapacity(i, self.blocks.items[e]);
                _ = self.blocks.pop();
                e = self.stripFreeBlocksAtEnd();
                i += 1;
            } else {
                self.blocks.items[i] = .{ .file = .{
                    .id = self.blocks.items[e].file.id,
                    .size = available,
                } };
                self.blocks.items[e].file.size -= available;
                i = self.findFreeBlock(i + 1, e, null) orelse break;
            }
        }
        // self.dump();
    }

    pub fn moveWholeFiles(self: *Disk) void {
        var e: usize = self.blocks.items.len - 1;
        var i: usize = self.findFreeBlock(0, e, null) orelse return;
        std.debug.assert(std.mem.eql(u8, @tagName(self.blocks.items[e]), "file"));

        while (i < e) : (i = self.findFreeBlock(i, e, null) orelse break) {
            // self.dump();
            const fileSize = self.blocks.items[e].file.size;
            if (self.findFreeBlock(i, e, fileSize)) |j| {
                if (j < e) {
                    const available = self.blocks.items[j].free;
                    if (fileSize == available) {
                        self.blocks.items[j] = self.blocks.items[e];
                        self.blocks.items[e] = .{ .free = fileSize };
                    } else {
                        std.debug.assert(fileSize < available);
                        self.blocks.items[j].free -= fileSize;
                        self.blocks.insertAssumeCapacity(j, self.blocks.items[e]);
                        e += 1;
                        self.blocks.items[e] = .{ .free = fileSize };
                    }
                }
            }

            e -= 1;
            while (true) : (e -= 1) {
                switch (self.blocks.items[e]) {
                    .file => break,
                    .free => {},
                }
            }
        }
        // self.dump();
    }

    fn findFreeBlock(self: Disk, start: usize, end: usize, size: ?u4) ?usize {
        var i: usize = start;
        while (i < end) : (i += 1) switch (self.blocks.items[i]) {
            .free => |sz| if (size == null or sz >= size.?) return i,
            .file => {},
        };
        return null;
    }

    fn stripFreeBlocksAtEnd(self: *Disk) usize {
        loop: switch (self.blocks.getLast()) {
            .free => {
                _ = self.blocks.pop();
                continue :loop self.blocks.getLast();
            },
            .file => break :loop,
        }
        return self.blocks.items.len - 1;
    }

    pub fn checksum(self: Disk) u64 {
        var i: u64 = 0;
        var prevFactor: u64 = 0;
        var sum: u64 = 0;
        for (self.blocks.items) |block| switch (block) {
            .file => |file| {
                i += file.size;
                const n = i - 1;
                const factor = n * (n + 1) / 2;
                sum += (factor - prevFactor) * file.id;
                prevFactor = factor;
            },
            .free => |size| {
                i += size;
                const n = i - 1;
                prevFactor = n * (n + 1) / 2;
            },
        };
        return sum;
    }

    fn dump(self: Disk) void {
        for (self.blocks.items) |block| {
            switch (block) {
                .file => |file| {
                    var i: u4 = 0;
                    while (i < file.size) : (i += 1) std.debug.print("{d}", .{file.id});
                },
                .free => |size| {
                    var i: u4 = 0;
                    while (i < size) : (i += 1) std.debug.print(".", .{});
                },
            }
        }
        std.debug.print("\n", .{});
    }
};

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    var disk = try Disk.init(input, gpa);
    defer disk.deinit(gpa);
    disk.floodFillEmptyBlocks();
    return disk.checksum();
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    var disk = try Disk.init(input, gpa);
    defer disk.deinit(gpa);
    disk.moveWholeFiles();
    return disk.checksum();
}

pub const Day = aoc.DayInfo("09", u64, u64, 6607511583593, 6636608781232, @This(), &.{.{
    .expected1 = 1928,
    .expected2 = 2858,
    .input =
    \\2333133121414131402
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
