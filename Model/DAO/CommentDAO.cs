using Model.EntityFramework;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class CommentDAO
    {
        SuperMarketKDbContext db = null;
        public CommentDAO()
        {
            db = new SuperMarketKDbContext();
        }

        public IEnumerable<Feedback> ListAllPagingFeedback(string searchString, int page, int pageSize)
        {
            var listFeedBack = db.Feedbacks.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                return listFeedBack.Where(x => x.Content.Contains(searchString)
                || x.Name.Contains(searchString) || x.Email.Contains(searchString)).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }
            return listFeedBack.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<CommentView> ListAllPaging(string searchString, int page, int pageSize)
        {
            List<Comment> comments = db.Comments.ToList();
            List<Content> contents = db.Contents.ToList();
            var commentRecord = from a in comments
                                join b in contents on a.PostID equals b.ID
                                select new CommentView
                                {
                                    comment = a,
                                    content = b
                                };
            if (!string.IsNullOrEmpty(searchString))
            {
                return commentRecord.Where(x => x.comment.Name.Contains(searchString)
                || x.comment.Message.Contains(searchString) || x.comment.PostID == Convert.ToInt32(searchString)).OrderByDescending(x => x.comment.CreatedDate).ToPagedList(page, pageSize);
            }
            return commentRecord.OrderByDescending(x=>x.comment.CreatedDate).ToPagedList(page, pageSize);
        }
    }
}
