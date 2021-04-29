using Autofac;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Handlers;
using Zero99Lotto.SRC.Common.Messages.Queries;

namespace Zero99Lotto.SRC.Common.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IComponentContext _context;

        public QueryDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _context.Resolve(handlerType);
            return await handler.Handle((dynamic)query);
        }
    }
}
