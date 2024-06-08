namespace KeyboardApi.Repository.Vendor;

public class DataSeeder : IDataSeeder
{
    private readonly VendorDbContext _vendorDbContext;

    public DataSeeder(VendorDbContext vendorDbContext)
    {
        _vendorDbContext = vendorDbContext;
    }
    
    public async Task Seed()
    {
        if (_vendorDbContext.Vendors.Any() is false)
        {
            var vendors = new List<Keyboard.Common.Models.Vendor>()
            {
                new() { Id = Guid.NewGuid(), Name = "Prototypist", YearsActive = 12, Live = true},
                new() { Id = Guid.NewGuid(), Name = "MyKbs", YearsActive = 2, Live = false},
                new() { Id = Guid.NewGuid(), Name = "MechUk", YearsActive = 99, Live = true}
            };

            await _vendorDbContext.AddRangeAsync(vendors);
            await _vendorDbContext.SaveChangesAsync();
        }
    } 
}