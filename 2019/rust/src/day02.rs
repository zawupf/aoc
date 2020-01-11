use crate::computer::Computer;
use crate::utils::read_input_text;

pub fn job1() -> String {
    let mut computer = read_input_text("02")
        .parse::<Computer<i32>>()
        .expect("Invalid source code");
    computer.memory[1] = 12;
    computer.memory[2] = 2;
    computer.run();
    computer.memory[0].to_string()
}

pub fn job2() -> String {
    find_magic_number().to_string()
}

fn find_magic_number() -> i32 {
    for noun in 0..100 {
        for verb in 0..100 {
            let mut computer = read_input_text("02")
                .parse::<Computer<i32>>()
                .expect("Invalid source code");
            computer.memory[1] = noun;
            computer.memory[2] = verb;
            computer.run();
            if computer.memory[0] == 19_690_720 {
                return noun * 100 + verb;
            }
        }
    }
    panic!("No magic result found")
}

#[cfg(test)]
mod tests {
    #[test]
    fn stars() {
        use super::{job1, job2};
        assert_eq!("3765464", job1());
        assert_eq!("7610", job2());
    }
}
