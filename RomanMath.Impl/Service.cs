using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace RomanMath.Impl
{
	public static class Service
	{
        /// <summary>
        /// See TODO.txt file for task details.
        /// Do not change contracts: input and output arguments, method name and access modifiers
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>

        public static int Evaluate(string expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            expression = expression.Trim();
            if (expression.Length == 0) throw new ArgumentException("Expression can't be empty");

            if (
                Regex.IsMatch(expression,
                    @"[^\+\-\*MDCLXVI] || [IX]{2,}[MDCLV] || [VLD]{2,} || [IXCM]{3,} || [+-[*]]{2,}") ||
                !Regex.IsMatch(expression, @"^(?:[MDCLXVI]+[\+\-\*])+[MDCLXVI]$")
            )
            {
                throw new ArgumentException("Expression is incorrect");
            }

            var numberRegex = new Regex(@"[MDCLXVI]+");
            var romanNumbers = numberRegex.Matches(expression);
            var digitNumbers = new List<int>();
            foreach (var romanNumber in romanNumbers)
            {
                digitNumbers.Add(ConvertRomanToDigit(romanNumber.ToString()));
            }

            var operationRegex = new Regex(@"[\+\-\*]{2}");
            var operations = operationRegex.Matches(expression);
            var operationArray = new List<char>();
            foreach (var operation in operations)
            {
                operationArray.Add(Convert.ToChar(operation));
            }

            return Calculate(digitNumbers, operationArray);
        }


        private static int ConvertRomanToDigit(string romanNumbers)
        {
            var digitResult = 0;
            var convertor = new Dictionary<char, int>()
            {
                {'I', 1},
                {'V', 5},
                {'X', 10},
                {'L', 50},
                {'C', 100},
                {'D', 500},
                {'L', 1000},
            };
            if (romanNumbers.Length == 1) return convertor[romanNumbers[0]];
            var digitNumbers = new int[romanNumbers.Length];

            var iterator = 0;
            foreach (var romanNumber in romanNumbers)
            {
                digitNumbers[iterator] = convertor[romanNumber];
                iterator++;
            }
            
            for (var i = 0; i < digitNumbers.Length; i++)
            {
                if (digitNumbers[i] == 1 || digitNumbers[i] == 10)
                {
                    switch (digitNumbers[i].CompareTo(digitNumbers[i + 1]))
                    {
                        case -1:
                            digitResult += digitNumbers[i + 1] - digitNumbers[i];
                            i++;
                            break;
                        case 1:
                            digitResult += digitNumbers[i + 1] + digitNumbers[i];
                            i++;
                            break;
                        default:
                            digitResult += digitNumbers[i];
                            break;
                    }
                }
            }

            return digitResult;
        }

        private static int Calculate(List<int> numbers, List<char> operations)
        {
            while (operations.Any(x => x == '*'))
            {
                var item = operations.Find(x => x == '*');
                var index = operations.IndexOf(item);
                operations.RemoveAt(index);
                numbers[index] *= numbers[index + 1];
                numbers.RemoveAt(index+1);
            }

            if (operations.Count > 1)
            {
                for (int i = 0; i < operations.Count; i++)
                {
                    if (operations[i] == '+')
                        numbers[i + 1] += numbers[i];
                    else numbers[i + 1] = numbers[i] - numbers[i + 1];
                }
            }
            return numbers.Last();
        }
	}
}
