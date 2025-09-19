const std = @import("std");
const Allocator = std.mem.Allocator;

pub fn inputPath(comptime day: *const [2:0]u8) []const u8 {
    return "../_inputs/Day" ++ day ++ ".txt";
}

pub fn readInput(comptime day: *const [2:0]u8, gpa: Allocator) ![]u8 {
    return std.fs.cwd().readFileAlloc(inputPath(day), gpa, .unlimited);
}
