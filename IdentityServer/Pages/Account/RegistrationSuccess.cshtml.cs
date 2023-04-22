
using IdentityServerHost.Pages.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerHost.Pages.Account;

[SecurityHeaders]
[AllowAnonymous]
public class RegistrationSuccess : PageModel
{
    [BindProperty] public InputModel Input { get; set; }
    public string ReturnUrl = "";
    
    public void OnGet(string returnUrl)
    {
        ReturnUrl = returnUrl;
    }
}