using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EntityFramework
{
    [Table("ProductImage")]
    public class ProductImage
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
