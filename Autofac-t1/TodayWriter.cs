using System;

namespace DemoApp
{
  // This TodayWriter is where it all comes together.
  // Notice it takes a constructor parameter of type
  // IOutput - that lets the writer write to anywhere
  // based on the implementation. Further, it implements
  // WriteDate such that today's date is written out;
  // you could have one that writes in a different format
  // or a different date.
  public class TodayWriter : IDateWriter
  {
    private IOutput _output;
    private IConfiguration _config;
    public TodayWriter(IOutput output)
    {
      this._output = output;
      this._config = null;
    }

    public TodayWriter(IOutput output, IConfiguration config)
    {
      this._output = output;
      this._config = config;
    }

    public void WriteDate()
    {      
      if (_config != null)
      {
        this._output.Write("Config found");
      } else
      {
        this._output.Write(DateTime.Today.ToShortDateString());
      }
    }
  }
}