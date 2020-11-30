from main import greeting
import unittest


class TestMain(unittest.TestCase):
    def test_greeting(self):
        self.assertEqual('Hello, World!', greeting('World'))
        self.assertEqual('Hello, Arni!', greeting('Arni'))
