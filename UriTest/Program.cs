using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace UriTest
{
    class Program
    {
        static async Task<Dictionary<string,decimal>> GetExchangeRates(Uri uri)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(uri);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Rest API call failed");
            }
            var responseString = await response.Content.ReadAsStringAsync();

            JObject jObject = JObject.Parse(responseString);
            JObject jRates = (JObject)jObject["rates"];
            JToken valueBase = jRates.GetValue("EUR", StringComparison.InvariantCulture);
            decimal rateBase = 1;
            if (valueBase != null)
            {
                rateBase = Convert.ToDecimal(valueBase, CultureInfo.InvariantCulture);
            }

            Dictionary<string, decimal> rates = new Dictionary<string, decimal>();
            foreach (var jRate in jRates)
            {
                string currency = jRate.Key;
                decimal rate = Convert.ToDecimal(jRate.Value, CultureInfo.InvariantCulture);
                rate = rate / rateBase;
                rates.TryAdd(currency, rate);
            }
            return rates;
        }

        static async Task<int> Main(string[] args)
        {
            try
            {
                Uri uri = null;
                if(uri is null)
                {
                    Console.WriteLine("Null");
                } else
                {
                    Console.WriteLine("Not Null");
                }
                
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return 0;
        }
    }
}
