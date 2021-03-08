using NUnit.Framework;
using RomanMath.Impl;

namespace RomanMath.Tests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void Test1()
		{
			var result = Service.Evaluate("IV+II/V");
			Assert.AreEqual(0, result);
		}
	}
}