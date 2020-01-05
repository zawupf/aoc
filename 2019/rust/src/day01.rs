use crate::utils::read_input_lines;

pub fn job1() -> String {
    read_input_lines("01")
        .into_iter()
        .map(|line| fuel_per_mass(line.parse::<i32>().expect("Parse i32 failed")))
        .sum::<i32>()
        .to_string()
}

pub fn job2() -> String {
    read_input_lines("01")
        .into_iter()
        .map(|line| total_fuel_per_mass(line.parse::<i32>().expect("Parse i32 failed")))
        .sum::<i32>()
        .to_string()
}

fn total_fuel_per_mass(mass: i32) -> i32 {
    match fuel_per_mass(mass) {
        0 => 0,
        f => f + total_fuel_per_mass(f),
    }
}

fn fuel_per_mass(mass: i32) -> i32 {
    match mass / 3 - 2 {
        x if x > 0 => x,
        _ => 0,
    }
}

#[cfg(test)]
mod tests {
    #[test]
    fn stars() {
        use super::{job1, job2};
        assert_eq!("3421505", job1());
        assert_eq!("5129386", job2());
    }

    #[test]
    fn fuel_per_mass_works() {
        use super::fuel_per_mass;
        assert_eq!(2, fuel_per_mass(12));
        assert_eq!(2, fuel_per_mass(14));
        assert_eq!(654, fuel_per_mass(1969));
        assert_eq!(33583, fuel_per_mass(100_756));
    }

    #[test]
    fn total_fuel_per_mass_works() {
        use super::total_fuel_per_mass;
        assert_eq!(2, total_fuel_per_mass(14));
        assert_eq!(966, total_fuel_per_mass(1969));
        assert_eq!(50346, total_fuel_per_mass(100_756));
    }
}
