using Application.Mappings;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Tests.TestHelpers;

internal static class MapperFactory
{
    public static IMapper Create()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }
}
