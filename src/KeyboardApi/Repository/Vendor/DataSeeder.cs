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
            var vendors = new List<Models.Vendor>()
            {
                new() { Id = Guid.NewGuid(), Name = "Prototypist", Brands = new() {"NovelKeys", "TGR", "Geon"}},
                new() { Id = Guid.NewGuid(), Name = "MyKbs", Brands = new() {"Keykobo", "CYSM", "Amazon"} },
                new() { Id = Guid.NewGuid(), Name = "MechUk", Brands = new List<string>() {"GMK", "Cherry", "Apple"} }
            };

            await _vendorDbContext.AddRangeAsync(vendors);
            await _vendorDbContext.SaveChangesAsync();
        }
    } 
}