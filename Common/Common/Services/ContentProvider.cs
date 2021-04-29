using jsreport.AspNetCore;
using jsreport.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Zero99Lotto.SRC.Common.Services
{
    public class ContentProvider : IContentProvider
    {
        private readonly IJsReportMVCService _jsReportMVCService;
        private IHttpContextAccessor _accessor;

        public ContentProvider(IJsReportMVCService jsReportMVCService, IHttpContextAccessor accessor)
        {
            _jsReportMVCService = jsReportMVCService;
            _accessor = accessor;
        }

        public async Task<(string ContentType, MemoryStream stream)> GenerateStream
            (string viewPath, object model, Recipe type)
        {
            var routeData = _accessor.HttpContext.GetRouteData();
            var jsReportFeature = new JsReportFeature(_accessor.HttpContext);
            jsReportFeature.Recipe(type);
            jsReportFeature.RenderRequest.Template.Content = await 
                RenderViewToStringAsync(_accessor.HttpContext, routeData, viewPath, viewPath, model)
                .ConfigureAwait(false);
            var report = await _jsReportMVCService.RenderAsync(jsReportFeature.RenderRequest)
                .ConfigureAwait(false);
            var contentType = report.Meta.ContentType;
            var memoryStream = new MemoryStream();
            report.Content.CopyTo(memoryStream);
            return (contentType, memoryStream);
        }

        public async Task<string> RenderViewToStringAsync(HttpContext context, string viewPath, object model)
            => await RenderViewToStringAsync(context, _accessor.HttpContext.GetRouteData(), viewPath, viewPath, model).ConfigureAwait(true);

        private async Task<string> RenderViewToStringAsync(HttpContext context, RouteData routeData,
            string executingViewPath, string viewPath, object model)
        {
            var actionContext = new ActionContext(context, routeData, new ActionDescriptor());
            using (var sw = new StringWriter())
            {
                var viewResult = ((IRazorViewEngine)context.RequestServices.GetService
                    (typeof(IRazorViewEngine))).GetView(executingViewPath, viewPath, false);

                if (viewResult.View == null)
                    throw new ArgumentNullException($"{viewPath} does not match any available view");

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, 
                        ((ITempDataProvider)context.RequestServices.GetService(typeof(ITempDataProvider)))),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext).ConfigureAwait(false);
                return sw.ToString();
            }
        }
    }
}
