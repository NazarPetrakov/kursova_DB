using System.ComponentModel.DataAnnotations;

namespace kursova.Models
{
    public class UpdateSpecialistViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The First Name is required")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "The First Name is required")]
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "The First Name is required")]
        public string Specialty { get; set; } = null!;
        public bool HasRelatedApplications { get; set; }
    }
}
