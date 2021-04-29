using System;
using System.Reflection;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace t5
{
  
  class CreateSchedule
  {
    public int[] DaysOfWeek { get; set; }

    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan StartTime { get; set; }

  }  

  public class Program
  {
    public static void Main()
    {
      string jsonString = "{\"DaysOfWeek\": [1,2,3], \"StartTime\": \"14:00:00\"}";
      CreateSchedule cs = JsonSerializer.Deserialize<CreateSchedule>(jsonString);
      Console.WriteLine(cs.StartTime.ToString());
      string outputString = JsonSerializer.Serialize<CreateSchedule>(cs);
      Console.WriteLine(outputString);
      cs = JsonSerializer.Deserialize<CreateSchedule>(outputString);
      Console.WriteLine(cs.StartTime.ToString());
    }
  }

  internal class TimeSpanConverter : JsonConverter<TimeSpan>
  {
    public override TimeSpan Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      string timeString = reader.GetString();
      return TimeSpan.Parse(timeString);
    }

    public override void Write(
        Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
      writer.WriteStringValue(value.ToString());
    }
  }

}
