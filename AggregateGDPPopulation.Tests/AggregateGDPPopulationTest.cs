using System;
using Xunit;
using AggregateGDPPopulation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Class1.AggregateCalcualtion();

            var Actual = JObject.Parse(File.ReadAllText(@"../../../output.json"));
            var Expected = JObject.Parse(File.ReadAllText(@"../../../expected-output.json"));

            Assert.Equal(Expected, Actual);
        }
    }
}
