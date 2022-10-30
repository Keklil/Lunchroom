using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Client.Apis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailConfirmationController : ControllerBase
{
    private readonly IApiClient _api;

    public EmailConfirmationController(IApiClient api)
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
        
        //Auto-auth after confirm
        var login = new UserLogin(){ Email = email };
        
        var authResult = await _api.Auth_AuthAsync(login);

        HttpContext.Session.SetString("Token", authResult.Token);
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var encodeToken = tokenHandler.ReadJwtToken(authResult.Token);
        
        var userId = encodeToken.Claims.First(claim => claim.Type == "UserID").Value;
        //HttpContext.Session.SetString("UserId", userId);
        //HttpContext.Session.SetString("Email", email);
        var role = encodeToken.Claims.First(claim => claim.Type == "role").Value;
        //HttpContext.Session.SetString("Role", role);
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Email, email),
            new Claim("UserId", userId),
            new Claim("Token", authResult.Token)
        };
        var identity = new ClaimsIdentity( claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var authProperties = new AuthenticationProperties()
        {
            IsPersistent = true
        };
        
        await HttpContext.SignInAsync( CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
        
        return RedirectToPage("/Account/EmailConfirmation");
    }
}