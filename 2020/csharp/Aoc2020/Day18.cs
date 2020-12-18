using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day18 : IDay
    {
        public override string Day { get; } = nameof(Day18)[3..];

        public override string Result1() =>
            InputLines
            .Select(EvaluateSimple)
            .Sum()
            .ToString();

        public override string Result2() =>
            InputLines
            .Select(EvaluateAdvanced)
            .Sum()
            .ToString();

        public static long EvaluateAdvanced(string expr) =>
            Evaluate(expr, AdvancedALU);

        public static long EvaluateSimple(string expr) =>
            Evaluate(expr, SimpleALU);

        private static readonly Regex rxGroup = new(@"\([^()]+\)");
        private static long Evaluate(string expr, Func<string, long> alu)
        {
            string prevExpr;
            do
            {
                prevExpr = expr;
                expr = rxGroup.Replace(expr, m => alu(m.Value[1..^1]).ToString());
            } while (expr != prevExpr);
            return alu(expr);
        }

        internal static long SimpleALU(string expr) =>
            expr
            .Split(' ')
            .Aggregate((0L, '\0'), (state, symbol) =>
            {
                var (result, op) = state;
                if (symbol == "+" || symbol == "*")
                    op = symbol[0];
                else
                {
                    var n = long.Parse(symbol);
                    result = op switch
                    {
                        '+' => result + n,
                        '*' => result * n,
                        '\0' => n,
                        '?' => throw new ApplicationException("Undefined operator"),
                        _ => throw new ApplicationException("Invalid operator"),
                    };
                    op = '?';
                }
                return (result, op);
            })
            .Item1;

        private static readonly Regex rxHighPrio = new(@"(\d+ \+ )+\d+");
        internal static long AdvancedALU(string expr) =>
            EvaluateSimple(rxHighPrio.Replace(expr, @"($0)"));
    }
}
