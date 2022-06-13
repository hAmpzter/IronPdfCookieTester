using IronPdf;
using IronPdf.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace IronPdfCookieTester.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScriptsController : Controller
    {
        [HttpGet("/scripts/fetchInject")]
        public IActionResult FetchScript() => File($"~/Scripts/fetchInject.js", "application/javascript");
    }
}
