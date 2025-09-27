using BlogWebApp.Entity.Entities;
using Microsoft.AspNetCore.Http;

namespace BlogWebApp.Entity.DTOs.Makales
{
    public class MakaleListeDto
    {
        public IList<Kategori> Kategories { get; set; }
        public IList<Makale> Articles { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual int CurrentPage { get; set; } = 1;
        public virtual int PageSize { get; set; } = 3;
        public virtual int TotalCount { get; set; }
        public virtual int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));
        public virtual bool ShowPrevious => CurrentPage > 1;
        public virtual bool ShowNext => CurrentPage < TotalPages;
        public virtual bool IsAscending { get; set; } = false;

        public IList<Makale> TopViewedArticles { get; set; }
        public Resim Resim { get; set; }
        public IFormFile? Foto { get; set; }

        public IList<Yorum> Yorums { get; set; }


    }
}
