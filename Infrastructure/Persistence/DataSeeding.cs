using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext,
        UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager,
        StoreIdentityDbContext _IdentityDbContext) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                var PendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                if (PendingMigrations.Any())
                {
                    await _dbContext.Database.MigrateAsync();
                }

                if (!_dbContext.ProductBrands.Any())
                {
                    //var ProductBrandsData = await File.ReadAllTextAsync("..\\Infrastructure\\Persistence\\Data\\DataSeed\\brands.json");
                    var ProductBrandsData = File.OpenRead("..\\Infrastructure\\Persistence\\Data\\DataSeed\\brands.json");
                    // Convert String to C# objects [ProductBrand]
                    var ProductBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(ProductBrandsData);
                    if (ProductBrands is not null && ProductBrands.Any())
                        await _dbContext.AddRangeAsync(ProductBrands);
                }
                if (!_dbContext.ProductTypes.Any())
                {
                    var ProductTypesData = File.OpenRead("..\\Infrastructure\\Persistence\\Data\\DataSeed\\types.json");
                    var ProductTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(ProductTypesData);
                    if (ProductTypes is not null && ProductTypes.Any())
                        await _dbContext.AddRangeAsync(ProductTypes);
                }   
                if (!_dbContext.Products.Any())
                {
                    var ProductsData = File.OpenRead("..\\Infrastructure\\Persistence\\Data\\DataSeed\\products.json");
                    var Products = await JsonSerializer.DeserializeAsync<List<Product>>(ProductsData);
                    if (Products is not null && Products.Any())
                        await _dbContext.AddRangeAsync(Products);
                }
                if (!_dbContext.Set<DeliveryMethod>().Any())
                {
                    var DeliverysData = File.OpenRead("..\\Infrastructure\\Persistence\\Data\\DataSeed\\delivery.json");
                    var DeliveryMethods = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(DeliverysData);
                    if (DeliveryMethods is not null && DeliveryMethods.Any())
                        await _dbContext.Set<DeliveryMethod>().AddRangeAsync(DeliveryMethods);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // TODO
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {

                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser()
                    {
                        Email = "YehiaDakhly@gmail.com",
                        DisplayName = "Yehia Dakhly",
                        PhoneNumber = "01122878399",
                        UserName = "YehiaDakhly"
                    };
                    var User02 = new ApplicationUser()
                    {
                        Email = "SaleemYehiaDakhly@gmail.com",
                        DisplayName = "Saleem Dakhly",
                        PhoneNumber = "01122878399",
                        UserName = "SaleemDakhly"
                    };
                    await _userManager.CreateAsync(User01, "P@ssw0rd");
                    await _userManager.CreateAsync(User02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(User01, "SuperAdmin");
                    await _userManager.AddToRoleAsync(User02, "Admin");
                }
                //await _IdentityDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
