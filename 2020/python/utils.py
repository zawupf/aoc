from pathlib import Path
from typing import List


def _findInputFile(name: str, folder: Path = Path('.').absolute()) -> Path:
    def _parent(folder: Path):
        parent = folder.parent
        if parent == folder:
            raise ValueError('Input file "{}" not found'.format(name))
        return parent

    subpath = Path('_inputs') / 'Day{}.txt'.format(name)
    filepath = folder / subpath
    while not filepath.exists():
        folder = _parent(folder)
        filepath = folder / subpath
    return filepath


def readInputText(name: str) -> str:
    return _findInputFile(name).read_text().strip()


def readInputLines(name: str) -> List[str]:
    with _findInputFile(name).open('r') as file:
        return [line.strip() for line in file if not line.isspace()]
