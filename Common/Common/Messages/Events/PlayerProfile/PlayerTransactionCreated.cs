using System;

namespace Zero99Lotto.SRC.Common.Messages.Events.PlayerProfile
{
    /// <summary>
    /// Transaction for game
    /// </summary>
    public class PlayerTransactionCreated : IEvent
    {
        public Guid UserId { get; }
        public decimal Balance { get; }
        public decimal Amount { get; }
        public int? GameId { get; }
        /// <summary>
        /// 0 - payment,
        /// 1 - deposit,
        /// 2 - withdrawal
        /// </summary>
        public int Type { get; }
        /// <summary>
        /// 0 - Pending,
        /// 1 - Succeeded,
        /// 2 - Failed,
        /// 3 - Canceled
        /// </summary>
        public int Status { get; }

        public PlayerTransactionCreated(Guid userId, decimal amount, decimal balance, int? gameId, int type, int status)
        {
            Balance = balance;
            Amount = amount;
            GameId = gameId;
            UserId = userId;
            Type = type;
            Status = status;
        }
    }
}
