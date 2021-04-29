using System;
using Zero99Lotto.SRC.Common.Messages.Events;

namespace Zero99Lotto.SRC.Common.Messages.Payment
{
    public class PaymentSucceeded : IEvent
    {  
        public decimal Value { get; }
        public Guid UserId { get; }
        public string OrderRef { get; }
        public Guid TransactionId { get; }
        public int Type { get; }

        public string CurrencyCode { get; }

        public PaymentSucceeded(Guid transactionId, decimal value, Guid userId, string orderRef, int type, string currencyCode)
        {
            TransactionId = transactionId;
            Value = value;
            UserId = userId;
            OrderRef = orderRef;
            Type = type;
            CurrencyCode = currencyCode;
        }
    }
}
