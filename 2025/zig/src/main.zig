const std = @import("std");
const Allocator = std.mem.Allocator;

const day01 = @import("day01");
const day02 = @import("day02");
const day03 = @import("day03");
const day04 = @import("day04");
const day05 = @import("day05");
const day06 = @import("day06");
const day07 = @import("day07");
const day08 = @import("day08");

// const day09 = @import("day09");
// const day10 = @import("day10");
// const day11 = @import("day11");
// const day12 = @import("day12");
// const day13 = @import("day13");
// const day14 = @import("day14");
// const day15 = @import("day15");
// const day16 = @import("day16");
// const day17 = @import("day17");
// const day18 = @import("day18");
// const day19 = @import("day19");
// const day20 = @import("day20");
// const day21 = @import("day21");
// const day22 = @import("day22");
// const day23 = @import("day23");
// const day24 = @import("day24");
// const day25 = @import("day25");

fn run(day: type, gpa: Allocator, io: std.Io) !void {
    // std.debug.print("\n", .{});
    try day.Day.runPart1(gpa, io);
    try day.Day.runPart2(gpa, io);
}

pub fn main() !void {
    var allocator = std.heap.GeneralPurposeAllocator(.{}){};
    const gpa = allocator.allocator();
    defer {
        const deinit_status = allocator.deinit();
        if (deinit_status == .leak) @panic("Memory leak detected");
    }

    var threadedIo: std.Io.Threaded = .init(gpa, .{ .environ = .empty });
    defer threadedIo.deinit();
    const io = threadedIo.io();

    var timer = try std.time.Timer.start();

    try run(day01, gpa, io);
    try run(day02, gpa, io);
    try run(day03, gpa, io);
    try run(day04, gpa, io);
    try run(day05, gpa, io);
    try run(day06, gpa, io);
    try run(day07, gpa, io);
    try run(day08, gpa, io);
    // try run(day09, gpa, io);
    // try run(day10, gpa, io);
    // try run(day11, gpa, io);
    // try run(day12, gpa, io);

    const elapsed_ms: u64 = @intCast(timer.read() / 1_000_000);
    std.debug.print("🏁 All days completed in {d} ms\n", .{elapsed_ms});
}

test "simple test" {
    const gpa = std.testing.allocator;
    var list: std.ArrayList(i32) = .empty;
    defer list.deinit(gpa); // Try commenting this out and see if zig detects the memory leak!
    try list.append(gpa, 42);
    try std.testing.expectEqual(@as(i32, 42), list.pop());
}

test "fuzz example" {
    const Context = struct {
        fn testOne(context: @This(), input: []const u8) anyerror!void {
            _ = context;
            // Try passing `--fuzz` to `zig build test` and see if it manages to fail this test case!
            try std.testing.expect(!std.mem.eql(u8, "canyoufindme", input));
        }
    };
    try std.testing.fuzz(Context{}, Context.testOne, .{});
}
