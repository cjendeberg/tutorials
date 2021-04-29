using System;
using System.Collections.Generic;
using System.Text;

namespace Zero99Lotto.SRC.Common.Messages.Queries
{
    /// <summary>
    /// Marker interface
    /// </summary>
    public interface IQuery
    {
    }

    public interface IQuery<TResult> : IQuery
    {
    }

    public interface IPagedQuery : IQuery
    {
        int PageIndex { get; }
        int ItemsPerPage { get; }
        //string OrderBy { get; }
    }
}
