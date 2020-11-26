using System.Collections.Generic;

namespace Aoc2020Calendar
{
    public abstract class IDay
    {
        public abstract string Day { get; }

        public abstract string Result1();
        public abstract string Result2();

        public IEnumerable<string> InputLines => Utils.ReadInputLines(Day);
        public string InputText => Utils.ReadInputText(Day);
    }
}
