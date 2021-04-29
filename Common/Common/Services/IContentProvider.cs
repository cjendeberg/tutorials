using jsreport.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Zero99Lotto.SRC.Common.Services
{
    public interface IContentProvider
    {
        Task<(string ContentType, MemoryStream stream)> GenerateStream(string viewPath, object model, Recipe type);
        Task<string> RenderViewToStringAsync(HttpContext context, string viewPath, object model);
    }
}
