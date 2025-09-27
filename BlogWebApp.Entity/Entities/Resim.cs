using BlogWebApp.Core.Entities;

namespace BlogWebApp.Entity.Entities
{
    public class Resim:EntityBase
    {
        public Resim()
        {
            
        }
        public Resim(string dosyaYolu,string olusturanKullanici)
        {
      
            DosyaYolu = dosyaYolu;
            OlusturanKullanici = olusturanKullanici;

        }
        public string DosyaYolu { get; set; }
      
        public ICollection<Makale> Makales { get; set; }
        public ICollection<AppUser> Users { get; set; }


                    }
}
