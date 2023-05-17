using NEST.DAL;

namespace NEST.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _dbContext;
        public LayoutService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Dictionary<string, string> GetService()
       {
           return _dbContext.setting.AsEnumerable().ToDictionary(x=>x.Key,x=>x.Value);
        }
    }
}
