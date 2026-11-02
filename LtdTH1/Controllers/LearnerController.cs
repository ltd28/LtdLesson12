using LtdTH1.Data;
using LtdTH1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LtdTH1.Controllers;

public class LearnerController : Controller
{
    private const int PageSize = 3;
    private readonly SchoolContext _db;

    public LearnerController(SchoolContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(int? mid)
    {
        IQueryable<Learner> learners = _db.Learners
            .Include(learner => learner.Major)
            .AsNoTracking();

        if (mid.HasValue)
        {
            learners = learners.Where(learner => learner.MajorID == mid.Value);
        }

        ViewBag.mid = mid;
        ViewBag.keyword = null;
        ViewBag.pageNum = (int)Math.Ceiling(await learners.CountAsync() / (double)PageSize);
        ViewBag.pageIndex = 1;

        var result = await learners
            .OrderBy(learner => learner.LearnerID)
            .Take(PageSize)
            .ToListAsync();

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> LearnerFilter(int? mid, string? keyword, int? pageIndex)
    {
        IQueryable<Learner> learners = _db.Learners
            .AsNoTracking()
            .Include(learner => learner.Major);

        if (mid.HasValue)
        {
            learners = learners.Where(learner => learner.MajorID == mid.Value);
        }

        keyword = keyword?.Trim();
        if (!string.IsNullOrEmpty(keyword))
        {
            var normalizedKeyword = keyword.ToLower();
            learners = learners.Where(learner => learner.FirstMidName.ToLower().Contains(normalizedKeyword));
        }

        var pageCount = (int)Math.Ceiling(await learners.CountAsync() / (double)PageSize);
        var page = Math.Max(pageIndex ?? 1, 1);
        if (pageCount > 0)
        {
            page = Math.Min(page, pageCount);
        }

        ViewBag.mid = mid;
        ViewBag.keyword = keyword;
        ViewBag.pageNum = pageCount;
        ViewBag.pageIndex = page;

        var result = await learners
            .OrderBy(learner => learner.LearnerID)
            .Skip(PageSize * (page - 1))
            .Take(PageSize)
            .ToListAsync();

        return PartialView("LearnerTable", result);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateMajorsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
    {
        if (ModelState.IsValid)
        {
            _db.Learners.Add(learner);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        await PopulateMajorsAsync(learner.MajorID);
        return View(learner);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var learner = await _db.Learners.FindAsync(id.Value);
        if (learner is null)
        {
            return NotFound();
        }

        await PopulateMajorsAsync(learner.MajorID);
        return View(learner);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
    {
        if (id != learner.LearnerID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _db.Learners.Update(learner);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Learners.AnyAsync(item => item.LearnerID == id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        await PopulateMajorsAsync(learner.MajorID);
        return View(learner);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var learner = await _db.Learners
            .Include(item => item.Major)
            .Include(item => item.Enrollments)
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.LearnerID == id.Value);

        return learner is null ? NotFound() : View(learner);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var learner = await _db.Learners
            .Include(item => item.Enrollments)
            .FirstOrDefaultAsync(item => item.LearnerID == id);

        if (learner is null)
        {
            return NotFound();
        }

        if (learner.Enrollments.Count > 0)
        {
            TempData["DeleteError"] = "Không thể xóa người học đã có đăng ký khóa học.";
            return RedirectToAction(nameof(Delete), new { id });
        }

        _db.Learners.Remove(learner);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateMajorsAsync(int? selectedMajorId = null)
    {
        ViewBag.MajorID = new SelectList(
            await _db.Majors.AsNoTracking().OrderBy(major => major.MajorName).ToListAsync(),
            nameof(Major.MajorID),
            nameof(Major.MajorName),
            selectedMajorId);
    }
}
