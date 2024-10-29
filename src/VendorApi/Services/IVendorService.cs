using VendorApi.Models.Vendor;

namespace VendorApi.Services;

public interface IVendorService
{
    public Task<IEnumerable<VendorDetails>> GetAllAsync();
    public Task<VendorDetails?> GetAsync(Guid id);
    public Task<VendorOperationOutcome> CreateAsync(VendorDetails vendorDetails);
    public Task<VendorOperationOutcome> UpdateAsync(VendorDetails vendorDetails);
    public Task<VendorOperationOutcome> DeleteAsync(Guid id);

}