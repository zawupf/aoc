const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Field = enum(u8) {
    wall = '#',
    empty = '.',
    box = 'O',
    box_left = '[',
    box_right = ']',
    robot = '@',
};
const Grid = aoc.Grid(Field, u8);

const Move = enum(u8) { up = '^', down = 'v', left = '<', right = '>', no_move = '\n' };

const Errors = error{InvalidInput};

const State = struct {
    grid: Grid,
    moves: []const Move,

    pub fn init(input: []const u8, gpa: Allocator) !State {
        var chunks = aoc.splitChunks(input);
        const grid: Grid = try .init(chunks.next() orelse return Errors.InvalidInput, gpa);
        const moves: []const Move = @ptrCast(chunks.next() orelse return Errors.InvalidInput);
        return .{ .grid = grid, .moves = moves };
    }

    pub fn deinit(self: *const State, gpa: Allocator) void {
        self.grid.deinit(gpa);
    }

    pub fn scaleUp(self: *State, gpa: Allocator) !void {
        var buf = try gpa.alloc(Field, self.grid.buf.len * 2);
        errdefer gpa.free(buf);
        for (self.grid.buf, 0..) |field, i| {
            const j = i * 2;
            const fields: [2]Field = switch (field) {
                .wall => .{ .wall, .wall },
                .empty => .{ .empty, .empty },
                .box => .{ .box_left, .box_right },
                .box_left => unreachable,
                .box_right => unreachable,
                .robot => .{ .robot, .empty },
            };
            @memcpy(buf[j .. j + 2], &fields);
        }
        gpa.free(self.grid.buf);
        self.grid.buf = buf;
        self.grid.width *= 2;
    }

    pub fn robot(self: State) Grid.Pos {
        return self.grid.findScalar(.robot).?;
    }

    fn nextPos(from: Grid.Pos, dir: Move) Grid.Pos {
        return switch (dir) {
            .up => .{ .x = from.x, .y = from.y - 1 },
            .down => .{ .x = from.x, .y = from.y + 1 },
            .left => .{ .x = from.x - 1, .y = from.y },
            .right => .{ .x = from.x + 1, .y = from.y },
            .no_move => unreachable,
        };
    }

    pub fn canMove(self: State, from: Grid.Pos, dir: Move) bool {
        const to = nextPos(from, dir);
        const field = self.grid.at(to);
        return switch (field) {
            .wall => false,
            .empty => true,
            .box => self.canMove(to, dir),
            .box_left => blk: {
                switch (dir) {
                    .right => break :blk self.canMove(nextPos(to, .right), dir),
                    .up, .down => break :blk self.canMove(to, dir) and self.canMove(nextPos(to, .right), dir),
                    .left, .no_move => unreachable,
                }
            },
            .box_right => blk: {
                switch (dir) {
                    .left => break :blk self.canMove(nextPos(to, .left), dir),
                    .up, .down => break :blk self.canMove(to, dir) and self.canMove(nextPos(to, .left), dir),
                    .right, .no_move => unreachable,
                }
            },
            .robot => unreachable,
        };
    }

    fn move(self: *State, from: Grid.Pos, to: Grid.Pos) void {
        const fromField = self.grid.at(from);
        self.grid.setAt(to, fromField);
        self.grid.setAt(from, .empty);
    }

    pub fn doMove(self: *State, from: Grid.Pos, dir: Move) Grid.Pos {
        const to = nextPos(from, dir);
        const toField = self.grid.at(to);
        switch (toField) {
            .wall => unreachable,
            .empty => {
                self.move(from, to);
            },
            .box => {
                _ = self.doMove(to, dir);
                self.move(from, to);
            },
            .box_left => {
                const nextTo = nextPos(to, .right);
                switch (dir) {
                    .right => {
                        _ = self.doMove(nextTo, dir);
                        self.grid.setAt(nextTo, toField);
                        self.move(from, to);
                    },
                    .up, .down => {
                        _ = self.doMove(to, dir);
                        _ = self.doMove(nextTo, dir);
                        self.move(from, to);
                    },
                    .left, .no_move => unreachable,
                }
            },
            .box_right => {
                const nextTo = nextPos(to, .left);
                switch (dir) {
                    .left => {
                        _ = self.doMove(nextTo, dir);
                        self.grid.setAt(nextTo, toField);
                        self.move(from, to);
                    },
                    .up, .down => {
                        _ = self.doMove(to, dir);
                        _ = self.doMove(nextTo, dir);
                        self.move(from, to);
                    },
                    .right, .no_move => unreachable,
                }
            },
            .robot => unreachable,
        }
        return to;
    }

    pub fn moveRobot(self: *State) void {
        var r = self.robot();
        for (self.moves) |m| {
            if (m != .no_move and self.canMove(r, m)) {
                r = self.doMove(r, m);
            }
        }
    }

    pub fn gpsCoordSum(self: State) u32 {
        var sum: u32 = 0;
        for (self.grid.buf, 0..) |field, i| {
            if (field == .box or field == .box_left) {
                const pos = self.grid.indexToPos(i);
                sum += @as(u32, pos.x) + 100 * @as(u32, pos.y);
            }
        }
        return sum;
    }
};

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    var state: State = try .init(input, gpa);
    defer state.deinit(gpa);
    state.moveRobot();
    return state.gpsCoordSum();
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    var state: State = try .init(input, gpa);
    defer state.deinit(gpa);
    try state.scaleUp(gpa);
    state.moveRobot();
    return state.gpsCoordSum();
}

pub const Day = aoc.DayInfo("15", u32, u32, 1563092, 1582688, @This(), &.{
    .{
        .expected1 = 2028,
        .expected2 = null,
        .input =
        \\########
        \\#..O.O.#
        \\##@.O..#
        \\#...O..#
        \\#.#.O..#
        \\#...O..#
        \\#......#
        \\########
        \\
        \\<^^>>>vv<v>>v<<
        ,
    },
    .{
        .expected1 = 10092,
        .expected2 = 9021,
        .input =
        \\##########
        \\#..O..O.O#
        \\#......O.#
        \\#.OO..O.O#
        \\#..O@..O.#
        \\#O#..O...#
        \\#O..O..O.#
        \\#.OO.O.OO#
        \\#....O...#
        \\##########
        \\
        \\<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
        \\vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
        \\><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
        \\<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
        \\^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
        \\^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
        \\>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
        \\<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
        \\^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
        \\v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
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
