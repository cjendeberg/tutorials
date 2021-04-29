using Newtonsoft.Json;
using System;

namespace Zero99Lotto.SRC.Common.Messages.Commands.Identity
{
    public class SendFirstTimePlaySms : ICommand
    {
        public Guid UserId { get; }

        [JsonConstructor]
        public SendFirstTimePlaySms(Guid userId)
            => UserId = userId;
    }
}
