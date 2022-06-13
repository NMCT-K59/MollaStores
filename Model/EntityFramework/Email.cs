using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EntityFramework
{
    [Table("Email")]
    public class Email
    {
        public int ID { get; set; }

        [Column(TypeName = "ntext")]
        public string Email_ID { get; set; }
    }
}
