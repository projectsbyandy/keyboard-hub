namespace VendorApi.Repository.Vendor.Internal;

internal class DataSeeder(VendorDbContext vendorDbContext) : IDataSeeder
{
    private static readonly SemaphoreSlim Semaphore = new(1);
    private const int SemaphoreMaxWait = 1000;
    
    public async Task Seed()
    {
        await Semaphore.WaitAsync(SemaphoreMaxWait);

        try
        {
            if (vendorDbContext.Vendors.Any() is false)
            {
                var vendors = new List<Models.Vendor.VendorDetails>()
                {
                    new() { Id = new Guid("035bb0ee-bd4b-40b5-b459-24e33e1647c0"), Name = "Prototypist", Description = "We love Keyboards and Coffee", YearsActive = 12, IsLive = true },
                    new() { Id = Guid.NewGuid(), Name = "MyKbs", Description = "We aren't so active these days", YearsActive = 2, IsLive = false },
                    new() { Id = new Guid("8e4dab9f-8bfa-4ba5-8048-5efc7bd06fa1"), Description = "Sell and Design Keyboards", Name = "MechUk", YearsActive = 3, IsLive = false }
                };

                await vendorDbContext.AddRangeAsync(vendors);
                await vendorDbContext.SaveChangesAsync();
            }
        }
        finally
        {
            Semaphore.Release();
        }
    } 
}