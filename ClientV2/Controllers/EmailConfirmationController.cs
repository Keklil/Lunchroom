using System.Text.Json;
using ClientV2.Apis;
using Microsoft.AspNetCore.Mvc;

namespace ClientV2.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailConfirmationController : ControllerBase
{
    private readonly IApiClientV2 _api;

    [HttpGet]
    public async Task<ActionResult> Post(string token)
    {
        var response = await _api.Auth_ConfirmEmailAsync(token);

        if (response is null)
            return RedirectToPage("/Account/EmailConfirmation", "Error");

        var streamReader = new StreamReader(response.Stream);
        var emailRaw = await streamReader.ReadToEndAsync();
        var email = string.Empty;
        JsonSerializer.Deserialize<Dictionary<string, string>>(emailRaw).TryGetValue("email", out email);


        return RedirectToPage("/Account/EmailConfirmation");
    }

    public EmailConfirmationController(IApiClientV2 api)
    {
        _api = api;
    }
}