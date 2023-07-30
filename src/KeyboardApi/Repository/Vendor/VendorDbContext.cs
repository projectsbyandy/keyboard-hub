using Microsoft.EntityFrameworkCore;

namespace KeyboardApi.Repository.Vendor;

public class VendorDbContext : DbContext
{
    public VendorDbContext(DbContextOptions<VendorDbContext> options) : base(options)
    {
    }

    public DbSet<Models.Vendor> Vendors => Set<Models.Vendor>();
}