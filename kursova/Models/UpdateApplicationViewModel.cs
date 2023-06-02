using kursova.Data;
using System.ComponentModel.DataAnnotations;

namespace kursova.Models
{
    public class UpdateApplicationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Date is required")]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "The Content is required")]
        [StringLength(255)]
        public string Content { get; set; } = null!;

        public string? Status { get; set; }

        public int CitizenId { get; set; }

        public int? SpecialistId { get; set; }

    }
}
