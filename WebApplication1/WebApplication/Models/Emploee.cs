using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Emploee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string City { get; set; }
        public Gender GenderEmploey { get; set; }   
    }

    public enum Gender
    {
        [Display(Name = "Male")]
        Male=1,
        [Display(Name = "Female")]
        Female=2
    }
}