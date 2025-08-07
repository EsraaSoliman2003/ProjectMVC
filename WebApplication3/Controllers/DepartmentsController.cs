using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly AppDbContext context;

        public DepartmentsController(AppDbContext Context)
        {
            context = Context;
        }
        public IActionResult Index()
        {
            var departments = context.Departments.ToList();
            return View(departments);
        }

        public IActionResult Details(int id)
        {
            var department = context.Departments.Find(id);

            if (department == null)
            {
                return NotFound($"No department with ID {id}");
            }

            return View(department);
        }

        // GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department dept)
        {
            dept.Id = context.Departments.Max(e => e.Id) + 1;
            context.Departments.Add(dept);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var dept = context.Departments.Find(id);

            return View(dept);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department dept)
        {
            var deptToUpdate = context.Departments.Find(dept.Id);

            if (deptToUpdate != null)
            {
                deptToUpdate.Name = dept.Name;
                context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            var deptToDelete = context.Departments.Find(id);


            if (deptToDelete != null)
            {
                context.Departments.Remove(deptToDelete);
                context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
