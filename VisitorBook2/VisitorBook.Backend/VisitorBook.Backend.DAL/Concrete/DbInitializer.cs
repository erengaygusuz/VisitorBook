using Microsoft.EntityFrameworkCore;
using VisitorBook.Backend.Core.Abstract;
using VisitorBook.Backend.DAL.Data;

namespace VisitorBook.Backend.DAL.Concrete
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _appDbContext;

        public DbInitializer(AppDbContext appDbContext) 
        { 
            _appDbContext = appDbContext;
        }

        public void Initialize()
        {
            try
            {
                if (_appDbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _appDbContext.Database.Migrate();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return;
        }
    }
}
