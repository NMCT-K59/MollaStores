using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EntityFramework
{
    [Table("ReviewProduct")]
    public class ReviewProduct
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
