using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zero99Lotto.SRC.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PreventDuplicateRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionDescriptor = ((Microsoft.AspNetCore.Mvc.ControllerBase)context.Controller)
                                   .ControllerContext
                                   .ActionDescriptor;
            var token = context.HttpContext.Request.Headers["Authorization"];
            context.HttpContext.Session.SetString($"{actionDescriptor.ControllerName}_{actionDescriptor.ActionName}_{token}", "Processing submit!");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var actionDescriptor = ((Microsoft.AspNetCore.Mvc.ControllerBase)context.Controller)
                                   .ControllerContext
                                   .ActionDescriptor;
            var token = context.HttpContext.Request.Headers["Authorization"];
            context.HttpContext.Session.Remove($"{actionDescriptor.ControllerName}_{actionDescriptor.ActionName}_{token}");
        }
    }
}
