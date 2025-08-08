using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly AppDbContext context;

        public DepartmentsController(AppDbContext Context)
        {
            context = Context;
        }

        [AllowAnonymous]
        public IActionResult Index(string sortBy = "id", int page = 1)
        {
            var query = context.Departments.AsQueryable();
            if (sortBy == "name")
            {
                query = query.OrderBy(d => d.Name);
            }
            else
            {
                query = query.OrderBy(d => d.Id);
            }

            int pageSize = 5;

            var departments = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalItems = context.Departments.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;


            return View(departments);
        }

        [AllowAnonymous]
        public IActionResult Search(string name)
        {
            var departments = context.Departments
                                  .Where(d => EF.Functions.Like(d.Name, $"%{name}%"))
                                  .ToList();

            return PartialView("_DepartmentTablePartial", departments);
        }

        [AllowAnonymous]
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Department dept)
        {
            dept.Id = context.Departments.Max(e => e.Id) + 1;
            context.Departments.Add(dept);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var dept = context.Departments.Find(id);

            return View(dept);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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


        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var deptToDelete = context.Departments.Find(id);


            if (deptToDelete != null)
            {
                var student = context.Students.FirstOrDefault(s => s.DepartmentId == id);

                if (student != null)
                {
                    ViewBag.Error = "You Can't Delete This Department There is Related Data";
                    return View("Index", context.Departments.ToList());
                }
                
                context.Departments.Remove(deptToDelete);
                context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbs("GET", "POST")]
        [Authorize(Roles = "Admin")]
        public IActionResult IsNameUnique(string name, int id)
        {
            var isExist = context.Departments.Any(d => d.Name == name && d.Id != id);
            return Json(!isExist);
        }

    }
}
