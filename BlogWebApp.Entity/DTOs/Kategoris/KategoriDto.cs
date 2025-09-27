namespace BlogWebApp.Entity.DTOs.Kategoris
{
    public class KategoriDto
    {
        public Guid Id { get; set; }
        public string Ad { get; set; }

        public string OlusturanKullanici { get; set; }
        public DateTime OlusturulmaTarihi { get; set; }
        public bool IsDeleted { get; set; }


    }
}
