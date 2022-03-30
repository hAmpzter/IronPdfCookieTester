using IronPdf;
using IronPdf.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace IronPdfCookieTester.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrintController : Controller
    {
        public async Task<IActionResult> Get()
        {
            var customCookies = Request.Cookies.ToDictionary(cookie => cookie.Key, cookie => cookie.Value);

            var renderUrl = Flurl.Url.Combine($"{Request.Scheme}://{Request.Host}{Request.PathBase}", "/fetch-data");
            
            ChromePdfRenderer renderer = new ChromePdfRenderer
            {
                RenderingOptions = new ChromePdfRenderOptions()
                {
                    CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen,
                    CreatePdfFormsFromHtml = false,
                    EnableJavaScript = true,
                    RenderDelay = 3000,
                    Timeout = 30,
                    PrintHtmlBackgrounds = true,
                    PaperOrientation = PdfPaperOrientation.Landscape
                },
                LoginCredentials = new ChromeHttpLoginCredentials()
                {
                    EnableCookies = true,
                    CustomCookies = customCookies
                }
            };

            var pdf = await renderer.RenderUrlAsPdfAsync(renderUrl);

            return new FileStreamResult(pdf.Stream, "application/pdf");
        }

        [HttpGet("/print/setCookie")]
        public IActionResult SetCookie()
        {
            Response.Cookies.Append("MyNiceMadeUpCookie", DateTime.UtcNow.ToString());

            return Ok(new { success = true });
        }
    }
}
