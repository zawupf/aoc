using System;
using System.Linq;
using System.Collections.Generic;
using static Aoc._2019.Utils;

namespace Aoc._2019._14
{
    public class Run : IRun
    {
        public string Job1()
        {
            var factory = Factory.Parse(ReadInputLines("14"));
            factory.Insert(new Chemical(long.MaxValue, "ORE"));
            return factory.Take("1 FUEL").ToString();
        }

        public string Job2()
        {
            var factory = Factory.Parse(ReadInputLines("14"));
            factory.Insert("1000000000000 ORE");
            return factory.BuyFuel().ToString();
        }
    }

    public readonly struct Chemical
    {
        public long Quantity { get; }
        public string Name { get; }

        public Chemical(long quantity, string name)
        {
            (Quantity, Name) = (quantity, name);
        }

        public long CountFor(long quantity)
        {
            return quantity / Quantity + (quantity % Quantity == 0 ? 0 : 1);
        }
        public Chemical Times(long times)
        {
            return new Chemical(Quantity * times, Name);
        }

        public static Chemical Parse(string text)
        {
            var data = text
                .Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            return new Chemical(long.Parse(data[0]), data[1]);
        }

        public override string ToString()
        {
            return $"{Quantity} {Name}";
        }
    }

    public class Reaction
    {
        public readonly Chemical[] Inputs;
        public readonly Chemical Output;

        private Reaction(Chemical[] inputs, Chemical output)
        {
            (Inputs, Output) = (inputs, output);
        }

        public static Reaction Parse(string line)
        {
            var inOutChunks = line
                .Split("=>", 2, StringSplitOptions.RemoveEmptyEntries);
            var (inputText, outuptText) = (inOutChunks[0], inOutChunks[1]);
            return new Reaction(ParseInputs(inputText), Chemical.Parse(outuptText));

            Chemical[] ParseInputs(string text)
            {
                return (
                    from chemical in text.Split(',')
                    select Chemical.Parse(chemical)
                ).ToArray();
            }
        }

        public override string ToString()
        {
            return $"{string.Join(", ", Inputs)} => {Output}";
        }
    }

    class OutOfOreException : Exception { }

    public class Factory
    {
        private Dictionary<string, Reaction> reactions =
            new Dictionary<string, Reaction>();
        private Dictionary<string, long> availableChemicals
            = new Dictionary<string, long>();

        public static Factory Parse(IEnumerable<string> lines)
        {
            var factory = new Factory();
            var reactions = from line in lines select Reaction.Parse(line);
            foreach (var reaction in reactions)
            {
                factory.Add(reaction);
            }
            return factory;
        }

        public void Add(Reaction reaction)
        {
            reactions.Add(reaction.Output.Name, reaction);
            availableChemicals[reaction.Output.Name] =
                availableChemicals.GetValueOrDefault(reaction.Output.Name, 0);
            foreach (var r in reaction.Inputs)
            {
                availableChemicals[r.Name] =
                    availableChemicals.GetValueOrDefault(r.Name, 0);
            }
        }

        public void Insert(string chemical)
        {
            Insert(Chemical.Parse(chemical));
        }

        public void Insert(Chemical chemical)
        {
            availableChemicals[chemical.Name] += chemical.Quantity;
        }

        public long Take(string chemical)
        {
            return Take(Chemical.Parse(chemical));
        }

        public long Take(Chemical chemical)
        {
            long available = availableChemicals[chemical.Name];
            if (available >= chemical.Quantity)
            {
                availableChemicals[chemical.Name] =
                    available - chemical.Quantity;
                return chemical.Name == "ORE" ? chemical.Quantity : 0;
            }
            else
            {
                if (chemical.Name == "ORE")
                    throw new OutOfOreException();

                long missing = chemical.Quantity - availableChemicals[chemical.Name];
                long oreCount = Produce(new Chemical(missing, chemical.Name));
                availableChemicals[chemical.Name] -= chemical.Quantity;
                return oreCount;
            }

            long Produce(Chemical chemical)
            {
                var reaction = reactions[chemical.Name];
                var reactionCount = reaction.Output.CountFor(chemical.Quantity);
                var oreCount = (
                    from chem in reaction.Inputs
                    select Take(chem.Times(reactionCount))
                ).Sum();
                var produced = reaction.Output.Quantity * reactionCount;
                availableChemicals[chemical.Name] += produced;
                return oreCount;
            }
        }

        public long BuyFuel()
        {
            var initialChemicals =
                new Dictionary<string, long>(availableChemicals);

            var (minFuel, maxFuel) = FindMinMaxFuel();
            if (minFuel == 0)
                return 0;

            return FindMaxFuel(minFuel, maxFuel);

            long FindMaxFuel(long min, long max)
            {
                while (min != max - 1)
                {
                    var fuel = (min + max) / 2;
                    bool ok = TryTake(fuel);
                    if (ok)
                        min = fuel;
                    else
                        max = fuel;
                }
                return min;
            }

            (long, long) FindMinMaxFuel()
            {
                var fuel = 1;
                while (TryTake(fuel)) fuel *= 2;
                return (fuel / 2, fuel);
            }

            bool TryTake(long fuel)
            {
                RestoreChemicals();
                try
                {
                    Take(new Chemical(fuel, "FUEL"));
                    return true;
                }
                catch (OutOfOreException)
                {
                    return false;
                }
            }

            void RestoreChemicals()
            {
                availableChemicals =
                    new Dictionary<string, long>(initialChemicals);
            }
        }
    }
}
