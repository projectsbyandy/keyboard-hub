using VendorApi.Models.Vendor;
using VendorApi.Services;
using ILogger = Serilog.ILogger;

namespace VendorApi.Endpoints;

public static class VendorApi
{
    public static void AddVendorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/vendor").RequireAuthorization();
        
        group.MapGet("all", GetVendorsAsync);
        group.MapGet("{vendorId}", GetVendorByIdAsync);
        group.MapPost("", CreateVendorAsync);
        group.MapPut("", UpdateVendorAsync);
        group.MapDelete("{vendorId}", DeleteVendorAsync);
    }

    private static async Task<IResult> GetVendorsAsync(IVendorService vendorService)
    {
        try
        {
            var vendors = await vendorService.GetAllAsync();
            return Results.Ok(vendors);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetVendorByIdAsync(Guid vendorId, IVendorService vendorService)
    {
        var vendor = await vendorService.GetAsync(vendorId);
        
        return vendor is null ? Results.NotFound() : Results.Ok(vendor);
    }

    private static async Task<IResult> CreateVendorAsync(VendorDetails vendorDetails, IVendorService vendorService, ILogger logger)
    {
        try
        {
            var outcome = await vendorService.CreateAsync(vendorDetails);
            
            return outcome is VendorOperationOutcome.Created
                ? Results.Created()
                : Results.Problem(outcome.ToString());
        }
        catch (Exception ex)
        { 
            logger.Error("Unable to create Vendor: {Name}, due to: {Reason}", vendorDetails.Name, ex.Message);
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateVendorAsync(VendorDetails vendorDetails, IVendorService vendorService , ILogger logger)
    {
        try
        {
            var outcome = await vendorService.UpdateAsync(vendorDetails);

            return outcome is VendorOperationOutcome.Updated
                ? Results.Ok($"Updated Vendor: {vendorDetails.Id}")
                : Results.BadRequest(outcome.ToString());
        }
        catch (Exception ex)
        {
            logger.Error("Unable to update Vendor: {Name}, due to: {Reason}", vendorDetails.Name, ex.Message);
            return Results.Problem(ex.Message);
        }
    }
    
    private static async Task<IResult> DeleteVendorAsync(Guid vendorId, IVendorService vendorService , ILogger logger)
    {
        try
        {
            var outcome = await vendorService.DeleteAsync(vendorId);

            return outcome is VendorOperationOutcome.Deleted
                ? Results.Ok($"Deleted Vendor: {vendorId}")
                : Results.Problem(outcome.ToString());
        }
        catch (Exception ex)
        {
            logger.Error("Unable to delete Vendor: {Id}, due to: {Reason}", vendorId, ex.Message);
            return Results.Problem(ex.Message);
        }
    } 
}