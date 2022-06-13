namespace Model.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public long ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Hãy nhập tên đăng nhập")]
        public string UserName { get; set; }

        [StringLength(32)]
        [Required(AllowEmptyStrings = true)]
        public string Password { get; set; }

        [StringLength(20)]
        public string GroupID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Hãy nhập tên")]
        public string Name { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Hãy nhập email")]
        public string Email { get; set; }

        [StringLength(50)]
        
        public string Phone { get; set; }

        public int? ProvinceID { get; set; }

        public int? DistrictID { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [StringLength(100)]
        public string CodeRecovery { get; set; }

        public bool Status { get; set; }
    }
}
