using LtdTH1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LtdTH1.Controllers
{
    [Route("Admin/Student")]
    public class StudentController : Controller
    {
        private List<Student> listStudents = new List<Student>();
        public StudentController()
        {
            //tao danh sach sinh vien voi 4 du lieu mau
            listStudents = new List<Student>
            {
                  new Student() { Id = 101, Name = "Hải Nam", Branch = Branch.IT,
                      Gender = Gender.Male, IsRegular=true,
                      Address = "A1-2018", Email = "nam@g.com" },
                  new Student() { Id = 102, Name = "Minh Tú", Branch = Branch.BE,
                      Gender = Gender.Female, IsRegular=true,
                      Address = "A1-2019", Email = "tu@g.com" },
                  new Student() { Id = 103, Name = "Hoàng Phong", Branch = Branch.CE,
                      Gender = Gender.Male, IsRegular=false,
                      Address = "A1-2020", Email = "phong@g.com" },
                  new Student() { Id = 104, Name = "Xuân Mai", Branch = Branch.EE,
                      Gender = Gender.Female, IsRegular = false,
                      Address = "A1-2021", Email = "mai@g.com" }
            };
        }
        [HttpGet]
        [Route("List",Name = "StudentList")]
        public IActionResult Index()
        {
            return View(listStudents);
        }
        [HttpGet]
        [Route("Add",Name = "StudentAdd")]
        public IActionResult Create()
        {
            //lay danh sach cac gia tri dender de hien thi radio button tren form
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            //lay danh sach cac gia tri Branch de hien thi select - option tren form
            //de hien thi select - option tren view can dung list<SelectListItem>
            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT", Value = "1"},
                new SelectListItem { Text = "BE", Value = "2"},
                new SelectListItem { Text = "CE", Value="3"},
                new SelectListItem { Text = "EE", Value="4"}
            };
            return View();
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult Create(Student s, IFormFile? Photo)
        {
            if (!ModelState.IsValid)
            {
                PopulateFormOptions();
                return View(s);
            }

            s.Id = listStudents.Count == 0 ? 101 : listStudents.Max(student => student.Id) + 1;
            if (Photo is { Length: > 0 })
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                Directory.CreateDirectory(folder);
                string extension = Path.GetExtension(Photo.FileName);
                string fileName = $"{Guid.NewGuid()}{extension}";
                string filePath = Path.Combine(folder, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                Photo.CopyTo(stream);
                s.Image = fileName;
            }

            listStudents.Add(s);
            return View("Index", listStudents);
        }

        private void PopulateFormOptions()
        {
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT", Value = "1" },
                new SelectListItem { Text = "BE", Value = "2" },
                new SelectListItem { Text = "CE", Value = "3" },
                new SelectListItem { Text = "EE", Value = "4" }
            };
        }
    }
}
