using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Dispatchers;
using Zero99Lotto.SRC.Common.Messages.Commands;
using Zero99Lotto.SRC.Common.Messages.Queries;

namespace Zero99Lotto.SRC.Common.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public BaseController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public BaseController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public BaseController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        protected Task DispatchCommandAsync<TCommand>(TCommand command) where TCommand : ICommand
         => _commandDispatcher.DispatchAsync(command);

        protected Task<TResult> DispatchQueryAsync<TResult>(IQuery<TResult> query)
         => _queryDispatcher.DispatchAsync(query);

        protected ActionResult<T> Find<T>(T data)
        {
            if (data == null)
                return NotFound();
            return Ok(data);
        }
    }
}