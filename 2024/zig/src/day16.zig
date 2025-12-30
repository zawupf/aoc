const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Turn = enum(u2) { left, right, straight };
const Direction = enum(u2) { north, east, south, west };
const Grid = aoc.Grid(Tile, u8);
const Score = u32;
const Tile = struct {
    free: bool,
    scores: [4]Score,
    pub fn init(c: u8) Tile {
        return .{
            .free = c != '#',
            .scores = [_]Score{std.math.maxInt(u32)} ** 4,
        };
    }
};
const Reindeer = struct {
    pos: Grid.Pos,
    dir: Direction,

    fn next(self: Reindeer, turn: Turn, grid: Grid) ?Reindeer {
        const d: u3 = @intFromEnum(self.dir);
        const dir: Direction = @enumFromInt(switch (turn) {
            .left => (d + 3) % 4,
            .right => (d + 1) % 4,
            .straight => d,
        });

        const pos: Grid.Pos = switch (dir) {
            .north => .{ .x = self.pos.x, .y = self.pos.y - 1 },
            .south => .{ .x = self.pos.x, .y = self.pos.y + 1 },
            .east => .{ .x = self.pos.x + 1, .y = self.pos.y },
            .west => .{ .x = self.pos.x - 1, .y = self.pos.y },
        };

        return if (grid.at(pos).free) .{ .pos = pos, .dir = dir } else null;
    }
};

const turns = std.enums.values(Turn);

const Mode = enum { min_score, field_count };

pub fn solve(comptime mode: Mode, input: []const u8, gpa: Allocator) !u32 {
    var grid: Grid = try .initMapped(input, Tile.init, gpa);
    defer grid.deinit(gpa);

    const Path = switch (mode) {
        .min_score => void,
        .field_count => std.SinglyLinkedList,
    };
    const PathNode = switch (mode) {
        .min_score => void,
        .field_count => struct { pos: Grid.Pos, node: Path.Node },
    };

    const State = switch (mode) {
        .min_score => Reindeer,
        .field_count => struct { reindeer: Reindeer, path: Path },
    };

    const Result = switch (mode) {
        .min_score => u32,
        .field_count => struct {
            score: u32,
            paths: std.ArrayList(Path),
        },
    };

    var arena: std.heap.ArenaAllocator = .init(gpa);
    defer arena.deinit();
    const gpa_arena = arena.allocator();

    var stack: std.ArrayList(State) = .empty;
    defer stack.deinit(gpa);
    const start = Grid.Pos{ .x = 1, .y = grid.height - 2 };
    grid.ptr(start).scores[@intFromEnum(Direction.east)] = 0;
    const end = Grid.Pos{ .x = grid.width - 2, .y = 1 };

    const state: State = switch (mode) {
        .min_score => .{ .pos = start, .dir = .east },
        .field_count => init: {
            var startPath: Path = .{};
            var startNode = try gpa_arena.create(PathNode);
            startNode.pos = start;
            startPath.prepend(&startNode.node);
            break :init .{
                .reindeer = .{ .pos = start, .dir = .east },
                .path = startPath,
            };
        },
    };
    try stack.append(gpa, state);

    var result: Result = switch (mode) {
        .min_score => std.math.maxInt(u32),
        .field_count => Result{
            .score = std.math.maxInt(u32),
            .paths = .empty,
        },
    };
    defer if (mode == .field_count) result.paths.deinit(gpa);

    while (stack.pop()) |s| {
        const r = if (mode == .min_score) s else s.reindeer;
        if (r.pos.x == end.x and r.pos.y == end.y) {
            const cur_score = grid.at(r.pos).scores[@intFromEnum(r.dir)];
            switch (mode) {
                .min_score => result = @min(result, cur_score),
                .field_count => {
                    if (cur_score > result.score) continue;
                    if (cur_score < result.score) {
                        result.paths.clearRetainingCapacity();
                        result.score = cur_score;
                    }
                    try result.paths.append(gpa, s.path);
                },
            }
            continue;
        }

        const current_score = grid.at(r.pos).scores[@intFromEnum(r.dir)];
        for (turns) |turn| {
            if (r.next(turn, grid)) |next| {
                const points: u32 = switch (turn) {
                    .left, .right => 1001,
                    .straight => 1,
                };
                const new_score = current_score + points;
                const next_score_ptr = &grid.ptr(next.pos).scores[@intFromEnum(next.dir)];
                const skip = switch (mode) {
                    .min_score => new_score >= next_score_ptr.*,
                    .field_count => new_score > next_score_ptr.*,
                };
                if (skip) continue;

                next_score_ptr.* = new_score;

                const next_state: State = switch (mode) {
                    .min_score => next,
                    .field_count => blk: {
                        var next_path = s.path;
                        var next_node = try gpa_arena.create(PathNode);
                        next_node.pos = next.pos;
                        next_path.prepend(&next_node.node);
                        break :blk .{ .reindeer = next, .path = next_path };
                    },
                };
                try stack.append(gpa, next_state);
            }
        }
    }

    return switch (mode) {
        .min_score => result,
        .field_count => blk: {
            var fieldSet: std.DynamicBitSetUnmanaged = try .initEmpty(gpa, grid.buf.len);
            defer fieldSet.deinit(gpa);

            for (result.paths.items) |path| {
                var path_iter = path.first;
                while (path_iter) |node| : (path_iter = node.next) {
                    const p: *PathNode = @fieldParentPtr("node", node);
                    const idx = grid.posToIndex(p.pos);
                    fieldSet.set(idx);
                }
            }
            break :blk @intCast(fieldSet.count());
        },
    };
}

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    return solve(.min_score, input, gpa);
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    return solve(.field_count, input, gpa);
}

pub const Day = aoc.DayInfo("16", u32, u32, 85432, 465, @This(), &.{
    .{
        .expected1 = 7036,
        .expected2 = 45,
        .input =
        \\###############
        \\#.......#....E#
        \\#.#.###.#.###.#
        \\#.....#.#...#.#
        \\#.###.#####.#.#
        \\#.#.#.......#.#
        \\#.#.#####.###.#
        \\#...........#.#
        \\###.#.#####.#.#
        \\#...#.....#.#.#
        \\#.#.#.###.#.#.#
        \\#.....#...#.#.#
        \\#.###.#.#.#.#.#
        \\#S..#.....#...#
        \\###############
        ,
    },
    .{
        .expected1 = 11048,
        .expected2 = 64,
        .input =
        \\#################
        \\#...#...#...#..E#
        \\#.#.#.#.#.#.#.#.#
        \\#.#.#.#...#...#.#
        \\#.#.#.#.###.#.#.#
        \\#...#.#.#.....#.#
        \\#.#.#.#.#.#####.#
        \\#.#...#.#.#.....#
        \\#.#.#####.#.###.#
        \\#.#.#.......#...#
        \\#.#.###.#####.###
        \\#.#.#...#.....#.#
        \\#.#.#.#####.###.#
        \\#.#.#.........#.#
        \\#.#.#.#########.#
        \\#S#.............#
        \\#################
        ,
    },
});

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
