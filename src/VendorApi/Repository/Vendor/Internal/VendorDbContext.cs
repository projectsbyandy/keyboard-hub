using Microsoft.EntityFrameworkCore;
using VendorApi.Models.Vendor;

namespace VendorApi.Repository.Vendor.Internal;

internal class VendorDbContext(DbContextOptions<VendorDbContext> options) : DbContext(options)
{
    public DbSet<VendorDetails> Vendors => Set<VendorDetails>();
}