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
                    new() { Id = new Guid("035bb0ee-bd4b-40b5-b459-24e33e1647c0"), Name = "Prototypist", YearsActive = 12, Live = true },
                    new() { Id = Guid.NewGuid(), Name = "MyKbs", YearsActive = 2, Live = false },
                    new() { Id = new Guid("8e4dab9f-8bfa-4ba5-8048-5efc7bd06fa1"), Name = "MechUk", YearsActive = 3, Live = false }
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