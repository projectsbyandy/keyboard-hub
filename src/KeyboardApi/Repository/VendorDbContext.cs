using KeyboardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KeyboardAPI.Repository;

public class VendorDbContext : DbContext
{
    public VendorDbContext(DbContextOptions<VendorDbContext> options) : base(options)
    {
    }

    public DbSet<Vendor> Vendors => Set<Vendor>();
}