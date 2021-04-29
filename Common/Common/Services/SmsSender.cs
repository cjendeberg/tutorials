using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Settings;
using Zero99Lotto.SRC.Common.Messages.Commands.Identity;

namespace Zero99Lotto.SRC.Common.Services
{
  public class SmsSender : ISmsSender
  {
    private readonly ILogger<SmsSender> _logger;
    private readonly AspSmsSettings _aspsmsSettings;
    private HttpClient _httpClient;

    public SmsSender(ILogger<SmsSender> logger,
        AspSmsSettings aspsmsSettings, HttpClient httpClient)
    {
      _logger = logger;
      _aspsmsSettings = aspsmsSettings;
      _httpClient = httpClient;
    }

    virtual public async Task SendSmsAsync(string number, string message)
    {
      try
      {
        _logger.LogInformation($"[{nameof(SmsSender)}] - Settings Userkey: {_aspsmsSettings.Userkey}");
        _logger.LogInformation($"[{nameof(SmsSender)}] - Settings Originator: {_aspsmsSettings.Originator}");
        _logger.LogInformation($"[{nameof(SmsSender)}] - Settings BaseUrl: {_aspsmsSettings.BaseUrl}");

        _logger.LogInformation($"[{nameof(SmsSender)}] - SMS: {number}, Message: {message}");


        var command = new SendRegisterCodeSms()
        {
          UserName = _aspsmsSettings.Userkey,
          Password = _aspsmsSettings.ApiPassword,
          Originator = _aspsmsSettings.Originator,
          Recipients = new string[] { number },
          MessageText = message
        };

        UriBuilder uri = new UriBuilder(_aspsmsSettings.BaseUrl);
        uri.Path = "SendTextSMS";
        
        var commandContent = new StringContent(JsonConvert.SerializeObject(command), System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(uri.Uri, commandContent);
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
          string logMessage = $"[{nameof(SmsSender)}] - Status : {response.StatusCode} Reason :{response.ReasonPhrase} - {response.Content.ReadAsStringAsync()}";
          _logger.LogWarning(logMessage);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"[{nameof(SmsSender)}]");
      }

    }
  }
}
