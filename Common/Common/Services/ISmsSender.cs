using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Services;

namespace Zero99Lotto.SRC.Common.Services
{
    public interface ISmsSender : IService
    {
        Task SendSmsAsync(string number, string message);
    }
}
