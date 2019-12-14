using System;
using System.Linq;
using System.Collections.Generic;

namespace Aoc._2019._14
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var factory = Factory.Parse(ReadLines("14/input1.txt"));
            return factory.Take("1 FUEL").ToString();
        }

        public string Job2()
        {
            var factory = Factory.Parse(ReadLines("14/input1.txt"));
            return factory.BuyFuel(1000000000000).ToString();
        }
    }

    public readonly struct Chemical
    {
        public int Quantity { get; }
        public string Name { get; }

        public Chemical(int quantity, string name)
        {
            (Quantity, Name) = (quantity, name);
        }

        public int CountFor(int quantity)
        {
            return quantity / Quantity + (quantity % Quantity == 0 ? 0 : 1);
        }
        public Chemical Times(int times)
        {
            return new Chemical(Quantity * times, Name);
        }

        public static Chemical Parse(string text)
        {
            var data = text
                .Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            return new Chemical(int.Parse(data[0]), data[1]);
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

    public class Factory
    {
        private Dictionary<string, Reaction> reactions =
            new Dictionary<string, Reaction>();
        private Dictionary<string, int> availableChemicals
            = new Dictionary<string, int>();

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
        }

        public int Available(string chemical)
        {
            return availableChemicals.GetValueOrDefault(
                chemical, chemical == "ORE" ? int.MaxValue : 0);
        }

        public int Take(string chemical)
        {
            return Take(Chemical.Parse(chemical));
        }

        public int Take(Chemical chemical)
        {
            if (chemical.Name == "ORE")
                return chemical.Quantity;

            int available = Available(chemical.Name);
            if (available >= chemical.Quantity)
            {
                availableChemicals[chemical.Name] =
                    available - chemical.Quantity;
                return 0;
            }
            else
            {
                int missing = chemical.Quantity - Available(chemical.Name);
                int oreCount = Produce(new Chemical(missing, chemical.Name));
                availableChemicals[chemical.Name] =
                    Available(chemical.Name) - chemical.Quantity;
                return oreCount;
            }

            int Produce(Chemical chemical)
            {
                var reaction = reactions[chemical.Name];
                var reactionCount = reaction.Output.CountFor(chemical.Quantity);
                var oreCount = (
                    from chem in reaction.Inputs
                    select Take(chem.Times(reactionCount))
                ).Sum();
                var produced = reaction.Output.Quantity * reactionCount;
                availableChemicals[chemical.Name] =
                    Available(chemical.Name) + produced;
                return oreCount;
            }
        }

        public long BuyFuel(long oreCount)
        {
            var fuel = new Chemical(1, "FUEL");
            long chunkSize = 0;
            long chunkCost = 0;
            do
            {
                ++chunkSize;
                chunkCost += Take(fuel);
            } while (!Empty());
            var chunkCount = oreCount / chunkCost;

            var fuelCount = chunkCount * chunkSize;
            var ore = oreCount % chunkCost;
            while (true)
            {
                var cost = Take(fuel);
                ore -= cost;
                if (ore >= 0)
                    ++fuelCount;
                else
                    break;
            }

            return fuelCount;

            bool Empty()
            {
                return availableChemicals.Values.All(value => value == 0);
            }
        }
    }
}
