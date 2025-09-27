using BlogWebApp.Core.Entities;

namespace BlogWebApp.Entity.Entities
{
    public class Kategori:EntityBase
    {
        public Kategori()
        {
            
        }
        public Kategori(string ad,string olusturanKullanici)
        {
            OlusturanKullanici = olusturanKullanici;
            Ad = ad;
        }
        public string Ad { get; set; }
        public ICollection<Makale> Makales { get; set;  }
    }
}
