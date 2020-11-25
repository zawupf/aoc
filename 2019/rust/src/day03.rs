use crate::utils::read_input_lines;
use std::collections::HashSet;
use std::str::FromStr;

pub fn job1() -> String {
    let wires = read_input_lines("03")
        .into_iter()
        .map(|line| line.parse::<Wire>());
    "".to_owned()
}

pub fn job2() -> String {
    "".to_owned()
    // read_input_lines("01")
    //     .into_iter()
    //     .map(|line| total_fuel_per_mass(line.parse::<i32>().expect("Parse i32 failed")))
    //     .sum::<i32>()
    //     .to_string()
}

#[derive(PartialEq, Eq, Hash, Clone, Copy)]
struct Point(i32, i32);

struct Wire(HashSet<Point>);
impl FromStr for Wire {
    type Err = String;
    fn from_str(line: &str) -> Result<Self, <Self as FromStr>::Err> {
        use Move::*;
        let mut points = HashSet::new();
        let mut cursor = Point(0, 0);
        points.insert(cursor);

        line.parse::<Moves>()?.0.into_iter().for_each(|m| {
            let (l, dx, dy) = match m {
                Up(l) => (l, 0, -1),
                Down(l) => (l, 0, 1),
                Left(l) => (l, -1, 0),
                Right(l) => (l, 1, 0),
            };
            (0..l).for_each(|_| {
                cursor.0 += dx;
                cursor.1 += dy;
                points.insert(cursor);
            });
        });

        Ok(Wire(points))
    }
}

struct Moves(Vec<Move>);
impl FromStr for Moves {
    type Err = String;
    fn from_str(line: &str) -> Result<Self, <Self as FromStr>::Err> {
        line.split(',')
            .map(|mv| mv.parse::<Move>())
            .collect::<Result<Vec<_>, _>>()
            .map(Moves)
    }
}

enum Move {
    Up(i32),
    Down(i32),
    Left(i32),
    Right(i32),
}
impl FromStr for Move {
    type Err = String;
    fn from_str(m: &str) -> Result<Self, <Self as FromStr>::Err> {
        use Move::*;
        let direction = m.get(0..1).ok_or("Invalid move direction")?;
        let length = m
            .get(1..)
            .ok_or("Invalid move length")?
            .parse::<i32>()
            .or(Err("Invalid move length"))?;
        match direction {
            "U" => Ok(Up(length)),
            "D" => Ok(Down(length)),
            "L" => Ok(Left(length)),
            "R" => Ok(Right(length)),
            _ => Err("Invalid move direction".into()),
        }
    }
}

#[cfg(test)]
mod tests {
    #[test]
    fn stars() {
        use super::{job1, job2};
        // assert_eq!("8015", job1());
        // assert_eq!("163676", job2());
    }
}
