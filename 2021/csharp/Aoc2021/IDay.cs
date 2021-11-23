namespace Aoc2021;

public abstract class IDay
{
    public abstract string Day { get; }

    public abstract string Result1();
    public abstract string Result2();

    public IEnumerable<string> InputLines => ReadInputLines(Day);
    public string InputText => ReadInputText(Day);
}
