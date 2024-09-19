using Microsoft.EntityFrameworkCore;
using ProxyDataAPI.Models;

namespace ProxyDataAPI.Data
{
    public class ProxyDbContext : DbContext
    {
        public ProxyDbContext(DbContextOptions<ProxyDbContext> options) : base(options) { }

        public DbSet<Proxy> Proxies { get; set; }
    }
}
