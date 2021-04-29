using System;
using System.Collections.Generic;
using System.Text;

namespace Zero99Lotto.SRC.Common.Messages.Events.Game
{
    public class GameDeleted : IEvent
    {
        public int GameId { get; }

        public GameDeleted(int gameId) => GameId = gameId;
    }
}
