using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Services;

namespace Zero99Lotto.SRC.Common.Services
{
    public interface IEmailSender : IService
    {
        Task SendEmailAsync(string email, string subject, string message, string footer,
            List<string> ccs, string fileName = null, MemoryStream memoryStream = null);
    }
}
