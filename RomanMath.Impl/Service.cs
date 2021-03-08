using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;

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

            if (Regex.IsMatch(expression, @"[^\+\-\*MDCLXVI]") ||
                Regex.IsMatch(expression, @"[IX]{2,}[MDCLV]") ||
                Regex.IsMatch(expression, @"[VLD]{2,}") ||
                Regex.IsMatch(expression, @"[IXCM]{4,}") ||
                Regex.IsMatch(expression, @"[\+\-\*]{2}") ||
                !Regex.IsMatch(expression, @"^(?:[MDCLXVI]+[\+\-\*])+[MDCLXVI]$")) throw new ArgumentException("Expression is incorrect");


            var numberRegex = new Regex(@"[MDCLXVI]+");
            var romanNumbers = numberRegex.Matches(expression);
            var digitNumbers = new List<int>();
            foreach (var romanNumber in romanNumbers)
            {
                digitNumbers.Add(ConvertRomanToDigit(romanNumber.ToString()));
            }

            var operationRegex = new Regex(@"[\+\-\*]");
            var operations = operationRegex.Matches(expression);
            var operationArray = new List<char>();
            foreach (var operation in operations)
            {
                var item = operation.ToString();
                operationArray.Add(item[0]);
            }

            return Calculate(digitNumbers, operationArray);
        }


        private static int ConvertRomanToDigit(string romanNumbers)
        {
            var convertor = new Dictionary<char, int>()
            {
                {'I', 1},
                {'V', 5},
                {'X', 10},
                {'L', 50},
                {'C', 100},
                {'D', 500},
                {'M', 1000},
            };
            if (romanNumbers.Length == 1) return convertor[romanNumbers[0]];

            var digitNumbers = new List<int>();

            foreach (var romanNumber in romanNumbers)
            {
                digitNumbers.Add(convertor[romanNumber]);
            }
            
            for (var i = 0; i < digitNumbers.Count-1; i++)
            {

                if (digitNumbers[i].CompareTo(digitNumbers[i + 1]) < 0 && (digitNumbers[i] == 1 || digitNumbers[i] == 10))
                {
                    digitNumbers[i + 1] = digitNumbers[i + 1] - digitNumbers[i];
                }
                else
                {
                    digitNumbers[i + 1] += digitNumbers[i];
                }
            }

            return digitNumbers.Last();
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

            var count = 0;
            while (operations.Count > count)
            {
                if (operations[count] == '+')
                    numbers[count + 1] += numbers[count];
                else numbers[count + 1] = numbers[count] - numbers[count + 1];
                count++;
            }
            return numbers.Last();
        }
	}
}
