using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext context;

        public StudentsController(AppDbContext Context)
        {
            context = Context;
        }
        public IActionResult Index(string sortBy = "id")
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
            var students = query.ToList();
            return View(students);
        }

        public IActionResult Search(string name)
        {
            var students = context.Students
                                  .Include(s => s.Department)
                                  .Where(s => EF.Functions.Like(s.FullName, $"%{name}%"))
                                  .ToList();

            return PartialView("_StudentsTable", students);
        }

        public IActionResult Details(int id)
        {
            var student = context.Students.Include(s => s.Department)
                                          .FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();

            return View(student);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(context.Departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            ViewBag.Departments = new SelectList(context.Departments, "Id", "Name");
            return View(student);
        }

        [HttpGet]
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
        public IActionResult Edit(Student student)
        {
            ViewBag.Departments = new SelectList(context.Departments, "Id", "Name");
            return View(student);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = context.Students.Find(id);
            if (student == null)
                return RedirectToAction(nameof(Index));

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
