using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechArt.DataClass.DataClassAccess
{
    [Table("TokenLog")]
    public class TokenLog
    {
        [Required]
        public Guid TokenID { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        public string AddtionalData { get; set; }
    }
}