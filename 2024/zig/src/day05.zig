const std = @import("std");
const Allocator = std.mem.Allocator;

const aoc = @import("aoc_utils");

const OrderingRule = struct {
    before: u8,
    after: u8,

    fn parse(line: []const u8) !OrderingRule {
        var parts = aoc.split(line, '|');
        const before = try std.fmt.parseInt(u8, parts.next().?, 10);
        const after = try std.fmt.parseInt(u8, parts.next().?, 10);
        return .{ .before = before, .after = after };
    }
};

const Pages = struct {
    items: []u8,

    fn isValid(self: @This(), rules: []const OrderingRule) bool {
        for (rules) |rule| {
            const before = std.mem.findScalar(u8, self.items, rule.before);
            if (before == null) continue;
            const after = std.mem.findScalar(u8, self.items, rule.after);
            if (after == null) continue;
            if (before.? >= after.?) return false;
        }
        return true;
    }

    fn middlePage(self: @This()) u8 {
        return self.items[self.items.len / 2];
    }

    fn sort(self: *@This(), rules: []const OrderingRule) void {
        std.mem.sortUnstable(u8, self.items, rules, @This().isValidOrder);
    }

    fn isValidOrder(rules: []const OrderingRule, before: u8, after: u8) bool {
        for (rules) |rule| {
            if (rule.before == before and rule.after == after) return true;
        }
        return false;
    }
};

const UpdateData = struct {
    rules: []OrderingRule,
    updates: []Pages,

    fn init(input: []const u8, gpa: Allocator) !UpdateData {
        var rules = std.ArrayList(OrderingRule).empty;
        var updates = std.ArrayList(Pages).empty;
        errdefer {
            rules.deinit(gpa);
            for (updates.items) |pages| {
                gpa.free(pages.items);
            }
            updates.deinit(gpa);
        }

        var chunks = aoc.splitChunks(input);

        var rulesIter = aoc.splitLines(chunks.next().?);
        while (rulesIter.next()) |line| {
            try rules.append(gpa, try OrderingRule.parse(line));
        }

        var updatesIter = aoc.splitLines(chunks.next().?);
        while (updatesIter.next()) |line| {
            var pages = std.ArrayList(u8).empty;
            errdefer pages.deinit(gpa);
            var parts = aoc.split(line, ',');
            while (parts.next()) |part| {
                try pages.append(gpa, try std.fmt.parseInt(u8, part, 10));
            }
            try updates.append(gpa, .{ .items = try pages.toOwnedSlice(gpa) });
        }

        std.debug.assert(chunks.next() == null);

        return .{
            .rules = try rules.toOwnedSlice(gpa),
            .updates = try updates.toOwnedSlice(gpa),
        };
    }

    fn deinit(self: *const UpdateData, gpa: Allocator) void {
        gpa.free(self.rules);
        for (self.updates) |pages| {
            gpa.free(pages.items);
        }
        gpa.free(self.updates);
    }
};

fn part1(input: []const u8, gpa: Allocator) !u32 {
    const data = try UpdateData.init(input, gpa);
    defer data.deinit(gpa);

    var result: u32 = 0;
    for (data.updates) |pages| {
        if (pages.isValid(data.rules)) {
            result += @intCast(pages.middlePage());
        }
    }
    return result;
}

fn part2(input: []const u8, gpa: Allocator) !u32 {
    const data = try UpdateData.init(input, gpa);
    defer data.deinit(gpa);

    var result: u32 = 0;
    for (data.updates) |*pages| {
        if (!pages.isValid(data.rules)) {
            pages.sort(data.rules);
            result += @intCast(pages.middlePage());
        }
    }
    return result;
}

const Day = aoc.DayInfo("05", u32, u32, 6612, 4944, &.{.{
    .expected1 = 143,
    .expected2 = 123,
    .input =
    \\47|53
    \\97|13
    \\97|61
    \\97|47
    \\75|29
    \\61|13
    \\75|53
    \\29|13
    \\97|29
    \\53|29
    \\61|53
    \\97|53
    \\61|29
    \\47|13
    \\75|47
    \\97|75
    \\47|61
    \\75|61
    \\47|29
    \\75|13
    \\53|13
    \\
    \\75,47,61,53,29
    \\97,61,53,29,13
    \\75,29,13
    \\75,97,47,61,53
    \\61,13,29
    \\97,13,75,29,47
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
