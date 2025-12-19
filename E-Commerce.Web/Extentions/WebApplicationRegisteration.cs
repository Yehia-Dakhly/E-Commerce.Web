using DomainLayer.Contracts;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Web.Extentions
{
    public static class WebApplicationRegisteration
    {
        public static async Task SeedDataBaseAsync(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var ObjectOfDataSeeding = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();

            await ObjectOfDataSeeding.DataSeedAsync();
            await ObjectOfDataSeeding.IdentityDataSeedAsync();
        }
        public static IApplicationBuilder UseCustomExceptionMiddleWare(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomMiddleWares.CustomExceptionHandlerMiddleWare>();
            return app;
        }
        public static IApplicationBuilder UserSwaggerMiddleWares(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(Options =>
            {
                Options.ConfigObject = new Swashbuckle.AspNetCore.SwaggerUI.ConfigObject()
                {
                    DisplayRequestDuration = true
                };
                Options.DocumentTitle = "E-Commerce API";

                Options.JsonSerializerOptions = new System.Text.Json.JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                Options.DocExpansion(DocExpansion.None);
                Options.EnableFilter();
                Options.EnablePersistAuthorization();
            });
            return app;
        }
    }
}
