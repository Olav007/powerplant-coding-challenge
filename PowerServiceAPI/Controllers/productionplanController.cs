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
        private Newtonsoft.Json.JsonSerializerSettings formattingJsonResult = new Newtonsoft.Json.JsonSerializerSettings
        {
            Formatting = Newtonsoft.Json.Formatting.Indented,
            Converters = new List<Newtonsoft.Json.JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() },
            FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.DefaultValue,
            FloatParseHandling = Newtonsoft.Json.FloatParseHandling.Double
        };
        [HttpPost]
        public dynamic Post([FromBody] PowerLib.PowerSystem request)
        {

            var res = (new Calculator(request).Calculate(request.load));
            return Newtonsoft.Json.JsonConvert.SerializeObject(res, formattingJsonResult);
        }

    }
}
