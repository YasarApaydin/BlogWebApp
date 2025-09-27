using BlogWebApp.Core.Entities;

namespace BlogWebApp.Entity.Entities
{
    public class ArticleVisitor:IEntityBase
    {
        public ArticleVisitor()
        {
            
        }

        public ArticleVisitor(Guid makaleId,int visitorId)
        {
            MakaleId = makaleId;

            VisitorId = visitorId; 
        }
        public Guid MakaleId { get; set; }
        public Makale Makale { get; set; }
        public int VisitorId { get; set; }
        public Visitor Visitor { get; set; }
    }
}
