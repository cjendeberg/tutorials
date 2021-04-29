using System;

namespace Zero99Lotto.SRC.Common.Messages.Events.PlayerProfile
{
    public class PlayerTransactionUpdated : IEvent
    {
        public Guid UserId { get; }
        public decimal Balance { get; }
        public decimal Amount { get; }
        public int GameId { get; }
        public bool Paid { get; }

        public PlayerTransactionUpdated(Guid userId,decimal amount, decimal balance, int gameId, bool paid)
        {
            Balance = balance;
            Amount = amount;
            GameId = gameId;
            Paid = paid;
            UserId = userId;
        }
    }
}
