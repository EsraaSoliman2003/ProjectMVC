using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string? FullName { get; set; }

        [Range(18, 60, ErrorMessage = "Age must be between 18 & 60!")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string? Email { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }

        [NotMapped]
        [Display(Name = "Profile Image")]
        public IFormFile? ImageFile { get; set; }

        public string? ImagePath { get; set; }

    }
}
