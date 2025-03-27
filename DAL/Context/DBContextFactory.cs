//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;

//namespace DAL.Context
//{
//    public class DBContextFactory : IDesignTimeDbContextFactory<NotificationsDbContext>
//    {
//        public NotificationsDbContext CreateDbContext(string[] args)
//        {
//            IConfiguration config = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory())
//                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//                .Build();

//            var connectionString = config.GetConnectionString("BaseConnection");

//            var options = new DbContextOptionsBuilder<NotificationsDbContext>();
//            options.UseSqlite(connectionString);


//            return new NotificationsDbContext(options.Options);
//        }
//    }
//}
