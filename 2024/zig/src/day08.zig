const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const Grid = aoc.Grid(u8, isize);
const Pos = Grid.Pos;

const AntennaPositions = std.ArrayList(Pos);
const AntennaMap = std.AutoHashMapUnmanaged(u8, AntennaPositions);
const AntennaSet = std.AutoHashMapUnmanaged(Pos, void);

const State = struct {
    grid: Grid,
    antennas: AntennaMap,

    pub fn deinit(self: *State, gpa: Allocator) void {
        self.grid.deinit(gpa);
        var lists = self.antennas.valueIterator();
        while (lists.next()) |list| list.deinit(gpa);
        self.antennas.deinit(gpa);
    }
};

fn initState(input: []const u8, gpa: Allocator) !State {
    const grid = try Grid.init(input, gpa);
    errdefer grid.deinit(gpa);
    var antennas = AntennaMap.empty;
    for (grid.buf, 0..) |field, i| {
        if (field != '.') {
            const entry = try antennas.getOrPut(gpa, field);
            if (!entry.found_existing) {
                entry.value_ptr.* = AntennaPositions.empty;
            }
            try entry.value_ptr.append(gpa, grid.indexToPos(i));
        }
    }
    return .{ .grid = grid, .antennas = antennas };
}

pub fn part1(input: []const u8, gpa: Allocator) !Day.Result1 {
    var state = try initState(input, gpa);
    defer state.deinit(gpa);

    var antidotes = AntennaSet.empty;
    defer antidotes.deinit(gpa);

    var lists = state.antennas.valueIterator();
    while (lists.next()) |list| {
        for (list.items, 1..) |a1, i| {
            for (list.items[i..]) |a2| {
                const delta = Pos.sub(a2, a1);

                const antidote1 = Pos.sub(a1, delta);
                if (state.grid.inBound(antidote1)) {
                    try antidotes.put(gpa, antidote1, {});
                }

                const antidote2 = Pos.add(a2, delta);
                if (state.grid.inBound(antidote2)) {
                    try antidotes.put(gpa, antidote2, {});
                }
            }
        }
    }

    return antidotes.size;
}

pub fn part2(input: []const u8, gpa: Allocator) !Day.Result2 {
    var state = try initState(input, gpa);
    defer state.deinit(gpa);

    var antidotes = AntennaSet.empty;
    defer antidotes.deinit(gpa);

    var lists = state.antennas.valueIterator();
    while (lists.next()) |list| {
        for (list.items, 1..) |a1, i| {
            for (list.items[i..]) |a2| {
                const delta = Pos.sub(a2, a1);

                var p = a1;
                while (state.grid.inBound(p)) : (p.decBy(delta)) {
                    try antidotes.put(gpa, p, {});
                }

                p = Pos.add(a1, delta);
                while (state.grid.inBound(p)) : (p.incBy(delta)) {
                    try antidotes.put(gpa, p, {});
                }
            }
        }
    }

    return antidotes.size;
}

pub const Day = aoc.DayInfo("08", u32, u32, 318, 1126, @This(), &.{.{
    .expected1 = 14,
    .expected2 = 34,
    .input =
    \\............
    \\........0...
    \\.....0......
    \\.......0....
    \\....0.......
    \\......A.....
    \\............
    \\............
    \\........A...
    \\.........A..
    \\............
    \\............
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
