using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ClientV2.Apis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientV2.Pages.Account;

public class Login : PageModel
{
    private readonly IApiClientV2 _api;

    [BindProperty]
    [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z]+\.[A-Za-z]+$", ErrorMessage = "Почта не соответствует формату.")]
    [MaxLength(50, ErrorMessage = "Превшено максимальное количество символов")]
    public string Email { get; set; }
    
    [BindProperty]
    [MaxLength(30, ErrorMessage = "Превшено максимальное количество символов")]
    public string Password { get; set; }

    public bool AuthError { get; set; }
    public Login(IApiClientV2 api)
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
            var login = new UserLogin(){ Email = Email, Password = Password};

            try
            {
                var authResult = await _api.Auth_AuthAsync(login);
                HttpContext.Session.SetString("Token", authResult);
        
                var tokenHandler = new JwtSecurityTokenHandler();
                var encodeToken = tokenHandler.ReadJwtToken(authResult);
        
                var userId = encodeToken.Claims.First(claim => claim.Type == "UserID").Value;
                var email = encodeToken.Claims.First(claim => claim.Type == "email").Value;
                var role = encodeToken.Claims.First(claim => claim.Type == "role").Value;

                var claims = new List<Claim>()
                {
                    new (ClaimTypes.Role, role),
                    new (ClaimTypes.Email, email),
                    new ("UserId", userId),
                    new ("Token", authResult)
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
            catch (ApiExceptionV2 e)
            {
                AuthError = true;
                return Page();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}