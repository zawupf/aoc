using System.Collections.Generic;
using System.IO;

namespace Aoc._2019
{
    public interface IRun
    {
        public string Job1();
        public string Job2();
    }

    public class BaseRun
    {
        protected string InputPrefix { get; }

        protected BaseRun(string InputPrefix = "")
        {
            this.InputPrefix = InputPrefix;
        }

        public IEnumerable<string> ReadLines(
            string path) => File.ReadLines(InputPrefix + path);

        public string ReadAllText(
            string path) => File.ReadAllText(InputPrefix + path).TrimEnd();
    }
}
