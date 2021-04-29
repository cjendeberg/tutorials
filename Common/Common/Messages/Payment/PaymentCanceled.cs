using System;
using System.Collections.Generic;
using System.Text;
using Zero99Lotto.SRC.Common.Messages.Events;

namespace Zero99Lotto.SRC.Common.Messages.Payment
{
    public class PaymentCanceled : IEvent
    {
        public decimal Value => 0;
        public Guid UserId { get; }
        public string OrderRef { get; }
        public Guid TransactionId { get; }
        public int Type { get; }

        public PaymentCanceled(Guid transactionId, Guid userId, string orderRef, int type)
        {
            TransactionId = transactionId;
            UserId = userId;
            OrderRef = orderRef;
            Type = type;
        }
    }
}
