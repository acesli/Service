using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechArt.DataClass.DataClassAccess
{
    [Table("Identity")]
    public class Identity
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
         

        [Required]
        [MaxLength(200)]
        public string Password { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}