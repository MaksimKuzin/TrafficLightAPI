using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using TrafficLightAPI.Contracts;
using TrafficLightAPI.Models;

namespace TrafficLightAPI.Controllers
{
    [ApiController]
    public class TrafficLightController : ControllerBase
    {
        private readonly ISequenceLogic _sequenceLogic;

        public TrafficLightController(ISequenceLogic sequenceLogic) { _sequenceLogic = sequenceLogic; }

        [HttpPost("sequence/create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var id = await _sequenceLogic.CreateSequence();

                return Ok(new
                {
                    status = "ok",
                    response = new { sequence = id }
                });
            }

            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = "error",
                    msg = ex.Message
                });
            }
        }
        [HttpPost("observation/add")]
        public async Task<IActionResult> Add([FromBody] ObservationRequest observationRequest)
        {
            try
            {
                var response = await _sequenceLogic.AddObservation(observationRequest);

                return Ok(new { staus = "ok", response });
            }

            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = "error",
                    msg = ex.Message
                });
            }
        }
        [HttpGet("clear")]
        public async Task<IActionResult> Clear()
        {
            try
            {
                await _sequenceLogic.ClearData();

                return Ok(new { status = "ok", response = "ok" });
            }

            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = "error",
                    msg = ex.Message
                });
            }
        }
    }
}
