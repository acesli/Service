using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechArt.DataClass.DataClassAccess
{
    [Table("Session")]
    public class Session
    {
        [Key]
        [Required]
        public Guid ID { get; set; }

        [Required]
        [MaxLength(37)]
        public string IP { get; set; }

        [Required]
        public string  Device { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        public string AddtionalData { get; set; }
        
        public virtual ICollection<Token> Tokens { get; set; }
    }
}