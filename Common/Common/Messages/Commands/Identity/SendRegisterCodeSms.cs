using System;

namespace Zero99Lotto.SRC.Common.Messages.Commands.Identity
{
  public class SendRegisterCodeSms : ICommand
  {
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Originator { get; set; }
    public string[] Recipients { get; set; }
    public string MessageText { get; set; }
  }
}