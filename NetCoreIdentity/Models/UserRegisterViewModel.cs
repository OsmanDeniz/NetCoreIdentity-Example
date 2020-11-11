using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreIdentity.Models
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Bu alan bos gecilemez.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Bu alan bos gecilemez.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bu alan bos gecilemez.")]
        [Compare("Password",ErrorMessage ="Parolalar ayni degil")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Bu alan bos gecilemez.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bu alan bos gecilemez.")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Bu alan bos gecilemez.")]
        public string Email { get; set; }

    }
}
