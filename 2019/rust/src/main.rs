extern crate aoc_2019;

use aoc_2019::*;

fn main() {
    // run(1, day01::job1, day01::job2)
    // run(2, day02::job1, day02::job2)
    run(3, day03::job1, day03::job2)
}

fn run(day: i32, job1: impl Fn() -> String, job2: impl Fn() -> String) {
    println!("Day {}:", day);
    println!("Result 1: {}", job1());
    println!("Result 2: {}", job2());
}
