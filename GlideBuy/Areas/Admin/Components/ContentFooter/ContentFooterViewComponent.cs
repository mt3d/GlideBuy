using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Areas.Admin.Components
{
    public class ContentFooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
