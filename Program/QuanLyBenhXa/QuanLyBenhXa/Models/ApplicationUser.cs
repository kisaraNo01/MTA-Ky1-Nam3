using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBenhXa.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Link to BacSi if this user is a doctor
        public int? BacSiId { get; set; }

        [ForeignKey("BacSiId")]
        public virtual BacSi? BacSi { get; set; }
        
        // Additional user info
        public string FullName { get; set; } = string.Empty;
    }
}
