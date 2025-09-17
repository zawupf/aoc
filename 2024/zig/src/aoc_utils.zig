const std = @import("std");

pub fn readInput(comptime day: *const [2:0]u8, gpa: std.mem.Allocator) ![]u8 {
    const path: []const u8 = "../_inputs/Day" ++ day ++ ".txt";
    return std.fs.cwd().readFileAlloc(path, gpa, .unlimited);
}
