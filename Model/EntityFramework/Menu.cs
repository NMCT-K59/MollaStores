namespace Model.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Menu")]
    public partial class Menu
    {
        public int ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Hãy nhập tên menu")]
        public string Text { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Hãy nhập liên kết")]
        public string Link { get; set; }

        [Required(ErrorMessage = "Hãy nhập thứ tự hiển thị")]

        public int? DisplayOrder { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Hãy nhập loại cửa sổ")]
        public string Target { get; set; }

        public bool? Status { get; set; }

        public int? TypeID { get; set; }
    }
}
