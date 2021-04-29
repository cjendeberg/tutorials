using System.Collections.Generic;
using System.Linq;

namespace Zero99Lotto.SRC.Common.Messages.Queries
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; }
        public PageDetails PageDetails { get; }

        protected PagedResult() => Items = Enumerable.Empty<T>();

        protected PagedResult(IEnumerable<T> items, PageDetails pageDetails /*int pageIndex, int itemsPerPage, int totalPages, int totalItems*/)
        {
            Items = items;
            PageDetails = pageDetails;
        }

        public static PagedResult<T> Create(IEnumerable<T> items, PageDetails pageDetails)
            => new PagedResult<T>(items, pageDetails);
    }
}
