using Model.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class SlideDAO
    {
        SuperMarketKDbContext db = null;
        public SlideDAO()
        {
            db = new SuperMarketKDbContext();
        }
        public Slide GetByID(long id)
        {
            return db.Slides.Find(id);
        }
        public List<Slide> getList()
        {
            return db.Slides.OrderByDescending(x => x.CreatedDate).ToList();
        }
        public IEnumerable<Slide> ListAllPaging(string searchString, int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return db.Slides.Where(x => x.Image.Contains(searchString)
                || x.Description.Contains(searchString)).OrderBy(x => x.ID).ToPagedList(page, pageSize);
            }
            return db.Slides.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
       
       

    }
}
