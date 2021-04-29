using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace t1
{
  class Program { 

    static async Task<int> Main(string[] args)
    {
      Guid scheduleId = Guid.NewGuid();
      int[] daysOfWeek = new int[] {0,1,2,3,4,5,6};
      TimeSpan startTime = new TimeSpan(13,0,0);
      UpdateSchedule command = new UpdateSchedule(scheduleId, daysOfWeek, startTime);
      var commandContent = new StringContent(JsonConvert.SerializeObject(command), System.Text.Encoding.UTF8, "application/json");      
      string result = await commandContent.ReadAsStringAsync();
      System.Console.WriteLine(result);
      return 0;
    }
  }
}