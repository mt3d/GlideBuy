using GlideBuy.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components.HeaderLinks
{
    public class HeaderLinksViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new HeaderLinksModel();

            return View(model);
        }
    }
}
