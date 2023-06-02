using System.ComponentModel.DataAnnotations;
using kursova.Data;

namespace kursova.Models
{
    public class AddApplicationViewModel
    {
        public DateTime? Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "The Content is required")]
        [StringLength(255)]
        public string Content { get; set; } = null!;

        public int CitizenId { get; set; }

        public int? SpecialistId { get; set; }

    }
}
