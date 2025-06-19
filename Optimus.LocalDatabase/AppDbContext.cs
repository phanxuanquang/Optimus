using Microsoft.EntityFrameworkCore;

namespace Optimus.LocalDatabase
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public static readonly string DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Optimus", "AskDb-v0.0.0.sqlite");
    }
}
