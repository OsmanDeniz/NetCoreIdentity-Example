using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreIdentity.Models
{
    public class UserSignInViewModel
    {   

        [Required(ErrorMessage ="Bu alan bos gecilemez.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Bu alan bos gecilemez.")]
        public string Password { get; set; }
    }
}
