using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.ActionResults
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
