using Common;
using Model.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model.DAO
{
    public class ContentDAO
    {
        SuperMarketKDbContext db = null;
        public ContentDAO()
        {
            db = new SuperMarketKDbContext();
        }

        public void tangView(long id)
        {
            var content = db.Contents.Find(id);
            content.ViewCount++;
            db.SaveChanges();
        }

        public List<Content> getXemNhieu()
        {
            return db.Contents.Take(5).OrderByDescending(x => x.ViewCount).ToList();
        }

        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }

        public List<Content> getList()
        {
            return db.Contents.OrderByDescending(x=>x.CreatedDate).ToList();
        }

        public long Create(Content content)
        {
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = Common.StringHelper.ToUnsignString(content.Name);
            }
            content.CreatedDate = DateTime.Now;
            content.ViewCount = 0;
            db.Contents.Add(content);
            db.SaveChanges();
            if (!string.IsNullOrEmpty(content.Tags))
            {
                string[] tags = content.Tags.Split(',');
                foreach (var item in tags)
                {
                    var tagID = Common.StringHelper.ToUnsignString(item);
                    var exitstedTag = this.GetTagByID(tagID);
                    if (!exitstedTag)
                    {
                        this.InsertTag(tagID, item);
                    }
                    this.InsertContentTag(content.ID, tagID);
                }
            }
            return content.ID;
        }

        public void RemoveAllContentTag(long contentId)
        {
            db.ContentTags.RemoveRange(db.ContentTags.Where(x => x.ContentID == contentId));
            db.SaveChanges();
        }

        public long Edit(Content content)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreatedDate = DateTime.Now;
            db.SaveChanges();

            //Xử lý tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                this.RemoveAllContentTag(content.ID);
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.GetTagByID(tagId);

                    //insert to to tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }
                    //insert to content tag
                    this.InsertContentTag(content.ID, tagId);
                }
            }

            return content.ID;
        }

        public void InsertTag(string id, string name)
        {
            var tag = new Tag
            {
                ID = id,
                Name = name
            };
            db.Tags.Add(tag);
            db.SaveChanges();
        }

        public void InsertContentTag(long contentID, string tagID)
        {
            var contentTag = new ContentTag
            {
                ContentID = contentID,
                TagID = tagID
            };
            db.ContentTags.Add(contentTag);
            db.SaveChanges();
        }

        public bool GetTagByID(string id)
        {
            return db.Tags.Count(x=>x.ID==id)>0;
        }

        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return db.Contents.Where(x => x.Name.Contains(searchString)
                || x.Description.Contains(searchString) || x.Detail.Contains(searchString)).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }
            return db.Contents.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public bool Delete(int ID)
        {
            try
            {
                var user = db.Contents.Find(ID);
                db.Contents.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }

        }
    }
}
