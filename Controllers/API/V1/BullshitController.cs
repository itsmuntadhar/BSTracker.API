using BSTracker.DTOs.Requests;
using BSTracker.Entities;
using BSTracker.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace BSTracker.Controllers.API.V1
{
    [Route("/api/v1/bullshits")]
    [Produces(Application.Json)]
    [Consumes(Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class BullshitController : ControllerBase
    {
        private readonly IBullshitService _service;

        public BullshitController(IBullshitService service)
            => _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<Bullshit>> GetAction([FromQuery] int offset = 0, [FromQuery] string whoSaidIt = "")
            => Perform(() => Ok(_service.Get(offset)));

        [HttpGet("{id}")]
        public ActionResult<Bullshit> GetSingleAction([FromRoute] string id)
            => Perform(() =>
            {
                var bullshit = _service.Get(id);
                if (bullshit is null)
                    return NotFound();
                return Ok(bullshit);
            });

        [HttpGet("stats")]
        public ActionResult<Dictionary<string, int>> GetStatsAction()
            => Perform(() =>
            {
                Dictionary<string, int> stats = _service.GetStats();
                return Ok(stats);
            });

        [HttpPost]
        public ActionResult<Bullshit> CreateBullshitAction([FromBody] NewBullshit dto)
            => Perform(() =>
            {
                var bullshit = _service.Create(dto);
                return Created($"api/v1/bullshits/{bullshit.Id}", bullshit);
            });

        private ActionResult Perform(Func<ActionResult> func)
        {
            try
            {
                return func.Invoke();
            }
            catch (BadHttpRequestException ex)
            {
                if (ex.StatusCode == StatusCodes.Status404NotFound)
                    return NotFound();
                throw ex;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                if (ex.InnerException is not null)
                    message += ex.InnerException.Message;
                return BadRequest(message);
            }
        }
    }
}
