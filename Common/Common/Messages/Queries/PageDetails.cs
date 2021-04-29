using System;
using System.Collections.Generic;
using System.Text;

namespace Zero99Lotto.SRC.Common.Messages.Queries
{
    public class PageDetails
    {
        public int PageIndex { get; }
        public int ItemsPerPage { get; }
        public int TotalPages { get; }
        public int TotalItems { get; }

        protected PageDetails()
        {
        }

        protected PageDetails(int pageIndex, int itemsPerPage, int totalPages, int totalItems)
        {
            PageIndex = pageIndex;
            ItemsPerPage = itemsPerPage;
            TotalPages = totalPages;
            TotalItems = totalItems;
        }

        public static PageDetails Create(int pageIndex, int itemsPerPage, int totalPages, int totalItems)
            => new PageDetails(pageIndex, itemsPerPage, totalPages, totalItems);
    }
}
