namespace API.Modules.AccessControl
{
    public static class AccessControlEndpoints
    {
        public static IEndpointRouteBuilder MapAccessControlEndpoints(
            this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/api/access-control");

            group.MapGet(
                "/health",
                () => Results.Ok(
                    new
                    {
                        Context = "AccessControl",
                        Status = "Healthy"
                    }));

            return endpoints;
        }
    }
}