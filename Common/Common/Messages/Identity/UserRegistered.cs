using System;
using System.Collections.Generic;
using System.Text;
using Zero99Lotto.SRC.Common.Messages.Events;

namespace Zero99Lotto.SRC.Common.Messages.Identity
{
    public class UserRegistered : IEvent
    {
        public string UserIdentityId { get; }
        public string Country { get; }
        public string PromoCode { get; }

        public UserRegistered(string userIdentityId, string country, string promoCode = null)
        {
            Country = country;
            UserIdentityId = userIdentityId;
            PromoCode = promoCode;
        }
    }
}
