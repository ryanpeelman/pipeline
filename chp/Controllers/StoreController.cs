using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace chp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        // POST api/values
        [HttpPost]
        public void Post([FromBody] StorePayload payload)
        {
            var repository = StorageRepositoryFactory.Instance.Create(payload.Parameters);
            if (repository != null)
            {
                repository.Store(payload.Data);
            }
        }
    }
}
