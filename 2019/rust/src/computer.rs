use std::ops::Add;
use std::ops::Mul;
use std::str::FromStr;

pub struct Computer<T> {
    position: usize,
    pub memory: Vec<T>,
}

impl<T> Computer<T>
where
    T: FromStr + Into<Opcode> + Into<Position> + Copy + Add<Output = T> + Mul<Output = T>,
{
    fn new(memory: Vec<T>) -> Computer<T> {
        Computer {
            position: 0,
            memory,
        }
    }

    fn compile(source: &str) -> Result<Vec<T>, <T as FromStr>::Err> {
        Ok(source
            .split(',')
            .map(|number| number.parse::<T>())
            .collect::<Result<_, _>>()?)
    }

    pub fn run(&mut self) -> Status {
        use Opcode::*;
        match self.opcode() {
            Add => self.add(),
            Mul => self.mul(),
            Halt => {
                self.halt();
                return Status::Halted;
            }
        }
        self.run()
    }

    fn opcode(&self) -> Opcode {
        self.memory[self.position].into()
    }

    fn add(&mut self) {
        let a = self.par(0);
        let b = self.par(1);
        self.out(2, a + b);
        self.position += 4;
    }

    fn mul(&mut self) {
        let a = self.par(0);
        let b = self.par(1);
        self.out(2, a * b);
        self.position += 4;
    }

    fn halt(&mut self) {}

    fn par(&self, i: usize) -> T {
        let Position::Value(position) = self.memory[self.position + i + 1].into();
        self.memory[position]
    }

    fn out(&mut self, i: usize, value: T) {
        let Position::Value(position) = self.memory[self.position + i + 1].into();
        self.memory[position] = value;
    }
}

impl<T> FromStr for Computer<T>
where
    T: FromStr + Into<Opcode> + Into<Position> + Copy + Add<Output = T> + Mul<Output = T>,
{
    type Err = <T as FromStr>::Err;
    fn from_str(source: &str) -> Result<Self, <Self as FromStr>::Err> {
        Ok(Computer::new(Computer::compile(source)?))
    }
}

pub enum Position {
    Value(usize),
}

impl From<i32> for Position {
    fn from(value: i32) -> Self {
        Position::Value(value as usize)
    }
}

impl From<i64> for Position {
    fn from(value: i64) -> Self {
        Position::Value(value as usize)
    }
}

pub enum Opcode {
    Add,
    Mul,
    Halt,
}

impl From<i32> for Opcode {
    fn from(value: i32) -> Self {
        use Opcode::*;
        match value {
            1 => Add,
            2 => Mul,
            99 => Halt,
            _ => panic!("Invalid opcode"),
        }
    }
}

impl From<&i32> for Opcode {
    fn from(value: &i32) -> Self {
        Opcode::from(*value)
    }
}

impl From<i64> for Opcode {
    fn from(value: i64) -> Self {
        Opcode::from(value as i32)
    }
}

impl From<&i64> for Opcode {
    fn from(value: &i64) -> Self {
        Opcode::from(*value)
    }
}

pub enum Status {
    Halted,
}

#[cfg(test)]
mod tests {
    use super::Computer;

    static SAMPLE: &str = "1,9,10,3,2,3,11,0,99,30,40,50";

    #[test]
    fn from_str_works() {
        let computer = SAMPLE.parse::<Computer<i32>>().unwrap();
        let expected_memory: Vec<i32> = vec![1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50];
        assert_eq!(expected_memory, computer.memory)
    }

    #[test]
    fn computer_works() {
        let mut computer = SAMPLE.parse::<Computer<i32>>().unwrap();
        computer.run();
        assert_eq!(3500, computer.memory[0]);
    }
}
