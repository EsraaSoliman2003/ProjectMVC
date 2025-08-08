using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }
    }

}
