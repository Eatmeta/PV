using System.Security.Claims;
using Microsoft.AspNetCore.Components;

namespace BlazorServerApp.Pages;

public class UserPageBase : ComponentBase
{
    public string ErrorMessage { get; set; }
    public IEnumerable<Claim>? Claims;
    [Inject] public IHttpContextAccessor HttpContextAccessor { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Claims = HttpContextAccessor.HttpContext?.User.Claims;
        /*userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        D = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
        X = JsonConvert.SerializeObject(HttpContextAccessor.HttpContext?.User.Claims, Formatting.Indented,
            new JsonSerializerSettings() {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
        Claims = JsonConvert.SerializeObject(HttpContextAccessor.HttpContext?.User.Identities, Formatting.Indented,
            new JsonSerializerSettings() {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
        Claims = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.)
        var xX = HttpContextAccessor.HttpContext?.User.Identities.First(i =>
            i.Claims.First(c => c.Properties.First(p => p.Key == "family_name").Value));*/
    }
}