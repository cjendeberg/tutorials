using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zero99Lotto.SRC.Common.Settings
{
    public class SendGridSettings
    {
        public string ApiKey { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
}
