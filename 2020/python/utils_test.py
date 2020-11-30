import unittest
from utils import readInputLines, readInputText


class TestUtils(unittest.TestCase):
    def test_readInputText(self):
        self.assertTrue(readInputText('00').startswith(
            '139301\n84565\n124180\n'))

    def test_fail_readInputText(self):
        self.assertRaises(ValueError, lambda: readInputText('xy'))

    def test_readInputLines(self):
        self.assertEqual(['139301', '84565', '124180'],
                         readInputLines('00')[:3])

    def test_fail_readInputLines(self):
        self.assertRaises(ValueError, lambda: readInputLines('xy'))
