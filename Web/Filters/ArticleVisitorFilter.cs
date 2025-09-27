
using BlogWebApp.Data.UnitOfWorks;
using Microsoft.AspNetCore.Mvc.Filters;
using BlogWebApp.Entity.Entities;
namespace Web.Filters
{
    public class ArticleVisitorFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWork unitOfWork;

        public ArticleVisitorFilter(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            List<Visitor> visitors = await unitOfWork.GetRepository<Visitor>().GetAllAsync();


            string getIp = context.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "unknown";
            string getUserAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();


            Visitor visitor = new(getIp, getUserAgent);


            if (!visitors.Any(x => x.IpAddress == visitor.IpAddress))
            {
                await unitOfWork.GetRepository<Visitor>().AddAsync(visitor);
                await unitOfWork.SaveAsync(); 
            }

            
            await next();

        }
    }
}
