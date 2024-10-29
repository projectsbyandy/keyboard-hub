namespace VendorApi.Repository.Vendor.Internal;

internal class DataSeeder(VendorDbContext vendorDbContext) : IDataSeeder
{
    public async Task Seed()
    {
        if (vendorDbContext.Vendors.Any() is false)
        {
            var vendors = new List<Models.Vendor.VendorDetails>()
            {
                new() { Id = Guid.NewGuid(), Name = "Prototypist", YearsActive = 12, Live = true},
                new() { Id = Guid.NewGuid(), Name = "MyKbs", YearsActive = 2, Live = false},
                new() { Id = Guid.NewGuid(), Name = "MechUk", YearsActive = 99, Live = true}
            };

            await vendorDbContext.AddRangeAsync(vendors);
            await vendorDbContext.SaveChangesAsync();
        }
    } 
}