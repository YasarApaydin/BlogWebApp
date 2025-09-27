namespace BlogWebApp.Entity.Entities
{
    public class MakaleTag
    {
        public Guid MakaleId { get; set; }
        public Makale Makale { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
