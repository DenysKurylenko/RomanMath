using System;
using RomanMath.Impl;

namespace RomanMath.Console
{
	class Program
	{
		/// <summary>
		/// Use this method for local debugging only. The implementation should remain in RomanMath.Impl project.
		/// See TODO.txt file for task details.
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
        {
            while (true)
            {
				var input = System.Console.ReadLine();
                var result = Service.Evaluate(input);
				System.Console.WriteLine(result);
			}
            
			System.Console.ReadKey();
		}
	}
}
