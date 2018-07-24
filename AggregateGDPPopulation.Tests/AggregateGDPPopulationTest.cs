using System;
using Xunit;
using AggregateGDPPopulation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1Async()
        {
            Task<string> result1 = Class1.AggregateCalculation();
            string result = await result1;
            Class1.WriterAsync(@"../../../output.json", result);
            Task<string>  actual = Class1.ReaderAsync(@"../../../output.json");
            Task<string>  expected = Class1.ReaderAsync(@"../../../expected-output.json");
            string Actual = await actual;
            string Expected = await expected;


            //var Actual = JObject.Parse(File.ReadAllText(@"../../../output.json"));
            //var Expected = JObject.Parse(File.ReadAllText(@"../../../expected-output.json"));

            Assert.Equal(JObject.Parse(Expected), JObject.Parse(Actual));
        }
    }
}
