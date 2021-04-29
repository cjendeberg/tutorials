using System;

namespace Zero99Lotto.SRC.Common.Bus.Interfaces
{
    public interface IBusClient : IBusPublisher, IBusSubscriber, IDisposable
    {

    }
}
