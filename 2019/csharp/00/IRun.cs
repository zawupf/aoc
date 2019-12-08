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
        private string InputPrefix { get; }

        protected BaseRun(string InputPrefix = "")
        {
            this.InputPrefix = InputPrefix;
        }

        protected IEnumerable<string> ReadLines(
            string path) => File.ReadLines(InputPrefix + path);

        protected string ReadAllText(
            string path) => File.ReadAllText(InputPrefix + path).TrimEnd();
    }
}
