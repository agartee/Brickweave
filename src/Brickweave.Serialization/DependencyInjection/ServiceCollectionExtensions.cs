using System;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Serialization.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static SerializationOptionsBuilder AddBrickweaveSerialization(this IServiceCollection services,
            params Type[] shorthandTypes)
        {
            return new SerializationOptionsBuilder(services, shorthandTypes);
        }
    }
}
