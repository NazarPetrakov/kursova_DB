using System.ComponentModel.DataAnnotations;

namespace kursova.Models
{
    public class AddSpecialistViewModel
    {
        [Required(ErrorMessage = "The First Name is required")]
        [StringLength(25)]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "The Last Name is required")]
        [StringLength(25)]
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "The Specialty is required")]
        [StringLength(75)]
        public string Specialty { get; set; } = null!;
    }
}
