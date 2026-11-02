using Microsoft.AspNetCore.Mvc;

namespace LtdTH1.ViewComponents;

public class RenderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("RenderLeftMenu");
    }
}
