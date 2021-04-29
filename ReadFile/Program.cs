using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Globalization;

class Test
{
    public static void Main()
    {
        string path = @"C:\git-alt\zero99lotto\tests\Services\Payment\Payment.Tests\Providers\MockData\EcbSample.xml";

        // This text is added only once to the file.
        if (File.Exists(path))
        {
            // Open the file to read from.
            string content = File.ReadAllText(path, Encoding.UTF8);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);                        
            XmlNodeList list = xmlDocument.SelectNodes("//*[@currency]");
            foreach(XmlElement item in list)
            {
                string rate = item.GetAttribute("rate");
                decimal decimalRate = decimal.Parse(rate, CultureInfo.InvariantCulture);
                Console.WriteLine($"{item.GetAttribute("currency")} : {decimalRate}");
            }
            
        }
    }
}