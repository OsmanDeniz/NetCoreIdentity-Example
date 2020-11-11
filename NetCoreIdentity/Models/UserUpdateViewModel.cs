using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace NetCoreIdentity.Models
{
    public class UserUpdateViewModel
    {
        [Required(ErrorMessage = "Bu alan bos gecilemez.")]
        [EmailAddress(ErrorMessage = "Gecersiz format.")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PictureURL { get; set; }
        public IFormFile Picture { get; set; }
        [Required(ErrorMessage = "Bu alan bos gecilemez.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bu alan bos gecilemez.")]
        public string Surname { get; set; }
    }
}
