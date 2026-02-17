
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Airflights.Models
{
    public enum UserRoles
    {
        [EnumMember(Value = "Admin")]
        Admin,
        [EnumMember(Value = "Manager")]
        Manager,
        [EnumMember(Value = "Staf")]
        Staf,
        [EnumMember(Value = "User")]
        User        
    }
    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string Login { get; set; } = string.Empty;
        [Required]
        [MaxLength(256)]
        [JsonIgnore]
        public string Hash { get; set; } = string.Empty;
        [Required]
        [MaxLength(20)]
        public UserRoles Role { get; set; } = UserRoles.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; } = true;


    }
}