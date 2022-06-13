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
                    RenderDelay = 5000,
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
            
            Response.Cookies.Append("MyBigCookie.1", "Drs37wlyReqsQrRKwvjSg6vQ1M01Cpsw25Lk1h8AydQKlgMK9yrVfxI5XC54RPVwS3YDnmiSxHRDH9nOyxAIQASxfpOoOTeaFM9x2woQPDAaWnCFi3UKy1jQZ5nXGVnQbom7skXXlikK90niFzVphxStmy44tIi7kuCnkHKFziGCV1JQMkcoR8bbhBI6W6hh0stIJp5KgPiegUyjhgzb98NeFmuDfbXz1hv30dUx18AIkFMSuik3mLtxD1ICUOaHEnFUcxv1OkI4ikQxkILCN9Q7O0OXZ84rdNvBsyfQsnNBR11BekaM4fptpqHbjJidKghCkY5Lij48JsiSDHfLjMb6tiRO0CmCr9gFOOGlDEiVAKK3uZXvRzsZGJoXNYG7ZAjljNj60y3GfN9R3M5CiW242ijBFaC6vHspcNyWcqHwwubWKJAB74b0kWbPdTM6d6YnzFbOJPQHlDwaHRkyTAktFetf0dG55WYqA7SZD9I7wX8dqu3Q3HuTQusXDHMADwO9j36TNY7iHD3iAgK0qXANAf2jJGtHUZ1DFWdG1SRs5AmuBL5huspCs4zGrFEledWukuuz30Fmr9nUH3uAJRzssa75DVwVkBfsnb6321j0xMc4Qu1k7bPTWTotKnyjiYECqtjlWCTGe5IQcWe6ZJXNPos4TyDDn4FrXtDEATXpQvesjcx8j0E9sjUClMRmK0EIhKdWlrmiGTLxcdmrr4SSWGvt8RtY1UnxhsOmYzM3uB9m7nocCw9QW9fybZtSfJc7ERfJQJG5A6OHsVptQd3GxZytCexnqNAtHvBp7RTZ3aVEBcAjd60087beJge8BH4hDekJGgffbOWYpiBdxtm7fWj693vHv6MTPsbm5STuls8tUBTIQVDUnDGw2nBivtxgOovXokm4y6dk9HLIcGFkePHMgRIz9AlPqogGZkmWJtqfE4GowPqiSVKp7N4NYnQgOUDXD4SXodnqE35sOoAss4l3gVy8AyXIwJFQEE4U2KMZxunTxLh35neyOf0aWxu6m6KYW36zaUUOZKi6mZvNGQJaWyCNJyYHhNbKJLEg6kEjzStJaQRb38GR7FTZkpeJ7xq90fp6A1NZ6NNxv9SdOjjEn7kbsUtG5thr9DQSxngVLjUx15VYOiauKVOSV53a5klqhs6uKJSe9VTetz9NnTCL1j0wj3jgUVmNRt6yXpyRiq0nnGPKGwae9EsmsPztxH7VbhPpbqWBn92AH4FzjZ3iCL1a92Pzu5hEikCrk6pW5AYaD9T1YGYvls0Zsf7S1FLihxStfgAOI7DrYJNxPegtaQYhRNfCbEAMyxAMlxr0I");
            Response.Cookies.Append("MyBigCookie.2", "BLPJteejtVxWXzDK187rCuzTmkMGmt06X0thXeeQKzxgmqfOvw3QZndJFfpEsmzRj0wUGSxaJ1vCoJiNyGnpAnXxj4noV4nu1ADnAxTJ4uc0Eq6Sfiel3ZwgclIH9G7z4Drxy7svkLNRit60V1PepC2lO2xObqw4strGRq3BvD1fCz4KIS7i5bLwb55K8w5ibANmzcKYBJs2Coa9zuAZHj9gDJp1LImsIV767z7mHnmedVQf5YWc01x9FOR0L4xr1GWBI3rjqNrO6ttKiK0r7cxDFLDOB1pB2lhNKX02LvaNVPwaLoP2QFpuPlJAKZyKKh1mByQ01HlMg6kU77qk83Tk77xRPeJN8diGHZ9fxR5YNCn4DERS0MxZTOeBRqu6256bCaNunnJpiFx5aAHoahHvGdAQWrIc9hvIg8H0rIPbNYKVnvkF3eS9VzR0DnhW56DS3hqjePqkhavl6Jy0UmfX0kvgctFHaYNBGqKwxCXmqjLYONBZOu6SMGUKtEFwPGS6kPhh31fp39Ou3XOGJ77MNl9434IOj72qZC6b7fWF1ZLoABx2r8OPqyJjEQmRjwPIggYQOAeUXS3blVA7Kqx822FdJEqsyVzZYiO3SXrGU8ltElO4yt1oqUutn0fxqPqKfr6TAdOPaBV0Etj06jkbNq95HUh7egmszW5rZYTmk44RIQFPVEMh3lynnSmqKygkouFfseMJRbUcZmVN4xzOrkD0G9SpnR8wojF5ictRZNTXNI8yVe2u2fwqXNQ141ZhNKJtPtGh6ILzxQ4O3CIoccqC2VuYnv56zJ6tpNQHhqfBO4q6E24rrpr4yzTJgfVaMHr6YSAShVT5l8eg0AUo0ikuRxtVO1leGPgTnMjptiNygkG1LOqCvyxZwvJplMsZVwakFB8VSJdmQWsosQlU2dz2KRQUtGVB3L6ijizDCwpgt4sXykkeNYmVM8oT9brWtHvhxBgDBmy0zcsplpRA9kEtbOmDzPN7egKWYR5xEBKHwReoVFpS0lGEormo8aOt2cYO1WHVXl77ngSRUJZSAD2psR8edtVZd5izCOCrjKmKup7ZpJd37zjJJxOnNVzdgGhxpnrhdl9L4Zkrsf4nW8r3Svf9NqpFuAHhFmAr47wCtsPiJLSHqYOwDQuIkfhCPkLN1HMoxoYQB8djwG8BvzizReOeVTnQT7MWLzbTva8MJEqRnlz6hlxRgSinpFv7SgAaduRSd2EVQ0llZvCVQnEVW8Esx56jSqEoMgfl550wpaBcTSOTD7M619F9PWYpNIAhYD0hqUgPSEAKBAkBzg8xCAYwqUAdLISvpStnQraDX");

            return Ok(new { success = true });
        }
    }
}

