using LtdTH1.Data;
using LtdTH1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LtdTH1.Controllers;

public class SubjectController : Controller
{
    private readonly SchoolContext _db;

    public SubjectController(SchoolContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var subjects = await _db.Courses
            .AsNoTracking()
            .GroupBy(course => course.Title)
            .Select(group => new SubjectListItemViewModel
            {
                SubjectName = group.Key,
                CourseCount = group.Count()
            })
            .OrderBy(subject => subject.SubjectName)
            .ToListAsync();

        return View(subjects);
    }
}
