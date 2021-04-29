using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Handlers;
using Zero99Lotto.SRC.Common.Messages.Commands;

namespace Zero99Lotto.SRC.Common.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _context;

        public CommandDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
            => await _context.Resolve<ICommandHandler<TCommand>>().HandleAsync(command);
    }
}
