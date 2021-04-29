using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace t2
{

  

  public class XMLWrite
  {
    public class Book
    {
      public String title;
    }

    static void Main(string[] args)
    {
      //WriteXML();
      ReadXML();
    }

    public static void WriteXML()
    {
      Book overview = new Book();
      overview.title = "Serialization Overview";
      System.Xml.Serialization.XmlSerializer writer =
          new System.Xml.Serialization.XmlSerializer(typeof(Book));

      var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationOverview.xml";
      System.IO.FileStream file = System.IO.File.Create(path);

      writer.Serialize(file, overview);
      file.Close();
    }

    public static void ReadXML()
    {
      // Now we can read the serialized book ...  
      System.Xml.Serialization.XmlSerializer reader =
          new System.Xml.Serialization.XmlSerializer(typeof(Book));

      string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\SerializationOverview.xml";
      Console.WriteLine(path);
      System.IO.StreamReader file = new System.IO.StreamReader(path);
      Book overview = (Book)reader.Deserialize(file);
      file.Close();

      Console.WriteLine(overview.title);
    }
  }
}