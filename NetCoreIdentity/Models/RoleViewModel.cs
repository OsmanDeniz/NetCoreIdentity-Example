using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity.Models
{
    public class RoleViewModel
    {
        [Required(ErrorMessage ="Role adi gereklidir.")]
        public string Name { get; set; }
    }
}
