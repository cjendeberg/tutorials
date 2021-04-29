using Microsoft.AspNetCore.Builder;

namespace Zero99Lotto.SRC.Common.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Invoke before UseStaticFiles(), UseExceptionHandler()!
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePathBaseIfExists(this IApplicationBuilder app, string pathBase)
        {
            if (!string.IsNullOrWhiteSpace(pathBase))
                app.UsePathBase(pathBase);
            return app;
        }
    }
}
