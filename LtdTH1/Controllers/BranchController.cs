using LtdTH1.Models;
using Microsoft.AspNetCore.Mvc;

namespace LtdTH1.Controllers;

public class BranchController : Controller
{
    public IActionResult Index()
    {
        return View(Enum.GetValues<Branch>());
    }
}
