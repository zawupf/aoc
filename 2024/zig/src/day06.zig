const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Grid = aoc.Grid(Field, usize);
const Pos = Grid.Pos;

const Field = enum(u8) {
    empty = 0,
    up = 0b0001,
    right = 0b0010,
    down = 0b0100,
    left = 0b1000,
    blocked = 0b10000,
    _,
};

const State = enum {
    moving,
    trapped,
    escaped,
};

const Guard = struct {
    pos: Pos,
    dir: aoc.Orientation,
    state: State,

    fn peekNextPos(self: Guard, grid: Grid) ?Pos {
        switch (self.dir) {
            .up => if (self.pos.y == 0) return null,
            .right => if (self.pos.x + 1 == grid.width) return null,
            .down => if (self.pos.y + 1 == grid.height) return null,
            .left => if (self.pos.x == 0) return null,
        }

        const next_pos = self.dir.next(self.pos);
        if (grid.at(next_pos) != .blocked) return next_pos;

        const guard = Guard{
            .pos = self.pos,
            .dir = self.dir.turn(.clockwise),
            .state = self.state,
        };
        return guard.peekNextPos(grid);
    }

    fn move(self: *Guard, grid: *Grid) void {
        self.state = switch (self.dir) {
            .up => if (self.pos.y == 0) .escaped else .moving,
            .right => if (self.pos.x + 1 == grid.width) .escaped else .moving,
            .down => if (self.pos.y + 1 == grid.height) .escaped else .moving,
            .left => if (self.pos.x == 0) .escaped else .moving,
        };
        if (self.state != .moving) return;

        const next_pos = self.dir.next(self.pos);
        if (grid.at(next_pos) != .blocked) {
            self.pos = next_pos;
            self.mark(grid) catch {
                self.state = .trapped;
            };
        } else {
            self.turn(grid) catch {
                self.state = .trapped;
                return;
            };
            self.move(grid);
        }
    }

    fn turn(self: *Guard, grid: *Grid) !void {
        self.dir = self.dir.turn(.clockwise);
        try self.mark(grid);
    }

    fn mark(self: Guard, grid: *Grid) !void {
        const field = grid.at(self.pos);
        const f: Field = switch (self.dir) {
            .up => .up,
            .right => .right,
            .down => .down,
            .left => .left,
        };
        if (@intFromEnum(field) & @intFromEnum(f) != 0) return error.AlreadyMarked;
        grid.setAt(self.pos, @enumFromInt(@intFromEnum(field) | @intFromEnum(f)));
    }
};

fn initState(input: []const u8, gpa: Allocator) !struct { Guard, Grid } {
    var grid = try Grid.init(input, gpa);
    errdefer grid.deinit(gpa);

    for (grid.buf) |*c| switch (@intFromEnum(c.*)) {
        '.' => c.* = .empty,
        '#' => c.* = .blocked,
        '^' => c.* = .up,
        else => unreachable,
    };

    const guard = Guard{
        .pos = grid.findScalar(.up) orelse unreachable,
        .dir = .up,
        .state = .moving,
    };

    return .{ guard, grid };
}

fn copyState(guard: Guard, grid: Grid, gpa: Allocator) !struct { Guard, Grid } {
    return .{
        guard,
        Grid{
            .buf = try gpa.dupe(Field, grid.buf),
            .width = grid.width,
            .height = grid.height,
        },
    };
}

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    var guard, var grid = try initState(input, gpa);
    defer grid.deinit(gpa);

    while (guard.state == .moving) : (guard.move(&grid)) {}

    if (guard.state != .escaped) unreachable;

    var count: u32 = 0;
    for (grid.buf) |c| {
        if (c != .blocked and c != .empty) count += 1;
    }
    return count;
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    var guard, var grid = try initState(input, gpa);
    defer grid.deinit(gpa);

    var count: u32 = 0;
    while (guard.state == .moving) : (guard.move(&grid))
        if (guard.peekNextPos(grid)) |next_pos|
            if (grid.at(next_pos) == .empty) {
                var guard_, var grid_ = try copyState(guard, grid, gpa);
                defer gpa.free(grid_.buf);
                grid_.setAt(next_pos, .blocked);

                while (guard_.state == .moving) : (guard_.move(&grid_)) {}
                if (guard_.state == .trapped) {
                    count += 1;
                }
            };

    return count;
}

pub const Day = aoc.DayInfo("06", u32, u32, 5177, 1686, @This(), &.{.{
    .expected1 = 41,
    .expected2 = 6,
    .input =
    \\....#.....
    \\.........#
    \\..........
    \\..#.......
    \\.......#..
    \\..........
    \\.#..^.....
    \\........#.
    \\#.........
    \\......#...
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
