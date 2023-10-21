using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorBook.Core.Abstract;
using VisitorBook.DAL.Data;

namespace VisitorBook.DAL.Concrete
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
