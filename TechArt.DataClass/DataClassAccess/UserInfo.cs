using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechArt.DataClass.DataClassAccess
{
    [Table("UserInfo")]
    public class UserInfo
    {

        [Key, ForeignKey("Identity")]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string NickName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        public DateTime BirthDate { get; set; }


        //存储图片BASE64
        [Required]
        public string Image { get; set; }

        public DateTime CreateTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string CreateBy { get; set; }

        [Required]
        [MaxLength(50)]
        public string UpdBy { get; set; }

        public DateTime UpdTime { get; set; }

        public long UpdVer { get; set; }

        public Identity Identity { get; set; }
    }
}