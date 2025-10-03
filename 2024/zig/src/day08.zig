const std = @import("std");

const aoc = @import("aoc_utils");

const Grid = aoc.Grid(u8, isize);
const Pos = Grid.Pos;

const AntennaPositions = std.ArrayList(Pos);
const AntennaMap = std.AutoHashMapUnmanaged(u8, AntennaPositions);
const AntennaSet = std.AutoHashMapUnmanaged(Pos, void);

const State = struct {
    grid: Grid,
    antennas: AntennaMap,

    pub fn deinit(self: *State, gpa: std.mem.Allocator) void {
        gpa.free(self.grid.buf);
        var lists = self.antennas.valueIterator();
        while (lists.next()) |list| list.deinit(gpa);
        self.antennas.deinit(gpa);
    }
};

fn initState(input: []const u8, gpa: std.mem.Allocator) !State {
    const gridBuffer = try gpa.dupe(u8, input);
    const grid = Grid.init(gridBuffer);
    var antennas = AntennaMap.empty;
    for (grid.buf, 0..) |field, i| {
        if (field != '.' and field != '\n') {
            const entry = try antennas.getOrPut(gpa, field);
            if (!entry.found_existing) {
                entry.value_ptr.* = AntennaPositions.empty;
            }
            try entry.value_ptr.append(gpa, grid.indexToPos(i));
        }
    }
    return .{ .grid = grid, .antennas = antennas };
}

fn part1(input: []const u8, gpa: std.mem.Allocator) !Day.Result1 {
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

fn part2(input: []const u8, gpa: std.mem.Allocator) !Day.Result2 {
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

const Day = aoc.DayInfo("08", u32, u32, 318, 1126, &.{.{
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
    try Day.testPart1Samples(part1);
}
test "samples 2" {
    try Day.testPart2Samples(part2);
}
test "part 1" {
    try Day.testPart1(part1);
}
test "part 2" {
    try Day.testPart2(part2);
}
