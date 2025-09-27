namespace BlogWebApp.Core.Entities
{
    public abstract class EntityBase:IEntityBase
    {
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public virtual string OlusturanKullanici { get; set; } = "Undefined";   

        public virtual string DegistirenKullanici { get; set; }
        public virtual string SilenKullanici { get; set; }
        public virtual DateTime?  SilinmeTarihi { get; set; }
        public virtual DateTime DegistirilmeTarihi { get; set; }
        public virtual bool IsDeleted { get; set; } = false;

 



    }
}
