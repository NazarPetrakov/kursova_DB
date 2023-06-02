using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace kursova.Models
{
    public class AddCitizenViewModel
    {
        [Required(ErrorMessage = "The First Name is required")]
        [StringLength(25)]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "The Last Name is required")]
        [StringLength(25)]
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "The Phone Number is required")]
        [Phone(ErrorMessage = "Enter valid Phone Number")]
        [RegularExpression(@"^(\+\d{1, 2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$", ErrorMessage = "Enter valid Phone Number")]
        public string PhoneNumber { get; set; } = null!;
    }
}
