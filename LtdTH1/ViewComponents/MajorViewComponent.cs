using LtdTH1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LtdTH1.ViewComponents;

public class MajorViewComponent : ViewComponent
{
    private readonly SchoolContext _db;

    public MajorViewComponent(SchoolContext db)
    {
        _db = db;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var majors = await _db.Majors
            .AsNoTracking()
            .OrderBy(major => major.MajorName)
            .ToListAsync();

        return View("RenderMajor", majors);
    }
}
