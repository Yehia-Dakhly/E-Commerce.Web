using E_Commerce.Web.Extentions;
using Persistence;
using Service;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("AllowAll", Builder =>
                {
                    Builder.AllowAnyHeader();
                    Builder.AllowAnyMethod();
                    Builder.AllowAnyOrigin();
                });
            });
            builder.Services.AddSwaggerServices();
            builder.Services.AddInfrastructrueServices(builder.Configuration);
            builder.Services.AddAplicationServices();
            builder.Services.AddWebApplicationServices();
            builder.Services.AddJWTService(builder.Configuration);
            #endregion

            var app = builder.Build();

            #region DataSeeding
            await app.SeedDataBaseAsync();
            #endregion

            //app.Use(async (RequestContext, NextMiddleware) =>
            //{
            //    Console.WriteLine("Request Under Processing");
            //    await NextMiddleware.Invoke();
            //    Console.WriteLine("Waiting Response");
            //    Console.WriteLine(RequestContext.Response.Body);

            //});
            #region Configure the HTTP request pipeline - Middleware
            app.UseCustomExceptionMiddleWare();
            if (app.Environment.IsDevelopment())
            {
                app.UserSwaggerMiddleWares();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
