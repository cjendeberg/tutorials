using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
//using Microsoft.Extensions.Configuration.Json;

namespace Settings
{
  class Program
  {
    static void Main(string[] args)
    {
      var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appSettings.json")
        .AddAzureKeyVault(new Uri("https://qa-099lotto-kv.vault.azure.net/"), new DefaultAzureCredential())
        .AddEnvironmentVariables()
        .Build();
      Console.WriteLine(configuration["SQLSERVERPASSWORD"]);
      SqlSettings settings = new SqlSettings();
      configuration.GetSection("SQL").Bind(settings);
      Console.WriteLine(settings.ConnectionString);
    }
  }
}
