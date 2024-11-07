using Microsoft.EntityFrameworkCore;
using VendorApi.Models.Vendor;
using VendorApi.Repository.Vendor.Internal;
using ILogger = Serilog.ILogger;

namespace VendorApi.Services.Internal;

internal class VendorService(VendorDbContext context, ILogger logger) : IVendorService
{
    public async Task<IEnumerable<VendorDetails>> GetAllAsync()
    {
        try
        {
            return await context.Vendors.ToListAsync();
        }
        catch (Exception ex)
        {
            logger.Error("Problem getting Vendors from db due to:{@Error}", ex.Message);
            throw;
        }
    }

    public async Task<VendorDetails?> GetAsync(Guid id)
        => await context.Vendors.FindAsync(id);

    public async Task<VendorOperationOutcome> CreateAsync(VendorDetails vendorDetails)
    {
        try
        {
            var vendorToCreate = await context.Vendors.FindAsync(vendorDetails.Id);

            if (vendorToCreate is not null)
                return VendorOperationOutcome.AlreadyExists;

            await context.Vendors.AddAsync(vendorDetails);
            await context.SaveChangesAsync();

            return VendorOperationOutcome.Created;
        }
        catch (DbUpdateException ex)
        {
            logger.Error("Unable to create Vendor: {Name}, due to: {Reason}", vendorDetails.Name, ex.Message);
            throw;
        }
    }

    public async Task<VendorOperationOutcome> UpdateAsync(VendorDetails vendorDetails)
    {
        try
        {
            var vendorToUpdate = await context.Vendors.FindAsync(vendorDetails.Id);

            if (vendorToUpdate is null)
                return VendorOperationOutcome.DoesNotExist;

            vendorToUpdate.Name = vendorDetails.Name;
            vendorToUpdate.YearsActive = vendorDetails.YearsActive;
            vendorToUpdate.Live = vendorDetails.Live;

            await context.SaveChangesAsync();

            return VendorOperationOutcome.Updated;
        }
        catch (DbUpdateException ex)
        {
            logger.Error("Unable to Update Vendor: {Name}, due to: {Reason}", vendorDetails.Name, ex.Message);
            throw;
        }
    }

    public async Task<VendorOperationOutcome> DeleteAsync(Guid id)
    {
        try
        {
            var vendorToUpdate = await context.Vendors.FindAsync(id);

            if (vendorToUpdate is null)
                return VendorOperationOutcome.DoesNotExist;

            context.Remove(vendorToUpdate);
            await context.SaveChangesAsync();
            
            return VendorOperationOutcome.Deleted;
        }
        catch (DbUpdateException ex)
        {
            logger.Error("Unable to Delete Vendor: {Id}, due to: {Reason}", id, ex.Message);
            throw;
        }
    }
}