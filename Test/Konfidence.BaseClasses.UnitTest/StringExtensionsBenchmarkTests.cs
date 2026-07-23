using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FluentAssertions;
using Konfidence.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.UnitTest
{
    [TestClass]
    public class StringExtensionsBenchmark
    {
        [Benchmark]
        public void InitUpperCaseBenchmark()
        {
            // Arrange
            const string word = "hello";
            const string expectedResult = "Hello";

            // Act
            string result = word.InitUpperCase();

            // Assert
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