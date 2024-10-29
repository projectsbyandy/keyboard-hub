using System.Reflection;
using Ardalis.GuardClauses;
using Moq;
using Serilog;

namespace VendorApi.Integration.Tests.Setup;

public class ExternalServicesMock
{
    public Mock<ILogger> Logger { get; } = new();
    public IEnumerable<(Type, object)> GetMocks()
    {
        return GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(x =>
            {
                var underlyingType = x.PropertyType.GetGenericArguments()[0];
                var value = x.GetValue(this) as Mock;

                Guard.Against.Null(value, nameof(value));
                
                return (underlyingType, value.Object);
            })
            .ToArray();
    }
}