
namespace SendRegisterCodeSMS
{
  class SendRegisterCodeSMS : ICommand
  {
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Originator { get; set; }
    public string[] Recipients { get; set; }
    public string MessageText { get; set; }

  }
}