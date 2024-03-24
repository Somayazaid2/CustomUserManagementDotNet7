using System.ComponentModel.DataAnnotations;

namespace UserManagement.ViewModels
{
    public class ProfileFormViewModel
    {
        public string Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "max Length is 100 character")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "max Length is 100 character")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

    }
}
