using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FluentAssertions;
using Konfidence.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.Tests
{
    [TestClass]
    public class StringExtensionsBenchmark
    {
        [Benchmark]
        public void InitUpperCaseBenchmark()
        {
            // arrange
            const string word = "hello";
            const string expectedResult = "Hello";

            // act
            string result = word.InitUpperCase();

            // assert
            result.Should().Be(expectedResult);
        }

        // Additional benchmarks can be added here

        [TestMethod]
        [Ignore]
        public void RunBenchmark()
        {
            BenchmarkRunner.Run<StringExtensionsBenchmark>();
        }
    }
}