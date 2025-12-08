const std = @import("std");
const Allocator = std.mem.Allocator;

const day01 = @import("day01");
const day02 = @import("day02");
const day03 = @import("day03");
const day04 = @import("day04");
const day05 = @import("day05");

// const day06 = @import("day06");
// const day07 = @import("day07");
// const day08 = @import("day08");
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

fn run(day: type, gpa: Allocator) !void {
    // std.debug.print("\n", .{});
    try day.Day.runPart1(gpa);
    try day.Day.runPart2(gpa);
}

pub fn main() !void {
    var gpa = std.heap.GeneralPurposeAllocator(.{}){};
    const allocator = gpa.allocator();
    defer {
        const deinit_status = gpa.deinit();
        if (deinit_status == .leak) @panic("Memory leak detected");
    }

    var timer = try std.time.Timer.start();

    try run(day01, allocator);
    try run(day02, allocator);
    try run(day03, allocator);
    try run(day04, allocator);
    try run(day05, allocator);
    // try run(day06, allocator);
    // try run(day07, allocator);
    // try run(day08, allocator);
    // try run(day09, allocator);
    // try run(day10, allocator);
    // try run(day11, allocator);
    // try run(day12, allocator);
    // try run(day13, allocator);
    // try run(day14, allocator);
    // try run(day15, allocator);
    // try run(day16, allocator);
    // try run(day17, allocator);
    // try run(day18, allocator);
    // try run(day19, allocator);
    // try run(day20, allocator);
    // try run(day21, allocator);
    // try run(day22, allocator);
    // try run(day23, allocator);
    // try run(day24, allocator);
    // try run(day25, allocator);

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
