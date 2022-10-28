using APP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using UsuariosApi.Models;

namespace UsuariosApi.Data
{
    public class UserDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {

        public UserDbContext(DbContextOptions<UserDbContext> opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            IdentityUser<int> admin = new IdentityUser<int>
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = 9999
            };

            PasswordHasher<IdentityUser<int>> hasher = new PasswordHasher<IdentityUser<int>>();
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

            builder.Entity<IdentityUser<int>>().HasData(admin);

            builder.Entity<IdentityRole<int>>().HasData(
                    new IdentityRole<int> { Id = 9999, Name = "admin", NormalizedName = "ADMIN" }
            );

            builder.Entity<IdentityRole<int>>().HasData(
                    new IdentityRole<int> { Id = 9997, Name = "responsavel", NormalizedName = "RESPONSAVEL" }
            );
            builder.Entity<IdentityRole<int>>().HasData(
                    new IdentityRole<int> { Id = 9996, Name = "idoso", NormalizedName = "IDOSO" }
            );

            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { RoleId = 9999, UserId = 9999 }
                );
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UsuarioAssistido> UsuarioAssistido { get; set; }
        public DbSet<Tarefa> Tarefa { get; set; }
        public DbSet<TarefaExcluida> Tarefa_Excluida { get; set; }


    }
}