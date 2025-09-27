using BlogWebApp.Data.UnitOfWorks;
using BlogWebApp.Entity.Entities;
using BlogWebApp.Service.Services.Abstractions;

namespace BlogWebApp.Service.Services.Concreates
{
    public class DashboardService:IDashboardService
    {
        private readonly IUnitOfWork unitOfWork;


        public DashboardService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;

            
        }

        public async Task<List<int>> GetYearlyArticleCounts()
        {
            var article = await unitOfWork.GetRepository<Makale>().GetAllAsync(x=> !x.IsDeleted);

            var startDate = DateTime.Now.Date;
            startDate = new DateTime(startDate.Year, 1, 1);


            List<int> datas = new();

            for(int i=1; i <= 12; i++)
            {
                var startedDate = new DateTime(startDate.Year, i, 1);
                var endedDate = startedDate.AddMonths(1);
                var data = article.Where(x => x.YayinlanmaTarihi >= startedDate && x.YayinlanmaTarihi < endedDate).Count();
                datas.Add(data);
            }
            return datas;

        }

        public async Task<int> GetTotalArticleCounts()
        {
            var totalArticle = await unitOfWork.GetRepository<Makale>().CountAsync();
            return totalArticle;
        }


        public async Task<int> GetTotalCategoryCounts()
        {
            var totalkategori = await unitOfWork.GetRepository<Kategori>().CountAsync();
            return totalkategori;
        }

        public async Task<int> GetTotalUsersCounts()
        {
            var totalUsers = await unitOfWork.GetRepository<AppUser>().CountAsync();
            return totalUsers;
        }

        public async Task<int> GetTotalCommentsCounts()
        {
            var totalComments = await unitOfWork.GetRepository<Yorum>().CountAsync(x => !x.IsDeleted);
            return totalComments;
        }

    
    }
}
