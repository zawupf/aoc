from utils import readInputLines


def main():
    """
    The main entry
    """
    print(greeting('World'))
    print(readInputLines('00')[:5])


def greeting(name: str) -> str:
    """
    Build the greeting message
    """
    return 'Hello, ' + name + '!'


if __name__ == "__main__":
    main()
