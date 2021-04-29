using System;

namespace Zero99Lotto.SRC.Common.Messages.Events.Game
{
    public class GameCreated : IEvent
    {
        public int GameId { get; }
        public Guid IdentityUserId { get; }
        public Guid DrawingId { get; }
        public int[] OrdNumbers { get; }
        public int[] ExtraNumbers { get; }
        public DateTime PlayedDate { get; }

        public GameCreated(int gameId, Guid identityUserId, Guid drawingId, 
            int[] ordNumbers, int[] extraNumbers, DateTime playedDate)
        {
            GameId = gameId;
            IdentityUserId = identityUserId;
            DrawingId = drawingId;
            OrdNumbers = ordNumbers;
            ExtraNumbers = extraNumbers;
            PlayedDate = playedDate;
        }
    }
}