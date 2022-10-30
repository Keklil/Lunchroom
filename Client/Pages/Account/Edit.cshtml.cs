using System.ComponentModel.DataAnnotations;
using Client.Apis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Account;

[Authorize]
public class Edit : PageModel
{
    private readonly IApiClient _api;
    private readonly IHttpContextAccessor _accessor;
    
    public UserDto User { get; set; }
    
    [Required(ErrorMessage = "Обязательное поле")]
    [RegularExpression(@"[А-Яа-я]+", ErrorMessage = "Допустимы только символы кирилицы")]
    [BindProperty, MaxLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
    public string NameForm { get; set; }
    
    [Required(ErrorMessage = "Обязательное поле")]
    [RegularExpression(@"[А-Яа-я]+", ErrorMessage = "Допустимы только символы кирилицы")]
    [BindProperty, MaxLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
    public string SurnameForm { get; set; }
    
    
    
    public Edit(IApiClient api, IHttpContextAccessor accessor)
    {
        _api = api;
        _accessor = accessor;
    }
    
    public async Task<ActionResult> OnGetAsync()
    {
        var userIdFromContext = _accessor.HttpContext.User.Claims.First(x => x.Type == "UserId").Value;
        var userId = Guid.Parse(userIdFromContext);
        User = await _api.User_GetUserAsync(userId);
        
        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();
        
        var user = new UserForCreationDto()
        {
            Name = NameForm,
            Surname = SurnameForm,
            Patronymic = ""
        };
        var userIdFromContext = _accessor.HttpContext.User.Claims.First(x => x.Type == "UserId").Value;
        var userId = Guid.Parse(userIdFromContext);
        
        User = await _api.User_UpdateUserAsync(userId, user);
        
        return RedirectToPage("/Index");
    }
}