using Da.Data.Entities;
using Da.Data.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Da.Data.Extensions
{
    public static class ModelBuiderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
                new AppConfig() { Key = "HomeTitle", Value = "This is home page of Cafe" },
                new AppConfig() { Key = "HomeKeywork", Value = "This is kryword of Cafe" },
                new AppConfig() { Key = "HomeDescription", Value = "This is decription of Cafe" }
                );
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi", Name = "tiếng Việt", IsDefault = true },
                new Language() { Id = "en", Name = "English", IsDefault = false });
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id=1,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOder = 1,
                    Status = Status.Active,  
                },
                new Category()
                {
                    Id=2,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOder = 2,
                    Status = Status.Active,
                });
            modelBuilder.Entity<CategoryTranslation>().HasData(
                        new CategoryTranslation() { Id = 1, CategoryId = 1, Name = "Cafe1", LanguageId = "vi", SeoAlias = "cafe1v", SeoDescription = "Cà phê hòa tan1", SeoTitle = "Cà phê hòa tan1" },
                        new CategoryTranslation() { Id = 2, CategoryId = 1, Name = "Cafe1", LanguageId = "en", SeoAlias = "cafe1n", SeoDescription = "Cà 1", SeoTitle = "Cà 1" },
                        new CategoryTranslation() { Id = 3, CategoryId = 2, Name = "Cafe2", LanguageId = "vi", SeoAlias = "cafe2v", SeoDescription = "Cà phê hòa tan", SeoTitle = "Cà phê hòa tan" },
                        new CategoryTranslation() { Id = 4, CategoryId = 2, Name = "Cafe2", LanguageId = "en", SeoAlias = "cafe2n", SeoDescription = "Cafe2", SeoTitle = "Cafe2" }
                );
            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    DateCreated = DateTime.Now,
                    OriginalPrice = 100000,
                    Price = 200000,
                    Stock = 0,
                    ViewCount = 0,
                });
            modelBuilder.Entity<ProductTransLation>().HasData(
                new ProductTransLation() {
                    Id = 1,
                    ProductId = 1,
                    Name = "Cafe net",
                    LanguageId = "vi",
                    SeoAlias = "cafe-net",
                    SeoDesCription = "Cafe net",
                    SeoTitle = "Cafe net",
                    Details = "Cafe net",
                    Description = "Cafe net" },

                new ProductTransLation()
                {
                    Id=2,
                    ProductId=1,
                    Name = "Cafe ",
                    LanguageId = "en",
                    SeoAlias = "cafe-net",
                    SeoDesCription = "Cafe net",
                    SeoTitle = "Cafe net",
                    Details = "Cafe net",
                    Description = "Cafe net" 
                }) ;

            modelBuilder.Entity<ProductInCategory>().HasData(
                        new ProductInCategory(){ProductId=1,CategoryId=1}
                 );

            //
            var roleId = new Guid("28930550 - 9CC4 - 44D2 - 877F - 70FD2C7B292E");
            var adminId = new Guid("C9077C0B-AA64-4F01-8FF5-D965365C5CAE");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "cafe@gmail.com",
                NormalizedEmail = "cafe@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Abcd1234$"),
                SecurityStamp = string.Empty,
                FirstName = "duc",
                LastName = "trung",
                Dob = new DateTime(2021, 01, 31)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });

        }
    }
}
