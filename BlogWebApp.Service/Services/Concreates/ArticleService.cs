using AutoMapper;
using BlogWebApp.Data.Context;
using BlogWebApp.Data.UnitOfWorks;
using BlogWebApp.Entity.DTOs.Makales;
using BlogWebApp.Entity.Entities;
using BlogWebApp.Entity.Enums;
using BlogWebApp.Service.Extensions;
using BlogWebApp.Service.Helpers.Images;
using BlogWebApp.Service.Helpers.Slug;
using BlogWebApp.Service.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace BlogWebApp.Service.Services.Concreates
{
    public class ArticleService : IArticleService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal user;
        private readonly IImageHelper imageHelper;
      
        public ArticleService( IUnitOfWork _unitOfWork,IMapper _mapper,IHttpContextAccessor _httpContextAccessor, IImageHelper _imageHelper)
        {
            mapper = _mapper;
            
            httpContextAccessor = _httpContextAccessor;
            user = httpContextAccessor.HttpContext.User;
            unitOfWork = _unitOfWork;
            imageHelper = _imageHelper;
            
        }



        public async Task<MakaleListeDto> GetAllByPagingAsync(Guid? categoryId,int currentPage= 1, int pageSize=3,bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;
            var articles = categoryId == null
                ? await unitOfWork.GetRepository<Makale>().GetAllAsync(a => !a.IsDeleted && a.Durum == BlogDurumu.Yayinda, a => a.Kategori, i => i.Resim, u => u.User,c => c.Yorums)
                : await unitOfWork.GetRepository<Makale>().GetAllAsync(a => a.KategoriId == categoryId && !a.IsDeleted && a.Durum == BlogDurumu.Yayinda, x => x.Kategori, y => y.Resim, u => u.User, c => c.Yorums);


            var sortedArticles = isAscending
                ? articles.OrderBy(x => x.YayinlanmaTarihi).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(x => x.YayinlanmaTarihi).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();


            var kategori = (await unitOfWork.GetRepository<Kategori>().GetAllAsync(a => !a.IsDeleted))
                .OrderBy(k => k.Ad) 
                .ToList();


            var top3ViewArticles = await GetTop3MostViewedAsync();

            return new MakaleListeDto
            {
                Kategories = kategori,
                Articles = sortedArticles,
                CategoryId = categoryId == null ? null : categoryId.Value,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count(),
                IsAscending = isAscending,
                TopViewedArticles = top3ViewArticles

            };


        }

        public async Task<IList<Makale>> GetTop3MostViewedAsync()
        {
            var all = await unitOfWork.GetRepository<Makale>().GetAllAsync(a=> !a.IsDeleted && a.Durum == BlogDurumu.Yayinda, a => a.Resim, a => a.Kategori, a => a.User);

            return all.OrderByDescending(a => a.Goruntuleme).Take(3).ToList();

        }
        


        public  async Task CreateMakaleAsync(MakaleEkleDto makaleEkleDto)
        {

            var userId = user.GetLoggedInUserId();
            var userEmail = user.GetLoggedInEmail();
            
            Resim image;

            if (makaleEkleDto.Photo != null && makaleEkleDto.Photo.Length > 0)
            {
                var imageUpload = await imageHelper.Upload(makaleEkleDto.Baslik, makaleEkleDto.Photo, ImageType.Post);
                 image = new(imageUpload.FullName, userEmail);


            }
            else
            {
                var defaultImagePath = "https://ibb.co/9mX8Y1c0";
             
                image = new Resim(defaultImagePath,  userEmail);
            }

      

            await unitOfWork.GetRepository<Resim>().AddAsync(image);


            var baseSlug = makaleEkleDto.Baslik.ToSlug();
            var slug = baseSlug;
            int i = 2;
            var makaleRepo = unitOfWork.GetRepository<Makale>();
            while (await makaleRepo.AnyAsync(m => m.Slug == slug))
            {
                slug = $"{baseSlug}-{i++}";

            }

            var makale = new Makale(makaleEkleDto.Icerik,makaleEkleDto.Ozet,makaleEkleDto.Baslik, userId,userEmail,  makaleEkleDto.KategoriId,image.Id,makaleEkleDto.YorumIzin);
            makale.Durum = makaleEkleDto.Durum;
            makale.Slug = slug;
            await unitOfWork.GetRepository<Makale>().AddAsync(makale);
            await unitOfWork.SaveAsync();
        }


        public async Task PublishMakaleAsync(MakaleYayinlaDto makaleYayinlaDto)
        {
            var userId = user.GetLoggedInUserId();
            var userEmail = user.GetLoggedInEmail();

            Resim image;

            if (makaleYayinlaDto.Photo != null && makaleYayinlaDto.Photo.Length > 0)
            {
            
               var imageUpload = await imageHelper.Upload(makaleYayinlaDto.Baslik, makaleYayinlaDto.Photo, ImageType.Post);
                image = new(imageUpload.FullName,userEmail);
            }

            else
            {
                var defaultImagePath = "https://i.ibb.co/Y49Rj1PS/default-article.jpg";
               
                image = new Resim(defaultImagePath,  userEmail);
            }
             
         

            await unitOfWork.GetRepository<Resim>().AddAsync(image);




            var baseSlug = makaleYayinlaDto.Baslik.ToSlug();
            var slug = baseSlug;
            int i = 2;
            var makaleRepo = unitOfWork.GetRepository<Makale>();
            while(await makaleRepo.AnyAsync(m => m.Slug == slug))
            {
                slug = $"{baseSlug}-{i++}";

            }



            var makale = new Makale(makaleYayinlaDto.Icerik, makaleYayinlaDto.Ozet, makaleYayinlaDto.Baslik, userId, userEmail, makaleYayinlaDto.KategoriId, image.Id,makaleYayinlaDto.YorumIzin);
            makale.Slug = slug;
            makale.Durum = makaleYayinlaDto.Durum;

            if (!String.IsNullOrWhiteSpace(makaleYayinlaDto.Tags))
            {
                var tagList = makaleYayinlaDto.Tags.Split(',').Select(s => s.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList();
                var tagRepo = unitOfWork.GetRepository<Tag>();
                foreach (var tagName in tagList)
                {
                   
                    var tag = await tagRepo.GetOrDefaultAsync(x => x.Ad == tagName);

                    if (tag == null)
                    {
                        tag = new Tag { Ad = tagName };
                        await tagRepo.AddAsync(tag);
                        await unitOfWork.SaveAsync(); 
                    }

                    makale.MakaleTags.Add(new MakaleTag
                    {
                        Makale = makale,
                        Tag = tag
                    });
                }
            }

            await makaleRepo.AddAsync(makale);
            await unitOfWork.SaveAsync();
        }




        public async Task<List<MakaleDto>> GetAllArticlesWithCategoryNonDeletedAsync()
        {
           
           var article= await unitOfWork.GetRepository<Makale>().GetAllAsync(x=> !x.IsDeleted,x=>x.Kategori);
            var map = mapper.Map<List<MakaleDto>>(article);
            return map;
        }

        public async Task<MakaleDto> GetArticleWithCategoryNonDeletedAsync(Guid id)
        {
            var article = await unitOfWork.GetRepository<Makale>().GetAsync(x => !x.IsDeleted && x.Id==id, x => x.Kategori, i => i.Resim, x => x.Yorums.Where(y => !y.IsDeleted), x=> x.User.Resim);
            
            var map = mapper.Map<MakaleDto>(article);
            return map;
        }
        public async Task<string> UpdateArticleAsync(MakaleGuncelleDto makaleGuncelleDto)
        {
            var userEmail = user.GetLoggedInEmail();
            var article = await unitOfWork.GetRepository<Makale>().GetAsync(x => !x.IsDeleted && x.Id == makaleGuncelleDto.Id, x => x.Kategori, i => i.Resim);

            Guid? newResimId = null;
            if (makaleGuncelleDto.Foto != null)
            {

                if (article.Resim != null)
                {
                    imageHelper.Delete(article.Resim.DosyaYolu);
                }
                var imageUpload =await imageHelper.Upload(makaleGuncelleDto.Baslik,makaleGuncelleDto.Foto,ImageType.Post);
                Resim resim = new(imageUpload.FullName,userEmail);
                await unitOfWork.GetRepository<Resim>().AddAsync(resim);
                newResimId = resim.Id;

            }

            mapper.Map(makaleGuncelleDto, article);


            if (newResimId.HasValue)
            {
                article.ResimId = newResimId.Value;
            }

            article.DegistirilmeTarihi = DateTime.Now;
            article.DegistirenKullanici = userEmail;


            await unitOfWork.GetRepository<Makale>().UpdateAsync(article);
            await unitOfWork.SaveAsync();
            return article.Baslik;
        }


        public async Task<string> SafeDeleteArticleAsync(Guid makaleId)
        {
            var userEmail = user.GetLoggedInEmail();
            var article = await unitOfWork.GetRepository<Makale>().GetByGuidAsync(makaleId);
            article.IsDeleted = true;
            article.SilinmeTarihi = DateTime.Now;
            article.SilenKullanici = userEmail;
            await unitOfWork.GetRepository<Makale>().UpdateAsync(article);
            await unitOfWork.SaveAsync();
            return article.Baslik;
        }

        public async Task<List<MakaleDto>> GetAllArticlesWithCategoryDeletedAsync()
        {
            var article = await unitOfWork.GetRepository<Makale>().GetAllAsync(x => x.IsDeleted, x => x.Kategori);
            var map = mapper.Map<List<MakaleDto>>(article);
            return map;
        }

        public async Task<string> UndoDeleteArticleAsync(Guid makaleId)
        {
           
            var article = await unitOfWork.GetRepository<Makale>().GetByGuidAsync(makaleId);
            article.IsDeleted = false;
            article.SilinmeTarihi = null;   
            article.SilenKullanici = null;
            await unitOfWork.GetRepository<Makale>().UpdateAsync(article);
            await unitOfWork.SaveAsync();
            return article.Baslik;
        }

        public async Task<MakaleListeDto> SearchAsync(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;
            var articles =await unitOfWork.GetRepository<Makale>().GetAllAsync(a =>  !a.IsDeleted  && (a.Icerik.Contains(keyword) || a.Baslik.Contains(keyword) || a.Kategori.Ad.Contains(keyword)), x => x.Kategori, y => y.Resim, u => u.User);


            var sortedArticles = isAscending
                ? articles.OrderBy(x => x.YayinlanmaTarihi).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(x => x.YayinlanmaTarihi).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new MakaleListeDto
            {
                Articles = sortedArticles,
                
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count(),
                IsAscending = isAscending

            };
        }

        public async Task<MakaleDto> ArticleVisitorAddAsync(Guid id)
        {
            var ipAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var articleVisitors = await unitOfWork.GetRepository<ArticleVisitor>().GetAllAsync(null, x=> x.Visitor, y => y.Makale);
            var article = await unitOfWork.GetRepository<Makale>().GetAsync(x => x.Id == id);
            var result = await GetArticleWithCategoryNonDeletedAsync(id);
            var visitor = await unitOfWork.GetRepository<Visitor>().GetAsync(x => x.IpAddress == ipAddress);


            var addArticleVisitors = new ArticleVisitor(article.Id,visitor.Id);

            if(!articleVisitors.Any(x => x.VisitorId== addArticleVisitors.VisitorId && x.MakaleId == addArticleVisitors.MakaleId))
            {
                await unitOfWork.GetRepository<ArticleVisitor>().AddAsync(addArticleVisitors);
                article.Goruntuleme += 1;
                await unitOfWork.GetRepository<Makale>().UpdateAsync(article);
                await unitOfWork.SaveAsync();

            }







            return result;

            

        }

        public async Task<MakaleDto> GetBySlugAsync(string slug)
        {
            var makale = await unitOfWork.GetRepository<Makale>().GetSingleAsync(m => m.Slug == slug && !m.IsDeleted && m.Durum == BlogDurumu.Yayinda,t => t.Yorums, m => m.Kategori, m => m.User, m => m.Resim,m => m.MakaleTags);

            var tagIds = makale.MakaleTags.Select(mt => mt.TagId).ToList();

            var tags = await unitOfWork.GetRepository<Tag>()
     .GetAllAsync(t => tagIds.Contains(t.Id));

            var makaleDto = mapper.Map<MakaleDto>(makale);
            makaleDto.Tags = tags;

            return makaleDto;
        }

        public async Task<List<MakaleDto>> GetCurrentUserArticlesWithCategoryAsync()
        {
            var userId = user.GetLoggedInUserId();

            var article = await unitOfWork.GetRepository<Makale>().GetAllAsync(x => !x.IsDeleted && x.UserId == userId && x.Durum == BlogDurumu.Yayinda, x => x.Kategori,x => x.User.Resim);
            var map = mapper.Map<List<MakaleDto>>(article);
            return map;

        }




        public async Task<List<MakaleDto>> GetCurrentUserDraftsArticlesWithCategoryAsync()
        {
            var userId = user.GetLoggedInUserId();

            var article = await unitOfWork.GetRepository<Makale>().GetAllAsync(x => !x.IsDeleted && x.UserId == userId && x.Durum == BlogDurumu.Taslak, x => x.Kategori, x => x.User.Resim);
            var map = mapper.Map<List<MakaleDto>>(article);
            return map;

        }






        public async Task<MakaleDto> GetArticleWithCategoryNonDeletedSlugAsync(string slug)
        {
            var article = await unitOfWork.GetRepository<Makale>().GetAsync(x => !x.IsDeleted && x.Slug == slug, x => x.Kategori, i => i.Resim, x => x.Yorums.Where(y => !y.IsDeleted), x => x.User.Resim);

            var map = mapper.Map<MakaleDto>(article);
            return map;
        }

        public async Task<string> PublishDraftsMakaleAsync(Guid makaleId)
        {

            var article = await unitOfWork.GetRepository<Makale>().GetByGuidAsync(makaleId);

          
            article.Durum = BlogDurumu.Yayinda;
            await unitOfWork.GetRepository<Makale>().UpdateAsync(article);
            await unitOfWork.SaveAsync();
            return article.Baslik;
        }

 
    }
}
