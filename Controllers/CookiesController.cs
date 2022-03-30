using Microsoft.AspNetCore.Mvc;

namespace IronPdfCookieTester.Controllers;

[ApiController]
[Route("[controller]")]
public class CookiesController : ControllerBase
{
    private readonly ILogger<CookiesController> _logger;

    public CookiesController(ILogger<CookiesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<CookieViewModel> Get()
    {
        return Request.Cookies.Select((pair, index) => new CookieViewModel
        {
            Index = index,
            Key = pair.Key,
            Value = pair.Value
        })
        .ToArray();
    }
}
