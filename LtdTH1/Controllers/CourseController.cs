using LtdTH1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LtdTH1.Controllers;

public class CourseController : Controller
{
    private readonly SchoolContext _db;

    public CourseController(SchoolContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var courses = await _db.Courses
            .AsNoTracking()
            .Include(course => course.Enrollments)
            .OrderBy(course => course.CourseID)
            .ToListAsync();

        return View(courses);
    }
}
