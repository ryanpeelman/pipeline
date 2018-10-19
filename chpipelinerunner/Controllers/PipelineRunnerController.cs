using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace chpipelinerunner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PipelineRunnerController : ControllerBase
    {
        private Dictionary<string, Func<object, object>> _pipelineRoutes;

        public PipelineRunnerController()
        {
            _pipelineRoutes = new Dictionary<string, Func<object, object>>
            {
                { "log", (data) => { LogService.Instance.Log(data); return data; } },
                { "normalize", (data) => { return NormalizationService.Instance.NormalizeDate(data); }}
            };
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] DeviceDataMetadataPipelineMapPayload payload)
        {
            var data = payload.Data;

            var routes = payload.Map.Routes;

            List<Func<object, object>> pipeline = new List<Func<object, object>>();
            foreach (var route in routes)
            {
                var path = route.Path;

                if (_pipelineRoutes.ContainsKey(path))
                {
                    var f = _pipelineRoutes[path];
                    pipeline.Add(f);
                }
                else
                {
                    var f = BuildRouteRequestFunction(route);
                    pipeline.Add(f);
                }
            }

            foreach (Func<object, object> pipe in pipeline)
            {
                var result = pipe.Invoke(data);
                if (result is BadRequestObjectResult)
                {
                    Console.WriteLine((result as BadRequestObjectResult).Value);
                    continue;
                }

                data = result;
            }

            return Ok("pipeline completed");
        }

        private Func<object, object> BuildRouteRequestFunction(PipelineRoute route)
        {
            Func<object, object> f = (data) =>
            {
                var payload = new
                {
                    data = data,
                    parameters = route.Parameters
                };

                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(route.BaseUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var response = client.PostAsJsonAsync(route.Path, payload).Result;
                        response.EnsureSuccessStatusCode();

                        object result = response.Content.ReadAsAsync<object>().Result;
                        return Ok(result);
                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        return BadRequest($"Error submitting data to {route.Path}: {httpRequestException.Message}");
                    }
                }
            };

            return f;
        }
    }
}
