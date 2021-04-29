using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zero99Lotto.SRC.Common.Controllers;
using Zero99Lotto.SRC.Common.Dispatchers;
using Zero99Lotto.SRC.Common.Messages.Commands.Draw;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.DTO;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Messages.Queries;

namespace Draw.API.Controllers
{
    [ApiController]
    [Route("api/v1/draws")]
    [Produces("application/json")]
    public class DrawController : BaseController
    {
        public DrawController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(commandDispatcher, queryDispatcher)
        {
        }

        [HttpGet("active")]
        public async Task<ActionResult<DrawingDTO>> GetActiveDrawing([FromRoute] GetActiveDrawing query)
            => Find(await DispatchQueryAsync(query));

        [HttpGet("schedule")]
        public async Task<ActionResult<ScheduleDTO>> GetSchedule([FromRoute] GetSchedule query)
            => Find(await DispatchQueryAsync(query));

        [HttpPost]
        [Route("createschedule")]
        public async Task<ActionResult> CreateSchedule(CreateSchedule command)
        {
          await DispatchCommandAsync(command);

          return CreatedAtAction(nameof(GetSchedule), new { });
        }

        [HttpPost]
        [Route("updateschedule")]
        public async Task<ActionResult> UpdateSchedule(UpdateSchedule command)
        {
            await DispatchCommandAsync(command);

            return Ok();
        }

        [HttpPost]
        [Route("updatedrawing")]
        public async Task<ActionResult> UpdateDrawing(UpdateDrawing command)
        {
            await DispatchCommandAsync(command);

            return Ok();
        }
    }
}