namespace CleanArchitecture.API.Routes
{
    public static class StoreRoutes
    {
        public static void MapStoreRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/stores", async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });                
        }
    }
}
