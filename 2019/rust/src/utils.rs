use std::env::current_dir;
use std::path::PathBuf;

pub fn read_input_text(name: &str) -> String {
    let path = find_input_file(name);
    std::fs::read_to_string(path)
        .expect("Read file to string failed")
        .trim()
        .to_owned()
}

pub fn read_input_lines(name: &str) -> Vec<String> {
    read_input_text(name)
        .split('\n')
        .map(|line| line.to_owned())
        .collect()
}

fn find_input_file(name: &str) -> PathBuf {
    current_dir()
        .expect("No current directory")
        .ancestors()
        .find_map(
            |dir| match dir.join("inputs").join(format!("Day{}.txt", name)) {
                path if path.exists() => Some(path),
                _ => None,
            },
        )
        .expect("Input file not found")
}
