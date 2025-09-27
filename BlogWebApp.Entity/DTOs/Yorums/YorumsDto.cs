using BlogWebApp.Entity.Entities;

namespace BlogWebApp.Entity.DTOs.Yorums
{
    public class YorumsDto
    {

        public Guid Id { get; set; }
        public string Icerik { get; set; }
        public Guid MakaleId { get; set; }
        public Makale Makale { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime YorumTarihi { get; set; }
        public string OlusturanKullanici { get; set; }
        public bool IsDeleted { get; set; }
      

    }
}
