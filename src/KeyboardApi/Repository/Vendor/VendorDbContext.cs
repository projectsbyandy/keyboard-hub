using Microsoft.EntityFrameworkCore;
namespace KeyboardApi.Repository.Vendor;

public class VendorDbContext : DbContext
{
    public VendorDbContext(DbContextOptions<VendorDbContext> options) : base(options)
    {
    }

    public DbSet<Keyboard.Common.Models.Vendor> Vendors => Set<Keyboard.Common.Models.Vendor>();
}