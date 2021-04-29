using System;
using System.Collections.Generic;
using System.Text;
using Zero99Lotto.SRC.Common.Messages.Events;

namespace Zero99Lotto.SRC.Common.Messages.Payment
{
    public class CreatePaymentFailed : IEvent
    {
        public string OrderRef { get; }
        public Guid TransactionId { get; }
        public Guid UserId { get; }
        public int Type { get; }

        public CreatePaymentFailed(Guid transactionId, Guid userId, string orderRef, int type)
        {
            TransactionId = transactionId;
            UserId = userId;
            OrderRef = orderRef;
            Type = type;
        }
    }
}
