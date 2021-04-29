using System;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SendRegisterCodeSMS
{
  class Program
  {
    static async Task Main(string[] args)
    {

      if (args.Length == 2)
      {
        var command = new SendRegisterCodeSMS()
        {
          UserName = "JGWUHH3X4GVN",
          Password = "0goRIMPrSHWzyVBA9c2QnLp6",
          Originator = "099LOTTO",
          Recipients = new string[] { args[0] },
          MessageText = args[1]
        };

        string url = "https://json.aspsms.com/SendTextSMS";
        string serializedCommand = JsonConvert.SerializeObject(command);
        var commandContent = new StringContent(serializedCommand, Encoding.UTF8, "application/json");
        //var client = new HttpClient();
        //await client.PostAsync(url, commandContent);
      }
    }
  }
}
