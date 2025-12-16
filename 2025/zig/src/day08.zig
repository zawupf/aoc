const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    return try solve(.{ .part1 = 1000 }, input, gpa);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    return try solve(.part2, input, gpa);
}

const PartId = union(enum) { part1: u16, part2 };
fn Result(partId: PartId) type {
    return switch (partId) {
        .part1 => Day.Result1,
        .part2 => Day.Result2,
    };
}

const Pos = struct { x: u32, y: u32, z: u32 };
const Junction = struct { pos: Pos, circuitIdx: ?usize };
const DistanceInfo = struct {
    dist: u64,
    junctionIdx1: usize,
    junctionIdx2: usize,

    fn lessThan(_: void, a: DistanceInfo, b: DistanceInfo) bool {
        return a.dist < b.dist;
    }
};

fn solve(comptime partId: PartId, input: []const u8, gpa: Allocator) !Result(partId) {
    const junctions: []Junction = blk: {
        var lines = aoc.splitLines(aoc.trim(input));
        var junctions = std.ArrayList(Junction).empty;
        errdefer junctions.deinit(gpa);
        while (lines.next()) |line| {
            var values = aoc.split(line, ',');
            const x = try std.fmt.parseUnsigned(u32, values.next().?, 10);
            const y = try std.fmt.parseUnsigned(u32, values.next().?, 10);
            const z = try std.fmt.parseUnsigned(u32, values.next().?, 10);
            _ = try junctions.append(gpa, .{
                .pos = .{ .x = x, .y = y, .z = z },
                .circuitIdx = null,
            });
        }
        break :blk try junctions.toOwnedSlice(gpa);
    };
    defer gpa.free(junctions);

    const distances: []DistanceInfo = blk: {
        const n = junctions.len;
        var distances = try std.ArrayList(DistanceInfo).initCapacity(gpa, n * (n - 1) / 2);
        errdefer distances.deinit(gpa);
        for (junctions, 0..) |*j1, idx1| {
            const p1 = j1.pos;
            const idx2_start = idx1 + 1;
            for (junctions[idx2_start..], idx2_start..) |*j2, idx2| {
                const p2 = j2.pos;
                const dx: u64 = @abs(@as(i64, p1.x) - @as(i64, p2.x));
                const dy: u64 = @abs(@as(i64, p1.y) - @as(i64, p2.y));
                const dz: u64 = @abs(@as(i64, p1.z) - @as(i64, p2.z));
                const dist = dx * dx + dy * dy + dz * dz;
                distances.appendAssumeCapacity(.{
                    .dist = dist,
                    .junctionIdx1 = idx1,
                    .junctionIdx2 = idx2,
                });
            }
        }
        std.mem.sortUnstable(DistanceInfo, distances.items, {}, DistanceInfo.lessThan);
        break :blk try distances.toOwnedSlice(gpa);
    };
    defer gpa.free(distances);

    var circuits = std.ArrayList(std.AutoArrayHashMapUnmanaged(usize, void)).empty;
    defer {
        for (circuits.items) |*circuit| circuit.deinit(gpa);
        circuits.deinit(gpa);
    }

    var steps_left: u32 = switch (partId) {
        .part1 => |n| n,
        .part2 => 0,
    };

    var usedCircuitsCount: usize = 0;
    var usedCircuitIdx: usize = 0;
    var last_distance: ?DistanceInfo = null;

    for (distances) |d| {
        switch (partId) {
            .part1 => {
                if (steps_left == 0) break;
                steps_left -= 1;
            },
            .part2 => {},
        }

        try applyDistance(d, junctions, &circuits, gpa, &usedCircuitsCount, &usedCircuitIdx);
        last_distance = d;

        switch (partId) {
            .part1 => {},
            .part2 => {
                if (usedCircuitsCount == 1 and circuits.items[usedCircuitIdx].count() == junctions.len) break;
            },
        }
    }

    return switch (partId) {
        .part1 => blk: {
            var sizes = try std.ArrayList(usize).initCapacity(gpa, circuits.items.len);
            defer sizes.deinit(gpa);
            for (circuits.items) |*circuit| {
                sizes.appendAssumeCapacity(circuit.count());
            }
            std.mem.sortUnstable(usize, sizes.items, {}, std.sort.desc(usize));
            var result: usize = 1;
            for (sizes.items[0..3]) |size| result *= size;
            break :blk @intCast(result);
        },
        .part2 => blk: {
            const d = last_distance orelse unreachable;
            const x1: u64 = junctions[d.junctionIdx1].pos.x;
            const x2: u64 = junctions[d.junctionIdx2].pos.x;
            break :blk x1 * x2;
        },
    };
}

fn applyDistance(
    d: DistanceInfo,
    junctions: []Junction,
    circuits: *std.ArrayList(std.AutoArrayHashMapUnmanaged(usize, void)),
    gpa: Allocator,
    usedCircuitsCount: *usize,
    usedCircuitIdx: *usize,
) !void {
    const j1 = &junctions[d.junctionIdx1];
    const j2 = &junctions[d.junctionIdx2];

    if (j1.circuitIdx) |idx1| {
        if (j2.circuitIdx) |idx2| {
            if (idx1 == idx2) return;

            const circuit1 = &circuits.items[idx1];
            const circuit2 = &circuits.items[idx2];
            for (circuit2.keys()) |junc_idx| {
                junctions[junc_idx].circuitIdx = idx1;
                _ = try circuit1.put(gpa, junc_idx, {});
            }
            circuit2.clearAndFree(gpa);

            usedCircuitsCount.* -= 1;
            usedCircuitIdx.* = idx1;
            return;
        }

        j2.circuitIdx = idx1;
        _ = try circuits.items[idx1].put(gpa, d.junctionIdx2, {});
        usedCircuitIdx.* = idx1;
        return;
    }

    if (j2.circuitIdx) |idx2| {
        j1.circuitIdx = idx2;
        _ = try circuits.items[idx2].put(gpa, d.junctionIdx1, {});
        usedCircuitIdx.* = idx2;
        return;
    }

    var new_circuit = std.AutoArrayHashMapUnmanaged(usize, void).empty;
    errdefer new_circuit.deinit(gpa);
    _ = try new_circuit.put(gpa, d.junctionIdx1, {});
    _ = try new_circuit.put(gpa, d.junctionIdx2, {});

    const new_idx = circuits.items.len;
    j1.circuitIdx = new_idx;
    j2.circuitIdx = new_idx;

    usedCircuitsCount.* += 1;
    usedCircuitIdx.* = new_idx;
    try circuits.append(gpa, new_circuit);
}

pub const Day = aoc.DayInfo("08", u32, u64, 97384, 9003685096, @This(), &.{.{
    .expected1 = 40,
    .expected2 = 25272,
    .input =
    \\162,817,812
    \\57,618,57
    \\906,360,560
    \\592,479,940
    \\352,342,300
    \\466,668,158
    \\542,29,236
    \\431,825,988
    \\739,650,466
    \\52,470,668
    \\216,146,977
    \\819,987,18
    \\117,168,530
    \\805,96,715
    \\346,949,466
    \\970,615,88
    \\941,993,340
    \\862,61,35
    \\984,92,344
    \\425,690,689
    ,
}});

test "samples 1" {
    const data = Day.tests[0];
    try std.testing.expectEqual(
        data.expected1,
        try solve(.{ .part1 = 10 }, data.input, std.testing.allocator),
    );
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
