using System.IdentityModel.Tokens.Jwt;
using ClientV2.Apis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientV2.Pages.Team;

public class UploadMenu : PageModel
{
    private readonly IApiClientV2 _api;

    public PeriodicRefresh PeriodicRefresh { get; set; }

    [BindProperty] public Guid GroupId { get; set; }

    [BindProperty] public string MenuRaw { get; set; }

    public bool AlreadyUpload { get; set; }

    public async Task<ActionResult> OnGetAsync(string? token)
    {
        JwtSecurityToken encodeToken;
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            encodeToken = tokenHandler.ReadJwtToken(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }

        if (Guid.TryParse(encodeToken.Claims.First(claim => claim.Type == "GroupId").Value, out var groupId))
            NotFound();

        var groupInfo = await _api.Group_GetGroupAsync(groupId);
        PeriodicRefresh = groupInfo.Settings.PeriodicRefresh;
        GroupId = groupInfo.Id;
        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        var rawMenu = new RawMenuDto();
        rawMenu.GroupId = GroupId;
        rawMenu.Menu = MenuRaw.Split('\n').ToList();

        try
        {
            await _api.Menu_UploadMenuAsync(rawMenu);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            AlreadyUpload = true;
            return Page();
        }

        return Redirect("/Menu");
    }

    public UploadMenu(IApiClientV2 api)
    {
        _api = api;
    }
}