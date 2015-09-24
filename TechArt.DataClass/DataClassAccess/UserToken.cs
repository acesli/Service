using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechArt.DataClass.DataClassAccess
{
    [Table("UserToken")]
    public class UserToken
    {
        [Key, ForeignKey("Token")]
        public Guid TokenId { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        public virtual Token Token { get; set; }

    }
}