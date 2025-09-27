using BlogWebApp.Core.Entities;

namespace BlogWebApp.Entity.Entities
{
    public class Tag:EntityBase
    {
        public string Ad { get; set; }

        public ICollection<MakaleTag> MakaleTags { get; set; } = new List<MakaleTag>();
    }
}
