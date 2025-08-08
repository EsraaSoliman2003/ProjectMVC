using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly AppDbContext context;

        public StudentsController(AppDbContext Context)
        {
            context = Context;
        }

        [AllowAnonymous]
        public IActionResult Index(string sortBy = "id", int page = 1)
        {
            var query = context.Students.Include(s => s.Department).AsQueryable();
            if (sortBy == "name")
            {
                query = query.OrderBy( d => d.FullName );
            }
            else if (sortBy == "age")
            {
                query = query.OrderBy(d => d.Age);
            }
            else if (sortBy == "department")
            {
                query = query.OrderBy(d => d.Department);
            }
            else
            {
                query = query.OrderBy(d => d.Id);
            }

            int pageSize = 5;

            var students = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalItems = context.Students.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;


            return View(students);
        }


        [AllowAnonymous]
        public IActionResult Search(string name)
        {
            var students = context.Students
                                  .Include(s => s.Department)
                                  .Where(s => EF.Functions.Like(s.FullName, $"%{name}%"))
                                  .ToList();

            return PartialView("_StudentsTable", students);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var student = context.Students.Include(s => s.Department)
                                          .FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();

            return View(student);
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Student")]
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(context.Departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Student")]
        public IActionResult Create(Student student)
        {
            var fileName = Path.GetFileName(student.ImageFile.FileName);
            var filePath = "wwwroot/images/" + fileName;
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                student.ImageFile.CopyTo(stream);
            }
            student.ImagePath = "/images/" + fileName;
            if (ModelState.IsValid)
            {
                context.Students.Add(student);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = new SelectList(context.Departments, "Id", "Name");
            return View(student);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Student")]
        public IActionResult Edit(int id)
        {
            var student = context.Students.Find(id);
            if (student == null)
                return NotFound();

            ViewBag.Departments = new SelectList(context.Departments, "Id", "Name");
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Student")]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                string fileName = student.ImageFile.FileName;
                string filePath = "wwwroot/images/" + fileName;
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    student.ImageFile.CopyTo(fs);
                }
                student.ImagePath = "/images/" + fileName;
                context.Students.Update(student);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = new SelectList(context.Departments, "Id", "Name");
            return View(student);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Student")]
        public IActionResult Delete(int id)
        {
            var student = context.Students.Find(id);
            if (student == null)
                return RedirectToAction(nameof(Index));

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Student")]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = context.Students.Find(id);
            if (student != null)
            {
                context.Students.Remove(student);
                context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
