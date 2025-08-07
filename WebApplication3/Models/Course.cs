using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? CourseName { get; set; }
    }
}
