using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [MaxLength(100, ErrorMessage = "maxlength must be 100 character")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "maxlength must be 100 character")]
        public string LastName { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}
