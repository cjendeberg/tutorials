using System;
using System.Collections.Generic;
using System.Text;
using Zero99Lotto.SRC.Common.Messages.Events;

namespace Zero99Lotto.SRC.Common.Messages.Payment
{
    public class CreatePaymentSucceeded : IEvent
    {
        public Guid TransactionId { get; }
        public decimal Value { get; }
        public Guid UserId { get; }
        public string OrderRef { get; }
        public string CurrencyCode { get; }
        public int Type { get; }
        public int Status { get; }


        public CreatePaymentSucceeded(Guid transactionId, Guid userId, string orderRef, string currencyCode, int type, int status, decimal value=0)
        {
            TransactionId = transactionId;
            UserId = userId;
            OrderRef = orderRef;
            CurrencyCode = currencyCode;
            Type = type;
            Status = status;
            Value = value;
        }
    }
}
