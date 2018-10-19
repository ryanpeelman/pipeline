using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace chpipelinebroker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PipelineBrokerController : ControllerBase
    {
        private readonly string RUNNER_BASE_URL;
        private readonly string RUNNER_ROUTE;

        public PipelineBrokerController()
        {
            RUNNER_BASE_URL = Environment.GetEnvironmentVariable("RUNNER_BASE_URL");
            RUNNER_ROUTE = Environment.GetEnvironmentVariable("RUNNER_ROUTE");
        }

        // POST api/pipelinebroker
        [HttpPost]
        public IActionResult Post([FromBody] DeviceDataPayload payload)
        {
            var outgoingPayload = BuildPayload(payload);

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(RUNNER_BASE_URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PostAsJsonAsync(RUNNER_ROUTE, outgoingPayload).Result;
                    response.EnsureSuccessStatusCode();

                    object result = response.Content.ReadAsStringAsync().Result;
                    return Ok(result);
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error submitting data to pipeline runner: {httpRequestException.Message}");
                }
                catch (Exception exception)
                {
                    return BadRequest($"Error submitting data to pipeline runner: {exception.Message}");
                }
            }
        }

        private DeviceDataMetadataPipelineMapPayload BuildPayload(DeviceDataPayload incomingPayload)
        {
            var patient = PatientRepository.Instance.GetPatientByDeviceId(incomingPayload.DeviceId);
            var map = PipelineMapRepository.Instance.GetPipelineMapByPatientId(patient.Id);

            var outgoingPayload = new DeviceDataMetadataPipelineMapPayload
            {
                Data = incomingPayload.Data,
                DataType = incomingPayload.DataType,
                DeviceId = incomingPayload.DeviceId,
                Map = map,
                PatientId = patient.Id,
                StudyId = patient.StudyId
            };

            return outgoingPayload;
        }
    }
}
