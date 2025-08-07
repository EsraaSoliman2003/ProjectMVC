using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Models
{
    public class Department
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Remote(action: "IsNameUnique", controller: "Departments", AdditionalFields = "Id", ErrorMessage = "This department name already exists.")]
        public string? Name { get; set; }
        public ICollection<Student> Students { get; set; } =
            new HashSet<Student>();

    }
}
