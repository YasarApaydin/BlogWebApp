using BlogWebApp.Core.Entities;

namespace BlogWebApp.Entity.Entities
{
    public class Yorum:EntityBase
    {


        public Yorum()
        {
            
        }

        public Yorum(Guid makaleId, string icerik,Guid userId, string olusturanKullanici)
        {
            MakaleId = makaleId;
            Icerik = icerik;
            UserId = userId;
            OlusturanKullanici = olusturanKullanici;
        }

        public Guid MakaleId { get; set; }
        public Makale Makale { get; set; }
        public string Icerik { get; set; }
        public DateTime YorumTarihi { get; set; } = DateTime.Now;

        public Guid UserId { get; set; }
        public AppUser User { get; set; }
    }
}
