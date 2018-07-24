using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregateGDPPopulation
{

    public static class Class1
    { 

        public static async void WriterAsync(string outputpath, string result)
        {
            using (StreamWriter srWrite = new StreamWriter(outputpath))
            {
                await srWrite.WriteLineAsync(result);
            }
        }

        public static async Task<string> AggregateCalculation()
        {
            string filepath = @"../../../../AggregateGDPPopulation/data/datafile.csv";
            string filepath1 = @"../../../../AggregateGDPPopulation/data/country-continent-map.json";
            Task<string> task1 = ReaderAsync(filepath);
            Task<string> task2 = ReaderAsync(filepath1);
            string data1 = await task1;
            string data2 = await task2;
            string[] data = data1.Replace("\"", "").Trim().Split('\n');
            JObject CCMap = JObject.Parse(data2);
            Dictionary<string, string> countryContinetMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(CCMap.ToString());
            Dictionary<string, OutputObject> output = new Dictionary<string, OutputObject>();
            string[] header = data[0].Split(',');
            int IndexOfCountry = Array.IndexOf(header, "Country Name");
            int IndexOfPopulation = Array.IndexOf(header, "Population (Millions) 2012");
            int IndexOfGdp = Array.IndexOf(header, "GDP Billions (USD) 2012");

            data = data.Skip(1).ToArray();

            foreach (string x in data)
            {
                string[] rowData = x.Split(',');
                    if (rowData[IndexOfCountry] != "European Union")
                    {

                        if (!output.ContainsKey(countryContinetMap[rowData[IndexOfCountry]]))
                        {
                            output.Add(countryContinetMap[rowData[IndexOfCountry]], new OutputObject() { POPULATION_2012 = float.Parse(rowData[IndexOfPopulation]), GDP_2012 = float.Parse(rowData[IndexOfGdp]) });
                        }
                        else
                        {
                            output[countryContinetMap[rowData[IndexOfCountry]]].POPULATION_2012 += float.Parse(rowData[IndexOfPopulation]);
                            output[countryContinetMap[rowData[IndexOfCountry]]].GDP_2012 += float.Parse(rowData[IndexOfGdp]);
                        }
                    }
                
            }
            var outputJsonString = JsonConvert.SerializeObject(output);
            return outputJsonString;
        }

       /* public static async Task<string> ReaderAsync(string filepath)
        {
            //StreamReader sr = new StreamReader()
            Task<string> x = Task.FromResult(File.ReadAllText(filepath));
            string y = await x;
            return y;
        }
        */
        public static async Task<string> ReaderAsync(string filepath)
        {
            //string data = File.ReadAllText(filepath);
            string data;
            using (StreamReader read = new StreamReader(filepath))
            {
                data = await read.ReadToEndAsync();
            }
            return data;
        }
    }

    public class OutputObject
    {
        public float GDP_2012 { get; set; }
        public float POPULATION_2012 { get; set; }
    }

    /*
    public static class Class1
    {
        public static void AggregateCalcualtion()
        {
            string[] data = File.ReadAllLines(@"../../../../AggregateGDPPopulation/data/datafile.csv", Encoding.UTF8);
            JObject CCmap = JObject.Parse(File.ReadAllText(@"../../../../AggregateGDPPopulation/data/country-continent-map.json", Encoding.UTF8));
            Dictionary<string, string> countryContinetMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(CCmap.ToString());
            Dictionary<string, OutputObject> output = new Dictionary<string, OutputObject>();

            string[] header = data[0].Replace("\"", "").Split(',');
            int IndexOfCountry = Array.IndexOf(header, "Country Name");
            int IndexOfPopulation = Array.IndexOf(header, "Population (Millions) 2012");
            int IndexOfGdp = Array.IndexOf(header, "GDP Billions (USD) 2012");

            data = data.Skip(1).ToArray();

            foreach (string x in data)
            {
                string[] rowData = x.Replace("\"", "").Split(',');

                if (rowData[IndexOfCountry] != "European Union")
                {
                    if (!output.ContainsKey(countryContinetMap[rowData[IndexOfCountry]]))
                    {
                        output.Add(countryContinetMap[rowData[IndexOfCountry]], new OutputObject() { POPULATION_2012 = float.Parse(rowData[4]), GDP_2012 = float.Parse(rowData[10]) });
                    }
                    else
                    {
                        output[countryContinetMap[rowData[IndexOfCountry]]].POPULATION_2012 += float.Parse(rowData[4]);
                        output[countryContinetMap[rowData[IndexOfCountry]]].GDP_2012 += float.Parse(rowData[10]);
                    }
                }
            }
            var outputJsonString = JsonConvert.SerializeObject(output);
            File.WriteAllText(@"../../../output.json", outputJsonString);
        }
    }
    public class OutputObject
    {
        public float GDP_2012 { get; set; }
        public float POPULATION_2012 { get; set; }
    }
    */
}
