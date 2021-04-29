using System;

namespace DemoApp
{
  // This interface decouples the notion of writing
  // a date from the actual mechanism that performs
  // the writing. Like with IOutput, the process
  // is abstracted behind an interface.
  public interface IDateWriter
  {
    void WriteDate();
  }
}