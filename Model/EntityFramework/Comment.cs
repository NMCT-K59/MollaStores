using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EntityFramework
{
    [Table("Comment")]
    public partial class Comment
    {
        public long ID { get; set; }

        [Column(TypeName = "ntext")]
        public string Message { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long PostID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
