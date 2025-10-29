using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace GlideBuy.Pages.Admin
{
	[Authorize]
	public class IdentityUsersModel : PageModel
	{
		private UserManager<IdentityUser> userManager;

		public IdentityUsersModel(UserManager<IdentityUser> mgr)
		{
			userManager = mgr;
		}

		public IdentityUser? AdminUser { get; set; } = new();

		public async Task OnGetAsync()
		{
			AdminUser = await userManager.FindByNameAsync("Admin");
		}
	}
}
