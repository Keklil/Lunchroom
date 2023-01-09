using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using ClientV2.Apis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ClientV2.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailConfirmationController : ControllerBase
{
    private readonly IApiClientV2 _api;

    public EmailConfirmationController(IApiClientV2 api)
    {
        _api = api;
    }
    
    [HttpGet]
    public async Task<ActionResult> Post(string token)
    {
        var response = await _api.Auth_ConfirmEmailAsync(token);

        if (response is null)
            return RedirectToPage("/Account/EmailConfirmation", "Error");
        
        StreamReader streamReader = new StreamReader(response.Stream);
        var emailRaw = await streamReader.ReadToEndAsync();
        var email = string.Empty;
        JsonSerializer.Deserialize<Dictionary<string, string>>(emailRaw).TryGetValue("email", out email);
        
        
        return RedirectToPage("/Account/EmailConfirmation");
    }
}