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
const DistanceInfo = struct {
    dist: u64,
    junctionIdx1: usize,
    junctionIdx2: usize,

    fn compare(_: void, a: DistanceInfo, b: DistanceInfo) std.math.Order {
        return std.math.order(a.dist, b.dist);
    }
};

const DistanceQueue = std.PriorityQueue(DistanceInfo, void, DistanceInfo.compare);

const UnionFind = struct {
    parent: []usize,
    rank: []u8,
    component_count: usize,

    fn init(gpa: Allocator, n: usize) !UnionFind {
        const parent = try gpa.alloc(usize, n);
        const rank = try gpa.alloc(u8, n);
        for (0..n) |i| {
            parent[i] = i;
            rank[i] = 0;
        }
        return .{ .parent = parent, .rank = rank, .component_count = n };
    }

    fn deinit(self: *UnionFind, gpa: Allocator) void {
        gpa.free(self.parent);
        gpa.free(self.rank);
    }

    fn find(self: *UnionFind, x: usize) usize {
        if (self.parent[x] != x) {
            self.parent[x] = self.find(self.parent[x]); // path compression
        }
        return self.parent[x];
    }

    /// Returns true if x and y were in different components (and are now merged)
    fn unite(self: *UnionFind, x: usize, y: usize) bool {
        const root_x = self.find(x);
        const root_y = self.find(y);
        if (root_x == root_y) return false;

        // union by rank
        if (self.rank[root_x] < self.rank[root_y]) {
            self.parent[root_x] = root_y;
        } else if (self.rank[root_x] > self.rank[root_y]) {
            self.parent[root_y] = root_x;
        } else {
            self.parent[root_y] = root_x;
            self.rank[root_x] += 1;
        }
        self.component_count -= 1;
        return true;
    }

    fn sizes(self: *UnionFind, gpa: Allocator) ![]usize {
        var counts = try gpa.alloc(usize, self.parent.len);
        @memset(counts, 0);
        for (0..counts.len) |i| {
            counts[self.find(i)] += 1;
        }
        return counts;
    }
};

fn solve(comptime partId: PartId, input: []const u8, gpa: Allocator) !Result(partId) {
    const positions: []Pos = blk: {
        var lines = aoc.splitLines(aoc.trim(input));
        var positions: std.ArrayList(Pos) = .empty;
        errdefer positions.deinit(gpa);
        while (lines.next()) |line| {
            var values = aoc.split(line, ',');
            const x = try std.fmt.parseUnsigned(u32, values.next().?, 10);
            const y = try std.fmt.parseUnsigned(u32, values.next().?, 10);
            const z = try std.fmt.parseUnsigned(u32, values.next().?, 10);
            _ = try positions.append(gpa, .{ .x = x, .y = y, .z = z });
        }
        break :blk try positions.toOwnedSlice(gpa);
    };
    defer gpa.free(positions);

    var distances: DistanceQueue = blk: {
        const n = positions.len;
        const items = try gpa.alloc(DistanceInfo, n * (n - 1) / 2);
        var i: usize = 0;
        for (positions, 0..) |p1, idx1| {
            const idx2_start = idx1 + 1;
            for (positions[idx2_start..], idx2_start..) |p2, idx2| {
                const dx: u64 = @abs(@as(i64, p1.x) - @as(i64, p2.x));
                const dy: u64 = @abs(@as(i64, p1.y) - @as(i64, p2.y));
                const dz: u64 = @abs(@as(i64, p1.z) - @as(i64, p2.z));
                const dist = dx * dx + dy * dy + dz * dz;
                items[i] = .{
                    .dist = dist,
                    .junctionIdx1 = idx1,
                    .junctionIdx2 = idx2,
                };
                i += 1;
            }
        }
        break :blk DistanceQueue.fromOwnedSlice(gpa, items, {});
    };
    defer distances.deinit();

    var uf: UnionFind = try .init(gpa, positions.len);
    defer uf.deinit(gpa);

    var steps_left: u32 = switch (partId) {
        .part1 => |n| n,
        .part2 => 0,
    };

    var last_distance: ?DistanceInfo = null;

    while (distances.removeOrNull()) |d| {
        switch (partId) {
            .part1 => {
                if (steps_left == 0) break;
                steps_left -= 1;
            },
            .part2 => {},
        }

        _ = uf.unite(d.junctionIdx1, d.junctionIdx2);
        last_distance = d;

        switch (partId) {
            .part1 => {},
            .part2 => {
                if (uf.component_count == 1) break;
            },
        }
    }

    return switch (partId) {
        .part1 => blk: {
            // Count component sizes
            const sizes = try uf.sizes(gpa);
            defer gpa.free(sizes);

            var max1: usize = 0;
            var max2: usize = 0;
            var max3: usize = 0;
            for (sizes) |sz| {
                if (sz >= max1) {
                    max3 = max2;
                    max2 = max1;
                    max1 = sz;
                } else if (sz >= max2) {
                    max3 = max2;
                    max2 = sz;
                } else if (sz > max3) {
                    max3 = sz;
                }
            }
            const result: usize = max1 * max2 * max3;
            break :blk @intCast(result);
        },
        .part2 => blk: {
            const d = last_distance orelse unreachable;
            const x1: u64 = positions[d.junctionIdx1].x;
            const x2: u64 = positions[d.junctionIdx2].x;
            break :blk x1 * x2;
        },
    };
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
