using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using PowerLib;

namespace PowerServiceAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class productionplanController : ControllerBase
    {
        private const string PowerServiceAPI_HostAddress = "https://localhost:8888";

        [HttpPost]
        public dynamic Post([FromBody] PowerLib.PowerSystem request)
        {
            return Calculator.Calculate(request);

        }

    }
}
