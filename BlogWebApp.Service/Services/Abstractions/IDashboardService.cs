namespace BlogWebApp.Service.Services.Abstractions
{
    public interface IDashboardService
    {
        Task<List<int>> GetYearlyArticleCounts();
        Task<int> GetTotalArticleCounts();
        Task<int> GetTotalCategoryCounts();

        Task<int> GetTotalUsersCounts();
        Task<int> GetTotalCommentsCounts();



    }
}
