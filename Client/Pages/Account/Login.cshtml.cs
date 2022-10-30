using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Client.Apis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Account;

public class Login : PageModel
{
    private readonly IApiClient _api;

    [BindProperty]
    [RegularExpression(@"^[A-Za-z0-9._%+-]+@ussc\.ru$", ErrorMessage = "Почта должна принадлежать домену ussc.ru")]
    public string Email { get; set; }
    public Login(IApiClient api)
    {
        _api = api;
    }
    
    public void OnGet()
    {
        
    }
    
    public async Task<ActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        else
        {
            var login = new UserLogin(){ Email = Email };
        
            var authResult = await _api.Auth_AuthAsync(login);

            if (authResult.Token is null)
                return RedirectToPage("/Account/ConfirmationEmailSended");
        
            HttpContext.Session.SetString("Token", authResult.Token);
        
            var tokenHandler = new JwtSecurityTokenHandler();
            var encodeToken = tokenHandler.ReadJwtToken(authResult.Token);
        
            var userId = encodeToken.Claims.First(claim => claim.Type == "UserID").Value;
            //HttpContext.Session.SetString("UserId", userId);
            var email = encodeToken.Claims.First(claim => claim.Type == "email").Value;
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
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            
            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = true
            };
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
        
            return Redirect("/Index");
        }

    }
}